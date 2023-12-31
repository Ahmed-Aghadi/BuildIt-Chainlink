// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.13;

// import "solmate/tokens/ERC1155.sol";
import {ERC1155} from "openzeppelin/token/ERC1155/ERC1155.sol";
import "solmate/utils/LibString.sol";
// import "solmate/auth/Owned.sol";
import "openzeppelin/access/Ownable.sol";
import "openzeppelin/metatx/ERC2771Context.sol";
import {IRouterClient} from "@chainlink/contracts-ccip/src/v0.8/ccip/interfaces/IRouterClient.sol";
import {OwnerIsCreator} from "@chainlink/contracts-ccip/src/v0.8/shared/access/OwnerIsCreator.sol";
import {Client} from "@chainlink/contracts-ccip/src/v0.8/ccip/libraries/Client.sol";
import {CCIPReceiver} from "@chainlink/contracts-ccip/src/v0.8/ccip/applications/CCIPReceiver.sol";
import {IAny2EVMMessageReceiver} from "@chainlink/contracts-ccip/src/v0.8/ccip/interfaces/IAny2EVMMessageReceiver.sol";
import {LinkTokenInterface} from "chainlink/v0.8/interfaces/LinkTokenInterface.sol";
import {IERC1155MetadataURI} from "openzeppelin/token/ERC1155/extensions/IERC1155MetadataURI.sol";
import {IERC1155} from "openzeppelin/token/ERC1155/IERC1155.sol";
import {IERC165} from "forge-std/interfaces/IERC1155.sol";

contract Utils is ERC2771Context, ERC1155, Ownable, CCIPReceiver {
    error CrossChainNotSupported();
    error InvalidChain();
    error InsufficientBalance();
    string public baseUri;
    uint256 public utilCount;
    LinkTokenInterface public immutable i_link;
    // IRouterClient public immutable i_router;
    mapping(uint64 => address) public chains;
    mapping(uint64 => uint64) public chainIdToChainSelector;

    constructor(
        string memory _baseUri,
        address trustedForwarder,
        address _linkAddress,
        address _router
    ) ERC2771Context(trustedForwarder) ERC1155(_baseUri) CCIPReceiver(_router) {
        baseUri = _baseUri;
        // i_router = IRouterClient(_router);
        i_link = LinkTokenInterface(_linkAddress);
    }

    function setChain(uint64 chain, address addr) public onlyOwner {
        chains[chain] = addr;
    }

    function setChainSelector(uint64 chain, uint64 selector) public onlyOwner {
        chainIdToChainSelector[chain] = selector;
    }

    function crossChainTransfer(
        uint64 destinationChain,
        uint tokenId,
        uint amount
    ) public payable returns (bytes32 messageId) {
        address destinationAddress = chains[
            chainIdToChainSelector[destinationChain]
        ];
        if (destinationAddress != address(0)) {
            revert InvalidChain();
        }
        if (amount > balanceOf(_msgSender(), tokenId)) {
            revert InsufficientBalance();
        }
        _burn(_msgSender(), tokenId, amount);

        bytes memory payload = abi.encode(tokenId, amount, _msgSender());

        // Create an EVM2AnyMessage struct in memory with necessary information for sending a cross-chain message
        Client.EVM2AnyMessage memory evm2AnyMessage = Client.EVM2AnyMessage({
            receiver: abi.encode(destinationAddress), // ABI-encoded receiver address
            data: abi.encode(payload), // ABI-encoded string
            tokenAmounts: new Client.EVMTokenAmount[](0), // Empty array indicating no tokens are being sent
            extraArgs: Client._argsToBytes(
                // Additional arguments, setting gas limit and non-strict sequencing mode
                Client.EVMExtraArgsV1({gasLimit: 200_000, strict: false})
            ),
            // Set the feeToken  address, indicating LINK will be used for fees
            feeToken: address(i_link)
        });

        // Get the fee required to send the message
        uint256 fees = IRouterClient(i_router).getFee(
            chainIdToChainSelector[destinationChain],
            evm2AnyMessage
        );

        i_link.transferFrom(_msgSender(), address(this), fees);

        // approve the Router to transfer LINK tokens on contract's behalf. It will spend the fees in LINK
        i_link.approve(address(i_router), fees);

        // Send the message through the router and store the returned message ID
        messageId = IRouterClient(i_router).ccipSend(
            destinationChain,
            evm2AnyMessage
        );
    }

    function _ccipReceive(
        Client.Any2EVMMessage memory any2EvmMessage
    ) internal override {
        if (
            keccak256(
                abi.encodePacked(
                    chains[
                        chainIdToChainSelector[
                            any2EvmMessage.sourceChainSelector
                        ]
                    ]
                )
            ) !=
            keccak256(
                abi.encodePacked(abi.decode(any2EvmMessage.sender, (address)))
            )
        ) {
            revert InvalidChain();
        }
        (uint tokenId, uint amount, address sender) = abi.decode(
            any2EvmMessage.data,
            (uint, uint, address)
        );
        _mint(sender, tokenId, amount, "");
    }

    function mintMore(uint id, uint amount) public onlyOwner {
        _mint(_msgSender(), id, amount, "");
    }

    function mint(uint256 amount) public onlyOwner {
        utilCount += 1;
        _mint(_msgSender(), utilCount, amount, "");
    }

    function getLinkFees(
        uint64 destinationChain,
        uint tokenId,
        uint amount,
        address msgSender
    ) external view returns (uint256 fees) {
        address destinationAddress = chains[
            chainIdToChainSelector[destinationChain]
        ];

        bytes memory payload = abi.encode(tokenId, amount, msgSender);

        // Create an EVM2AnyMessage struct in memory with necessary information for sending a cross-chain message
        Client.EVM2AnyMessage memory evm2AnyMessage = Client.EVM2AnyMessage({
            receiver: abi.encode(destinationAddress), // ABI-encoded receiver address
            data: abi.encode(payload), // ABI-encoded string
            tokenAmounts: new Client.EVMTokenAmount[](0), // Empty array indicating no tokens are being sent
            extraArgs: Client._argsToBytes(
                // Additional arguments, setting gas limit and non-strict sequencing mode
                Client.EVMExtraArgsV1({gasLimit: 200_000, strict: false})
            ),
            // Set the feeToken  address, indicating LINK will be used for fees
            feeToken: address(i_link)
        });

        // Get the fee required to send the message
        fees = IRouterClient(i_router).getFee(
            chainIdToChainSelector[destinationChain],
            evm2AnyMessage
        );
    }

    // Add `virtual` to function signature of `supportsInterface` function in CCIPReceiver.sol
    function supportsInterface(
        bytes4 interfaceId
    ) public pure override(ERC1155, CCIPReceiver) returns (bool) {
        return
            interfaceId == type(IERC1155).interfaceId ||
            interfaceId == type(IERC1155MetadataURI).interfaceId ||
            interfaceId == type(IAny2EVMMessageReceiver).interfaceId ||
            interfaceId == type(IERC165).interfaceId ||
            CCIPReceiver.supportsInterface(interfaceId) ||
            super.supportsInterface(interfaceId);
    }

    function uri(
        uint256 id
    ) public view virtual override returns (string memory) {
        return string(abi.encodePacked(baseUri, LibString.toString(id)));
    }

    function _msgSender()
        internal
        view
        virtual
        override(Context, ERC2771Context)
        returns (address sender)
    {
        return ERC2771Context._msgSender();
    }

    function _msgData()
        internal
        view
        virtual
        override(Context, ERC2771Context)
        returns (bytes calldata)
    {
        return ERC2771Context._msgData();
    }
}
