using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;
using Newtonsoft.Json;
using System.Text.Json;
using System.Numerics;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Nethereum.Web3;
using System.Text.Json.Nodes;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class AxelarGasResponse
{
    public string res
    {
        get;
        set;
    }
}

public struct Land
{
    public BigInteger xIndex;
    public BigInteger yIndex;
}
public struct Listing
{
    public int id;
    public string sellerAddress;
    public bool inUSD;
    public int landId;
    public int xIndex;
    public int yIndex;
    public string price;
    public int timestamp;
    public bool isValid;
    public bool isAuction;
    public int auctionTime;
}
public struct Listings
{
    public List<Listing> listings;
}

public class ContractManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SignIn();
    public const int EMPTY = 0;
    public const int ROAD = 1;
    public const int HOUSE = 2;
    public const int SPECIAL = 3;

    /*    string mapContractAddressXDCApothem = "0x06ce5b276a53e072dc3144d3746e57fd2ca6a1b4";
        string utilsContractAddressXDCApothem = "0x489d47e592639ba11107e84dd6cca08f0892e27d";
        string faucetContractAddressXDCApothem = "0xe2c149c4cb26f137e7eab87e7675be71e53d7071";

        string mapContractAddressOkexTestnet = "0x11DA0f57086a19977E46B548b64166411d839a30";
        string utilsContractAddressOkexTestnet = "0xCA34FF4068f042203087D475805c4DD8347cE958";
        string faucetContractAddressOkexTestnet = "0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E";*/

    string mapContractAddressPolygonZKEVMTestnet = "0x9ea48FE0c9b0ae6e3E27Dec076800D9347c2E0D7";
    string utilsContractAddressPolygonZKEVMTestnet = "0x4f033bF08e610DDeBe5fA9707d5334Ad5c5A893e";
    string faucetContractAddressPolygonZKEVMTestnet = "0x4B03368f666fa7579BfeB49eF1c5E405389b174e";
    string marketplaceContractAddressPolygonZKEVMTestnet = "0x1b62E60b85678F5FF2fd6a8c27FB9dC7d5e1e2D4";

    /*    string mapContractAddressTheta = "0x9c0549334fbCDF0AFc85B83B2Bb4b422Ba38960f";
        string utilsContractAddressTheta = "0x3EC31e8B991FF0b3FfffD480e2A5F259B51DdF5c";
        string faucetContractAddressTheta = "0xc32Fb0cb3D6CcfD111146a2CA26A3FC774F3ECd2";*/

    string mapContractAddressMumbai = "0x21148779871A17056025C93bdC9E83334b93fe3d";
    string utilsContractAddressMumbai = "0x82BFe300311f0324423406382fC6bdccbC2BaB47";
    string faucetContractAddressMumbai = "0x2984288408926fe760673767804a8301d7DB6ae3";
    string marketplaceContractAddressMumbai = "0xf4EeB57859a5b2e98f747E918d414D74CecE0D6b";

    string mapContractAddressSepolia = "0x5452fD02e04Abc7101588014BB7A44d1e761e261";
    string utilsContractAddressSepolia = "0xE9cEAe69B724F4340Ca3D6D2F0D147b0Bc3E1978";
    string faucetContractAddressSepolia = "0xA9b084B2Cc9918C5fF6c66138C1cC5D02954F442";
    string marketplaceContractAddressSepolia = "0xf20017025433b14295CF630a8392B31A2A39c00B";

    /*    string mapContractAddressMantleTestnet = "0x9f85f13F5FFb3b1dcb7B318F1712b6f42A4CFFd4";
        string utilsContractAddressMantleTestnet = "0x8e539DfdA07e5Bb63F6768eaDb800F01FC25C336";
        string faucetContractAddressMantleTestnet = "0x69A46a7b195eb6C89454C41dE3e5Bd96C694D8FB";
        string marketplaceContractAddressMantleTestnet = "0xcB82Ac8Ad0cd14DD4d6f6397B66Bf11dA538F12A";

        string mapContractAddressFantomTestnet = "0xB834239DA8b8e8478b67a7789b1bc3af0b15690b";
        string utilsContractAddressFantomTestnet = "0x8E10a436eafE80B2388D56e8Bb4435C31C930dbf";
        string faucetContractAddressFantomTestnet = "0x7E4a77cD71f7Be781Ba3f91be460109a0e25d186";
        string marketplaceContractAddressFantomTestnet = "0x120F7784cbA30Ea1dE4d2B18CF76b766C0678E41";

        string mapContractAddressFantom = "0x91db12f3ea6f4598c982d46e8fdc72b53c333afb";
        string utilsContractAddressFantom = "0x4a4e6cc94507b6ad2c91ad765d3f5b566b15d895";
        string faucetContractAddressFantom = "0x724257edfe7f3bbf8c06a01ae3becb48dc5e220a";
        string marketplaceContractAddressFantom = "0x20294525826458177030954af848d783f733a80a";

        string mapContractAddressArbitrumGoerli = "0x91db12F3eA6F4598c982D46e8Fdc72B53c333AFb";
        string utilsContractAddressArbitrumGoerli = "0x4A4e6Cc94507B6aD2c91aD765d3f5B566B15d895";
        string faucetContractAddressArbitrumGoerli = "0x724257edfe7f3bbf8c06a01ae3becb48dc5e220a";
        string marketplaceContractAddressArbitrumGoerli = "0x20294525826458177030954af848d783f733a80a";

        string mapContractAddressBSCTestnet = "0x9574E60E8aBeb8062CD3DCC3ed7714E067768a72";
        string utilsContractAddressBSCTestnet = "0x2e4dDe518EB8B63C47D388aa129386d9ca110a45";
        string faucetContractAddressBSCTestnet = "0x816d0366156F406069223D5E454Ab8c98E6D4440";
        string marketplaceContractAddressBSCTestnet = "0xB20CDCaF8636Efee0730Ac5fe37842afA13909e6";

        string mapContractAddressOPBNBTestnet = "0xdf78d5a57dcff31ca18978b56760867010aebc2e";
        string utilsContractAddressOPBNBTestnet = "0x11da0f57086a19977e46b548b64166411d839a30";
        string faucetContractAddressOPBNBTestnet = "0x76cfdE04F691B93c9993Be24d5FE7667E7A8782C";
        string marketplaceContractAddressOPBNBTestnet = "0x489d47E592639Ba11107E84dd6CCA08F0892E27d";*/

    string mapContractAddressGoerli = "0x5720d7a63FeFf600fAA72A0C97361D490DEab903";
    string utilsContractAddressGoerli = "0x52Cb4B27503848ABd8dd3629474835299E1E99af";
    string faucetContractAddressGoerli = "0x574F8B3f91d179F808bdDD879020164a1A317Daa";
    string marketplaceContractAddressGoerli = "0x14AADF21340053a7403B0D30625a0289E30bF8CF";

    string mapContractAddressAvalancheFujiTestnet = "0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E";
    string utilsContractAddressAvalancheFujiTestnet = "0x11DA0f57086a19977E46B548b64166411d839a30";
    string faucetContractAddressAvalancheFujiTestnet = "0x76cfdE04F691B93c9993Be24d5FE7667E7A8782C";
    string marketplaceContractAddressAvalancheFujiTestnet = "0x489d47E592639Ba11107E84dd6CCA08F0892E27d";

    string mapContractAddress = "0x3808000749fe82e14DEDd15510A2B1E1b076Ea5A";
    string mapContractABI = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_size\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"_perSize\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"_baseUri\",\"type\":\"string\"},{\"internalType\":\"address\",\"name\":\"_utilsAddress\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"InvalidLength\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidXIndex\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidYIndex\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"LandAlreadyOwned\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotOwner\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"SizeNotDivisibleByPerSize\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"ZeroPerSize\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"ZeroSize\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseUri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"land\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"xIndex\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"yIndex\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"landCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"landIds\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"map\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"xIndex\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"yIndex\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155BatchReceived\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155Received\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"perSize\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"x\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"y\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"utilId\",\"type\":\"uint256\"}],\"name\":\"placeItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256[]\",\"name\":\"x\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"y\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"utilId\",\"type\":\"uint256[]\"}],\"name\":\"placeItems\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"x\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"y\",\"type\":\"uint256\"}],\"name\":\"removeItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"size\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"x\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"y\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"utilId\",\"type\":\"uint256\"}],\"name\":\"updateItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256[]\",\"name\":\"x\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"y\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"utilId\",\"type\":\"uint256[]\"}],\"name\":\"updateItems\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"utilCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"utilsAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    string utilsContractAddress = "0xC5f5C446fe1fEd411464C7d5E60c9B2d4c139F14";
    string utilsContractABI = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"_baseUri\",\"type\":\"string\"},{\"internalType\":\"address\",\"name\":\"trustedForwarder\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"gateway_\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"gasReceiver_\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"CrossChainNotSupported\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InsufficientBalance\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidAddress\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidChain\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotApprovedByGateway\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseUri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"name\":\"chains\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"destinationChain\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"crossChainTransfer\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"commandId\",\"type\":\"bytes32\"},{\"internalType\":\"string\",\"name\":\"sourceChain\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"sourceAddress\",\"type\":\"string\"},{\"internalType\":\"bytes\",\"name\":\"payload\",\"type\":\"bytes\"}],\"name\":\"execute\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"commandId\",\"type\":\"bytes32\"},{\"internalType\":\"string\",\"name\":\"sourceChain\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"sourceAddress\",\"type\":\"string\"},{\"internalType\":\"bytes\",\"name\":\"payload\",\"type\":\"bytes\"},{\"internalType\":\"string\",\"name\":\"tokenSymbol\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"executeWithToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"gasService\",\"outputs\":[{\"internalType\":\"contract IAxelarGasService\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"gateway\",\"outputs\":[{\"internalType\":\"contract IAxelarGateway\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"forwarder\",\"type\":\"address\"}],\"name\":\"isTrustedForwarder\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mintMore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"chain\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"addr\",\"type\":\"string\"}],\"name\":\"setChain\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"utilCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    string faucetContractAddress = "0x4EE91D3dD15259dDfcf71d9F92e9007812B850f2";
    string faucetContractABI = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"tokenAddress\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155BatchReceived\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155Received\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
    string marketplaceContractAddress = "0x75E21b9484d857098BA1E83FAE31E60C95f0afE5";
    string marketplaceContractABI = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"eth_usd_priceFeedAddress\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"mapAddress\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"utilsAddress\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_linkAddress\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_registrar\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_registryAddress\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_gasLimit\",\"type\":\"uint256\"},{\"internalType\":\"address\",\"name\":\"trustedForwarder\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"AuctioNotSupported\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"AuctionCantBeBought\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"AuctionNotOver\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"AuctionOver\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"BidNotHighEnough\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidListing\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidListingId\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidPrice\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidTokenId\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NoBids\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotAuction\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotEnoughFunds\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotHighestBidder\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotListingOwner\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotTokenOwner\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"USDNotSupported\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"USDNotSupportedForAuction\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"ValidListing\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"auctionBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"balances\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"bid\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"buyListing\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"calculateWinner\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes\",\"name\":\"checkData\",\"type\":\"bytes\"}],\"name\":\"checkUpkeep\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"upkeepNeeded\",\"type\":\"bool\"},{\"internalType\":\"bytes\",\"name\":\"performData\",\"type\":\"bytes\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bool\",\"name\":\"inUSD\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"internalType\":\"bool\",\"name\":\"isAuction\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"auctionTime\",\"type\":\"uint256\"},{\"internalType\":\"uint96\",\"name\":\"amount\",\"type\":\"uint96\"}],\"name\":\"createListing\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"deleteListing\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"eth_usd_priceFeed\",\"outputs\":[{\"internalType\":\"contract AggregatorV3Interface\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"gasLimit\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"getPrice\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"highestBid\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"bidder\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"i_link\",\"outputs\":[{\"internalType\":\"contract LinkTokenInterface\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"i_registry\",\"outputs\":[{\"internalType\":\"contract AutomationRegistryInterface\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"invalidateAuctionBid\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"listingId\",\"type\":\"uint256\"}],\"name\":\"isListingValid\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"forwarder\",\"type\":\"address\"}],\"name\":\"isTrustedForwarder\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"listingCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"listingToUpkeepID\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"listings\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"seller\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"inUSD\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"timestamp\",\"type\":\"uint256\"},{\"internalType\":\"bool\",\"name\":\"isValid\",\"type\":\"bool\"},{\"internalType\":\"bool\",\"name\":\"isAuction\",\"type\":\"bool\"},{\"internalType\":\"uint256\",\"name\":\"aucionTime\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155BatchReceived\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC1155Received\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC721Received\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes\",\"name\":\"performData\",\"type\":\"bytes\"}],\"name\":\"performUpkeep\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"registrar\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
    string linkTokenContractAddress = "";
    string linkTokenContractABI = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseApproval\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseApproval\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"transferAndCall\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"success\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"typeAndVersion\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"pure\",\"type\":\"function\"}]";

    string utilsLxLyContractABI = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"_baseUri\",\"type\":\"string\"},{\"internalType\":\"address\",\"name\":\"trustedForwarder\",\"type\":\"address\"},{\"internalType\":\"contract IPolygonZkEVMBridge\",\"name\":\"_polygonZkEVMBridge\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"InsufficientBalance\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidChain\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"NotPolygonZkEVMBridge\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseUri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"name\":\"chains\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"destinationChain\",\"type\":\"uint32\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bool\",\"name\":\"forceUpdateGlobalExitRoot\",\"type\":\"bool\"}],\"name\":\"crossChainTransfer\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"forwarder\",\"type\":\"address\"}],\"name\":\"isTrustedForwarder\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mintMore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"networkID\",\"outputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sourceAddress\",\"type\":\"address\"},{\"internalType\":\"uint32\",\"name\":\"sourceChain\",\"type\":\"uint32\"},{\"internalType\":\"bytes\",\"name\":\"payload\",\"type\":\"bytes\"}],\"name\":\"onMessageReceived\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"polygonZkEVMBridge\",\"outputs\":[{\"internalType\":\"contract IPolygonZkEVMBridge\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"chain\",\"type\":\"uint32\"},{\"internalType\":\"address\",\"name\":\"addr\",\"type\":\"address\"}],\"name\":\"setChain\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"utilCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    string utilsCCIPContractABI = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"_baseUri\",\"type\":\"string\"},{\"internalType\":\"address\",\"name\":\"trustedForwarder\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_linkAddress\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_router\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"CrossChainNotSupported\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InsufficientBalance\",\"type\":\"error\"},{\"inputs\":[],\"name\":\"InvalidChain\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"router\",\"type\":\"address\"}],\"name\":\"InvalidRouter\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"baseUri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"components\":[{\"internalType\":\"bytes32\",\"name\":\"messageId\",\"type\":\"bytes32\"},{\"internalType\":\"uint64\",\"name\":\"sourceChainSelector\",\"type\":\"uint64\"},{\"internalType\":\"bytes\",\"name\":\"sender\",\"type\":\"bytes\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"},{\"components\":[{\"internalType\":\"address\",\"name\":\"token\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"internalType\":\"struct Client.EVMTokenAmount[]\",\"name\":\"destTokenAmounts\",\"type\":\"tuple[]\"}],\"internalType\":\"struct Client.Any2EVMMessage\",\"name\":\"message\",\"type\":\"tuple\"}],\"name\":\"ccipReceive\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"\",\"type\":\"uint64\"}],\"name\":\"chainIdToChainSelector\",\"outputs\":[{\"internalType\":\"uint64\",\"name\":\"\",\"type\":\"uint64\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"\",\"type\":\"uint64\"}],\"name\":\"chains\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"destinationChain\",\"type\":\"uint64\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"crossChainTransfer\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"messageId\",\"type\":\"bytes32\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"destinationChain\",\"type\":\"uint64\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"address\",\"name\":\"msgSender\",\"type\":\"address\"}],\"name\":\"getLinkFees\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"fees\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getRouter\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"i_link\",\"outputs\":[{\"internalType\":\"contract LinkTokenInterface\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"forwarder\",\"type\":\"address\"}],\"name\":\"isTrustedForwarder\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mintMore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"chain\",\"type\":\"uint64\"},{\"internalType\":\"address\",\"name\":\"addr\",\"type\":\"address\"}],\"name\":\"setChain\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"chain\",\"type\":\"uint64\"},{\"internalType\":\"uint64\",\"name\":\"selector\",\"type\":\"uint64\"}],\"name\":\"setChainSelector\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"utilCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";


    int[] chainsLxLy = { 5, 1442 };
    bool isChainLxLy = false;

    string axelarValueApiURL = "https://build-it-cross-chain.vercel.app/axelar";
    string ensLookupValueApiURL = "https://build-it-cross-chain.vercel.app/ens-lookup";

    public bool isUSDCompatible = false;
    public bool isAuctionCompatible = false;

    public static ContractManager Instance { get; private set; }
    public MapManager mapManager;
    public PlacementManager placementManager;
    public UIController uiController;
    public GameManager gameManager;
    public CanvasManager canvasManager;
    public StructureManager structureManager;
    public RoadManager roadManager;
    public TextMeshProUGUI loadingText;
    public Button utilsFaucetButton, landFaucetButton, marketplaceButton;
    Contract mapContract, utilsContract, faucetContract, marketplaceContract, linkTokenContract;
    Task initializeMapTask;
    string walletAddress;
    int roadBalance = 0;
    int houseBalance = 0;
    int specialBalance = 0;
    int roadEditedBalance = 0;
    int houseEditedBalance = 0;
    int specialEditedBalance = 0;
    int mapBalance = 0;
    int size = 0;
    public int perSize = 0;
    int landCount = 0;
    int[] landOwnedIds = null;
    Land[] landOwnedIndexes = null;
    bool[,] landOwned = null;
    int[,] map = null;
    public int[,] editedMap = null;
    bool isError = false;
    string errorText = "ERROR!!!";
    float timePassed = 0;
    float errorDuration = 5f;
    Listings[,] listings = null;
    // TMP_Dropdown.OptionData typeDropDownoptionAuction;

    public struct Index
    {
        public int xIndex;
        public int yIndex;
    }

    private void Awake()
    {
        // Single persistent instance at all times.

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Two ContractManager instances were found, removing this one.");
            Destroy(this.gameObject);
            return;
        }
        // typeDropDownoptionAuction = canvasManager.typeDropdown.options[1];
        // Debug.Log("option: " + typeDropDownoptionAuction.text);
    }

    // Start is called before the first frame update
    void Start()
    {
        // call ResetData() rather than writing everthing here as ResetData() is called OnNetworkSwitch too
        utilsFaucetButton.gameObject.SetActive(false);
        landFaucetButton.gameObject.SetActive(false);
        marketplaceButton.gameObject.SetActive(false);
        loadingText.enabled = false;
        // mapContract = ThirdwebManager.Instance.SDK.GetContract(mapContractAddress, mapContractABI);
        // OnWalletConnect();
        // Test();
    }

    async void Test()
    {
        /*string amountNativeToken = await GetCCIPGasValue(80001, 11155111);
        Debug.Log("amountNativeToken: " + amountNativeToken);*/
    }
    private async Task Initialize()
    {
        var chainId = await ThirdwebManager.Instance.SDK.wallet.GetChainId();
        // canvasManager.HideTransferUtilsButton();
        if (chainsLxLy.Contains(chainId))
        {
            utilsContractABI = utilsLxLyContractABI;
            isChainLxLy = true;
        }
        else
        {
            utilsContractABI = utilsCCIPContractABI;
            isChainLxLy = false;
        }
        if (chainId == 80001)
        {
            mapContractAddress = mapContractAddressMumbai;
            utilsContractAddress = utilsContractAddressMumbai;
            faucetContractAddress = faucetContractAddressMumbai;
            marketplaceContractAddress = marketplaceContractAddressMumbai;
            canvasManager.ShowTransferUtilsButton();
        }
        /*else if (chainId == 365)
        {
            mapContractAddress = mapContractAddressTheta;
            utilsContractAddress = utilsContractAddressTheta;
            faucetContractAddress = faucetContractAddressTheta;
        }*/
        else if (chainId == 1442)
        {
            mapContractAddress = mapContractAddressPolygonZKEVMTestnet;
            utilsContractAddress = utilsContractAddressPolygonZKEVMTestnet;
            faucetContractAddress = faucetContractAddressPolygonZKEVMTestnet;
            marketplaceContractAddress = marketplaceContractAddressPolygonZKEVMTestnet;
            canvasManager.ShowTransferUtilsButton();
        }
        /*else if (chainId == 65)
        {
            mapContractAddress = mapContractAddressOkexTestnet;
            utilsContractAddress = utilsContractAddressOkexTestnet;
            faucetContractAddress = faucetContractAddressOkexTestnet;
        }
        else if (chainId == 51)
        {
            mapContractAddress = mapContractAddressXDCApothem;
            utilsContractAddress = utilsContractAddressXDCApothem;
            faucetContractAddress = faucetContractAddressXDCApothem;
        }*/
        else if (chainId == 11155111)
        {
            mapContractAddress = mapContractAddressSepolia;
            utilsContractAddress = utilsContractAddressSepolia;
            faucetContractAddress = faucetContractAddressSepolia;
            marketplaceContractAddress = marketplaceContractAddressSepolia;
            canvasManager.ShowTransferUtilsButton();
        }
        /*else if (chainId == 5001)
        {
            mapContractAddress = mapContractAddressMantleTestnet;
            utilsContractAddress = utilsContractAddressMantleTestnet;
            faucetContractAddress = faucetContractAddressMantleTestnet;
            marketplaceContractAddress = marketplaceContractAddressMantleTestnet;
        }
        else if (chainId == 4002)
        {
            mapContractAddress = mapContractAddressFantomTestnet;
            utilsContractAddress = utilsContractAddressFantomTestnet;
            faucetContractAddress = faucetContractAddressFantomTestnet;
            marketplaceContractAddress = marketplaceContractAddressFantomTestnet;
            canvasManager.ShowTransferUtilsButton();
        }
        else if (chainId == 250)
        {
            mapContractAddress = mapContractAddressFantom;
            utilsContractAddress = utilsContractAddressFantom;
            faucetContractAddress = faucetContractAddressFantom;
            marketplaceContractAddress = marketplaceContractAddressFantom;
        }
        else if(chainId == 421613)
        {
            mapContractAddress = mapContractAddressArbitrumGoerli;
            utilsContractAddress = utilsContractAddressArbitrumGoerli;
            faucetContractAddress = faucetContractAddressArbitrumGoerli;
            marketplaceContractAddress = marketplaceContractAddressArbitrumGoerli;
            canvasManager.ShowTransferUtilsButton();
        }
        else if (chainId == 97)
        {
            mapContractAddress = mapContractAddressBSCTestnet;
            utilsContractAddress = utilsContractAddressBSCTestnet;
            faucetContractAddress = faucetContractAddressBSCTestnet;
            marketplaceContractAddress = marketplaceContractAddressBSCTestnet;
            canvasManager.ShowTransferUtilsButton();
        }
        else if (chainId == 5611)
        {
            mapContractAddress = mapContractAddressOPBNBTestnet;
            utilsContractAddress = utilsContractAddressOPBNBTestnet;
            faucetContractAddress = faucetContractAddressOPBNBTestnet;
            marketplaceContractAddress = marketplaceContractAddressOPBNBTestnet;
        }*/
        else if (chainId == 5)
        {
            mapContractAddress = mapContractAddressGoerli;
            utilsContractAddress = utilsContractAddressGoerli;
            faucetContractAddress = faucetContractAddressGoerli;
            marketplaceContractAddress = marketplaceContractAddressGoerli;
            canvasManager.ShowTransferUtilsButton();
        }
        else if (chainId == 43113)
        {
            mapContractAddress = mapContractAddressAvalancheFujiTestnet;
            utilsContractAddress = utilsContractAddressAvalancheFujiTestnet;
            faucetContractAddress = faucetContractAddressAvalancheFujiTestnet;
            marketplaceContractAddress = marketplaceContractAddressAvalancheFujiTestnet;
            canvasManager.ShowTransferUtilsButton();
        }

        marketplaceButton.gameObject.SetActive(true);

        mapContract = ThirdwebManager.Instance.SDK.GetContract(mapContractAddress, mapContractABI);
        utilsContract = ThirdwebManager.Instance.SDK.GetContract(utilsContractAddress, utilsContractABI);
        faucetContract = ThirdwebManager.Instance.SDK.GetContract(faucetContractAddress, faucetContractABI);
        marketplaceContract = ThirdwebManager.Instance.SDK.GetContract(marketplaceContractAddress, marketplaceContractABI);

        linkTokenContractAddress = await marketplaceContract.Read<string>("i_link");
        if (linkTokenContractAddress == "0x0000000000000000000000000000000000000000")
        {
            isAuctionCompatible = false;
            if (canvasManager.typeDropdown.options.Count == 2)
            {
                // canvasManager.typeDropdown.options.Remove(typeDropDownoptionAuction);
                canvasManager.typeDropdown.options.Remove(canvasManager.typeDropdown.options[1]);
            }
        }
        else
        {
            linkTokenContract = ThirdwebManager.Instance.SDK.GetContract(linkTokenContractAddress, linkTokenContractABI);
            isAuctionCompatible = true;
            if (canvasManager.typeDropdown.options.Count == 1)
            {
                // canvasManager.typeDropdown.options.Add(typeDropDownoptionAuction);
                canvasManager.typeDropdown.options.Add(new TMP_Dropdown.OptionData { text = "Auction" });
            }
        }

        string usdPriceFeedAddress = await marketplaceContract.Read<string>("eth_usd_priceFeed");
        if (usdPriceFeedAddress == "0x0000000000000000000000000000000000000000")
        {
            isUSDCompatible = false;
            canvasManager.usdToggle.gameObject.SetActive(false);
        }
        else
        {
            linkTokenContract = ThirdwebManager.Instance.SDK.GetContract(linkTokenContractAddress, linkTokenContractABI);
            isAuctionCompatible = true;
            canvasManager.usdToggle.gameObject.SetActive(true);
        }
        initializeMapTask = InitializeMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (isError)
        {
            loadingText.enabled = true;
            loadingText.text = errorText;
            timePassed += Time.deltaTime;
            if (timePassed >= errorDuration)
            {
                isError = false;
                timePassed = 0;
                errorText = "ERROR!!!";
                loadingText.text = "LOADING: 0%";
                loadingText.enabled = false;
            }
        }
    }

    private async Task InitializeMap()
    {
        mapManager.destroyHighlights();
        size = await getSize();
        landOwned = new bool[size, size];
        map = new int[size, size];
        editedMap = new int[size, size];
        placementManager.InitializeMap(size);
        mapManager.updateGridSize(size);
        perSize = await getPerSize();
        landCount = await getLandCount();
    }

    private async Task<int> getLandCount()
    {

        var landCount = await mapContract.Read<int>("landCount");
        Debug.Log("landCount: " + landCount);
        return landCount;
    }

    private async Task<int> getPerSize()
    {

        var perSize = await mapContract.Read<int>("perSize");
        Debug.Log("perSize: " + perSize);
        return perSize;
    }

    private async Task<int> getSize()
    {

        var size = await mapContract.Read<int>("size");
        Debug.Log("Size: " + size);
        return size;
    }

    public async Task setItemBalances()
    {

        Debug.Log("ROAD BALANCE QUERYING...");
        string roadBalanceString = await utilsContract.ERC1155.BalanceOf(walletAddress, ROAD.ToString());
        Debug.Log("ROAD BALANCE STRING " + roadBalanceString);
        roadBalance = int.Parse(roadBalanceString);
        Debug.Log("roadBalance: " + roadBalance);
        houseBalance = await utilsContract.Read<int>("balanceOf", walletAddress, HOUSE);
        specialBalance = await utilsContract.Read<int>("balanceOf", walletAddress, SPECIAL);
        Debug.Log("item balances: " + roadBalance + " " + houseBalance + " " + specialBalance);
        if (roadBalance == 0 || houseBalance == 0 || specialBalance == 0)
        {
            Debug.Log("INSUFFICIENT UTILS BALANCE");
            utilsFaucetButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("SUFFICIENT UTILS BALANCE");
            utilsFaucetButton.gameObject.SetActive(false);
        }
        roadEditedBalance = roadBalance;
        houseEditedBalance = houseBalance;
        specialEditedBalance = specialBalance;
        Debug.Log("item edited balances: " + roadEditedBalance + " " + houseEditedBalance + " " + specialEditedBalance);
        uiController.updateRoadBalance(roadEditedBalance);
        uiController.updateHouseBalance(houseEditedBalance);
        uiController.updateSpecialBalance(specialEditedBalance);
    }
    private async Task<int> getMapBalance()
    {
        Debug.Log("TEST101010!!! " + walletAddress);
        int balance = await mapContract.Read<int>("balanceOf", walletAddress);
        Debug.Log("TEST111111!!!");
        Debug.Log("map balance of user: " + balance);
        if (balance == 0)
        {
            Debug.Log("INSUFFICIENT MAP BALANCE");
            landFaucetButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("SUFFICIENT MAP BALANCE");
            landFaucetButton.gameObject.SetActive(false);
        }
        return balance;
    }

    private async Task<int[]> getLandOwnedIDs()
    {
        int[] ids = new int[mapBalance];
        int index = 0;
        for (int i = 1; i <= landCount; i++)
        {
            string owner = await mapContract.Read<string>("ownerOf", i);
            Debug.Log("owner of land " + i + ": " + owner);
            if (owner.ToLower() == walletAddress.ToLower())
            {
                ids[index++] = i;
            }
        }
        Debug.Log("ids: " + string.Join(",", ids));
        return ids;
    }

    private async Task<Land[]> getLandOwnedIndexes()
    {
        Land[] indexes = new Land[mapBalance];
        int i = 0;
        Debug.Log("landOwnedIds.Length: " + landOwnedIds.Length);
        foreach (int id in landOwnedIds)
        {
            Debug.Log("id: " + id);
            List<object> result = await mapContract.Read<List<object>>("land", id);
            Debug.Log("xIndex: " + result[0].ToString());
            Debug.Log("yIndex: " + result[1].ToString());
            Land myLandResult = new Land();
            myLandResult.xIndex = BigInteger.Parse(result[0].ToString());
            myLandResult.yIndex = BigInteger.Parse(result[1].ToString());
            indexes[i++] = myLandResult;
            i++;
        }
        Debug.Log("total i " + i);

        var s = "{";
        for (int x = 0; x < indexes.Length; x++)
        {
            s += "(" + indexes[x].xIndex + "," + indexes[x].yIndex + "),";
        }
        s += '}';
        Debug.Log("indexes: " + s);
        return indexes;
    }

    private void updateLandOwned(int size, int perSize, Land[] landOwnedIndexes)
    {
        landOwned = new bool[size, size];
        foreach (var land in landOwnedIndexes)
        {
            int x1 = (int)land.xIndex * perSize;
            int y1 = (int)land.yIndex * perSize;
            int x2 = x1 + perSize - 1;
            int y2 = y1 + perSize - 1;
            int tmpY1 = y1;
            while (x1 <= x2)
            {
                while (y1 <= y2)
                {
                    landOwned[x1, y1] = true;
                    y1++;
                }
                y1 = tmpY1;
                x1++;
            }
        }
    }

    private async Task updateMap(int size)
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        map = new int[size, size];
        for (int x = 0; x < size; x++)
        {
            /*if(x % 2 == 0 || x % 3 == 0)
            {
                continue;
            }*/
            Task[] tasks = new Task[size];
            int index = 0;
            for (int y = 0; y < size; y++)
            {
                loadingText.text = "Loading: " + ((int)((x * size + y) * 100f / (size * size))) + "%";
                tasks[index++] = updateMapIndex(x, y);

            }
            // await Task.WhenAll(tasks);
            // now you are at the UI thread
            foreach (var t in tasks)
            {
                try
                {
                    await t;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        loadingText.text = "Loading: 100%";
        loadingText.enabled = false;
        // await Task.WhenAll(tasks.Where(t => t != null).ToArray()); // if tasks returns null
        // await Task.WhenAll(tasks);
    }

    private async Task<int> updateMapIndex(int x, int y)
    {
        try
        {
            Debug.Log("map getting for: " + x + "," + y);
            map[x, y] = await mapContract.Read<int>("map", x, y);
            Debug.Log("Got: " + x + "," + y + "," + map[x, y]);
            return map[x, y];
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        return 0;
    }

    private void initializeMapItems()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Debug.Log("add: " + i + "," + j + "," + map[i, j]);
                int item = map[i, j];
                Vector3Int position = new Vector3Int(i, 0, j);
                placeItem(position, item, false);
                /*if (item == EMPTY)
                {
                    structureManager.DeleteItem(position, false);
                    roadManager.FixRoadPrefabsAfterDelete(position, false);
                }
                else if (item == ROAD)
                {
                    roadManager.PlaceRoad(position, false);
                    roadManager.FinishPlacingRoad();
                }
                else if (item == HOUSE)
                {
                    structureManager.PlaceHouse(position, false);
                }
                else if (item == SPECIAL)
                {
                    structureManager.PlaceSpecial(position, false);
                }
                else
                {
                    // SHOULDN'T COME HERE AS I'VE NOT MINTED MORE ITEMS IN UTILS CONTRACT
                    Debug.Log("SHOULDN'T COME HERE AS I'VE NOT MINTED MORE ITEMS IN UTILS CONTRACT: " + i + "," + j + "," + map[i, j]);
                }*/
            }
        }
    }

    public void removeItem(Vector3Int position, bool fromUser = true)
    {
        structureManager.DeleteItem(position, fromUser);
        roadManager.FixRoadPrefabsAfterDelete(position, fromUser);
    }

    public void placeRoad(Vector3Int position, bool fromUser = true)
    {
        roadManager.PlaceRoad(position, fromUser);
        roadManager.FinishPlacingRoad();
    }

    public void placeHouse(Vector3Int position, bool fromUser = true)
    {
        structureManager.PlaceHouse(position, fromUser);
    }

    public void placeSpecial(Vector3Int position, bool fromUser = true)
    {
        structureManager.PlaceSpecial(position, fromUser);
    }

    public void placeItem(Vector3Int position, int item, bool fromUser = true)
    {
        if (item == EMPTY)
        {
            removeItem(position, fromUser);
        }
        else if (item == ROAD)
        {
            placeRoad(position, fromUser);
        }
        else if (item == HOUSE)
        {
            placeHouse(position, fromUser);
        }
        else if (item == SPECIAL)
        {
            placeSpecial(position, fromUser);
        }
    }

    private async Task setUserData(bool queryMap = true)
    {
        mapManager.destroyHighlights();
        Debug.Log("TEST666!!!");
        walletAddress = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        //
        // walletAddress = "0x4CA5FE129837E965e49b507cfE36c0dc574e8864";
        //
        Debug.Log("TEST777!!!");
        await setItemBalances();
        Debug.Log("TEST888!!!");
        mapBalance = await getMapBalance();
        Debug.Log("TEST999!!!");
        Debug.Log("TEST121212!!!");
        uiController.updateMapBalance(mapBalance);
        Debug.Log("TEST131313!!!");
        landOwnedIds = await getLandOwnedIDs();
        Debug.Log("TEST141414!!!");
        Debug.Log("landOwnedIndexes");
        landOwnedIndexes = await getLandOwnedIndexes();
        await initializeMapTask; // wait for map to be initialized if not already
        //
        /*landOwnedIndexes = new Land[2];
        landOwnedIndexes[0] = new Land { xIndex = 0, yIndex = 0 };
        landOwnedIndexes[0] = new Land { xIndex = 2, yIndex = 1 };*/
        /*landOwnedIndexes = new Land[2];
        landOwnedIndexes[0] = new Land { xIndex = 0, yIndex = 1 };
        landOwnedIndexes[1] = new Land { xIndex = 2, yIndex = 2 };*/
        //
        mapManager.highlightOwnedLands(landOwnedIndexes, perSize);
        updateLandOwned(size, perSize, landOwnedIndexes);
        if (queryMap)
        {
            await updateMap(size);
            //
            /*map[2, 7] = 3;
            map[3, 8] = 2;
            map[4, 9] = 1;
            map[0, 0] = 1;
            map[1, 0] = 1;
            map[2, 0] = 1;
            map[3, 0] = 1;
            map[4, 0] = 2;
            map[5, 0] = 2;
            map[6, 0] = 2;
            map[3, 3] = 3;
            map[4, 4] = 3;
            map[5, 5] = 3;
            map[1, 5] = 3;
            map[2, 5] = 2;
            map[3, 7] = 1;*/
            //
            initializeMapItems();
            // DebugMap();
        }
    }

    private void DebugMap()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Debug.Log("map(" + i + "," + j + ") = " + map[i, j] + " | " + "editedMap(" + i + "," + j + ") = " + editedMap[i, j]);
            }
        }
    }

    public void updateEditedMap(Vector3Int position, CellType cell)
    {
        // Debug.Log("update: " + position.x + "," + position.z + "," + cell);
        if (cell == CellType.Empty)
        {
            editedMap[position.x, position.z] = EMPTY;
        }
        else if (cell == CellType.Road)
        {
            editedMap[position.x, position.z] = ROAD;
        }
        else if (cell == CellType.Structure)
        {
            editedMap[position.x, position.z] = HOUSE;
        }
        else if (cell == CellType.SpecialStructure)
        {
            editedMap[position.x, position.z] = SPECIAL;
        }
        else
        {
            // shouldn't come here
        }
    }

    public bool AddRoad()
    {
        if (roadEditedBalance == 0) return false;
        roadEditedBalance--;
        uiController.updateRoadBalance(roadEditedBalance);
        return true;
    }

    public bool AddHouse()
    {
        if (houseEditedBalance == 0) return false;
        houseEditedBalance--;
        uiController.updateHouseBalance(houseEditedBalance);
        return true;
    }

    public bool AddSpecial()
    {
        if (specialEditedBalance == 0) return false;
        specialEditedBalance--;
        uiController.updateSpecialBalance(specialEditedBalance);
        return true;
    }

    public void DeleteItem(Vector3Int position)
    {
        int x = position.x;
        int y = position.z;
        if (editedMap[x, y] == ROAD)
        {
            roadEditedBalance++;
            uiController.updateRoadBalance(roadEditedBalance);
        }
        else if (editedMap[x, y] == HOUSE)
        {
            houseEditedBalance++;
            uiController.updateHouseBalance(houseEditedBalance);
        }
        else if (editedMap[x, y] == SPECIAL)
        {
            specialEditedBalance++;
            uiController.updateSpecialBalance(specialEditedBalance);
        }
        else
        {
            Debug.Log("NO MATCH: " + editedMap[x, y] + " , " + x + " , " + y);
        }
    }

    public void CancelClicked()
    {
        gameManager.ClearInputActions();
        roadEditedBalance = roadBalance;
        houseEditedBalance = houseBalance;
        specialEditedBalance = specialBalance;
        uiController.updateRoadBalance(roadEditedBalance);
        uiController.updateHouseBalance(houseEditedBalance);
        uiController.updateSpecialBalance(specialEditedBalance);
        editedMap = new int[size, size];
        copyMapToEditedMap();
        initializeMapItems();
        // DebugMap();
    }

    private void copyMapToEditedMap()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                editedMap[i, j] = map[i, j];
            }
        }
    }

    public async Task confirmMapUpdates()
    {
        int[] x;
        int[] y;
        int[] utilId;
        int len = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i, j] != editedMap[i, j])
                {
                    len++;
                }
            }
        }
        if (len == 0)
        {
            return;
        }
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        canvasManager.RemoveClickListeners();
        x = new int[len];
        y = new int[len];
        utilId = new int[len];
        int index = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i, j] != editedMap[i, j])
                {
                    x[index] = i;
                    y[index] = j;
                    utilId[index] = editedMap[i, j];
                    Debug.Log("diff: " + i + "," + j + "," + map[i, j] + "," + editedMap[i, j]);
                    index++;
                }
            }
        }
        loadingText.text = "Loading: 10%";
        try
        {
            bool result = await utilsContract.Read<bool>("isApprovedForAll", walletAddress, mapContractAddress);
            if (!result)
            {
                loadingText.text = "Loading: 20%";
                await utilsContract.Write("setApprovalForAll", mapContractAddress, true);
            }
            loadingText.text = "Loading: 65%";
            await mapContract.Write("updateItems", x, y, utilId);
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
            await setUserData();
        }
        catch (Exception e)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogError(e);
        }
        canvasManager.AttachClickListeners();
    }

    // x,y are co-ordinates and not land Index
    public bool userOwnsIndex(int x, int y)
    {
        if (landOwned == null)
        {
            return false;
        }
        return landOwned[x, y];
    }
    public bool landListedIndex(int xIndex, int yIndex)
    {
        if (listings == null)
        {
            return false;
        }
        if (listings[xIndex, yIndex].listings == null)
        {
            return false;
        }

        bool listed = false;

        foreach (var listing in listings[xIndex, yIndex].listings)
        {
            if (listing.isValid)
            {
                listed = true;
                break;
            }
        }

        return listed;
    }
    private async Task setData()
    {
        await Initialize();
        Debug.Log("TEST444!!!");
        await setUserData();
        Debug.Log("TEST555!!!");
        canvasManager.AttachClickListeners();
    }

    private void ResetData()
    {
        gameManager.ClearInputActions();
        canvasManager.RemoveClickListeners();
        walletAddress = "";
        mapBalance = 0;
        map = new int[size, size];
        editedMap = new int[size, size];
        initializeMapItems();
        uiController.updateMapBalance(mapBalance);
        loadingText.enabled = false;
        utilsFaucetButton.gameObject.SetActive(false);
        landFaucetButton.gameObject.SetActive(false);
        marketplaceButton.gameObject.SetActive(false);
        canvasManager.HideTransferUtilsButton();
    }

    public async void GetUtilsFaucet()
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        try
        {
            await faucetContract.Write("getToken", utilsContractAddress, ROAD);
            loadingText.text = "Loading: 30%";
            await faucetContract.Write("getToken", utilsContractAddress, HOUSE);
            loadingText.text = "Loading:60%";
            await faucetContract.Write("getToken", utilsContractAddress, SPECIAL);
            loadingText.text = "Loading: 90%";
            await setItemBalances();
            loadingText.text = "Loading: 100%";
        }
        catch (Exception ex)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogError(ex);
            await setItemBalances();
        }
        loadingText.enabled = false;
    }

    public async void GetLandFaucet()
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        try
        {
            if (landCount >= ((size / perSize) * (size / perSize)))
            {
                errorText = "NO LAND LEFT";
                isError = true;
                return;
            }
            List<Land> landsUnavailable = new List<Land>();
            for (int i = 1; i <= landCount; i++)
            {
                List<object> result = await mapContract.Read<List<object>>("land", i);
                Debug.Log("xIndex: " + result[0].ToString());
                Debug.Log("yIndex: " + result[1].ToString());
                Land myLandResult = new Land();
                myLandResult.xIndex = BigInteger.Parse(result[0].ToString());
                myLandResult.yIndex = BigInteger.Parse(result[1].ToString());
                landsUnavailable.Add(myLandResult);
                loadingText.text = "Loading: " + (i * 50 / landCount) + "%";
            }
            loadingText.text = "Loading: 50%";
            int xAvailable = 0, yAvailable = 0;
            for (int x = 0; x < size / perSize; x++)
            {
                bool isAvailable = false;
                for (int y = 0; y < size / perSize; y++)
                {
                    Land currentLand = new Land();
                    currentLand.xIndex = x;
                    currentLand.yIndex = y;
                    if (!landsUnavailable.Contains(currentLand))
                    {
                        xAvailable = x;
                        yAvailable = y;
                        isAvailable = true;
                        break;
                    }
                }
                if (isAvailable)
                {
                    break;
                }
            }
            loadingText.text = "Loading: 60%";
            await mapContract.Write("mint", xAvailable, yAvailable);
            loadingText.text = "Loading: 90%";
            landCount = await getLandCount();
            loadingText.text = "Loading: 91%";
            await setUserData(false);
            loadingText.text = "Loading: 100%";
        }
        catch (Exception ex)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogError(ex);
        }
        loadingText.enabled = false;
    }

    public async Task CreateListing(bool inUSD, int xIndex, int yIndex, string price, bool isAuction, string auctionTime, string amount)
    {
        try
        {
            loadingText.enabled = true;
            loadingText.text = "Loading: 0%";

            Debug.Log("Details01: " + inUSD);
            Debug.Log("Details02: " + xIndex);
            Debug.Log("Details03: " + yIndex);
            Debug.Log("Details04: " + Web3.Convert.ToWei(price).ToString());
            Debug.Log("Details05: " + isAuction);
            Debug.Log("Details06: " + auctionTime);
            Debug.Log("Details07: " + Web3.Convert.ToWei(amount).ToString());
            int landId = await mapContract.Read<int>("landIds", xIndex, yIndex);
            loadingText.text = "Loading: 15%";
            Debug.Log("LandId: " + landId);

            bool isApproved = await mapContract.Read<bool>("isApprovedForAll", walletAddress, marketplaceContractAddress);
            loadingText.text = "Loading: 25%";
            if (!isApproved)
            {
                await mapContract.Write("setApprovalForAll", marketplaceContractAddress, true);
            }
            if (isAuction)
            {
                int allowance = await linkTokenContract.Read<int>("allowance", walletAddress, marketplaceContractAddress);
                loadingText.text = "Loading: 35%";
                if (allowance < Web3.Convert.ToWei(amount))
                {
                    await linkTokenContract.Write("approve", marketplaceContractAddress, Web3.Convert.ToWei(amount).ToString());
                }
            }
            loadingText.text = "Loading: 60%";
            // await mapContract.Write("approve", marketplaceContractAddress, landId);
            await marketplaceContract.Write("createListing", inUSD, landId, Web3.Convert.ToWei(price).ToString(), isAuction, auctionTime, Web3.Convert.ToWei(amount).ToString());
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
        }
        catch (Exception ex)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogError(ex);
        }
    }

    public async Task<bool> SetMarketplaceData()
    {
        /*Land[] listingLands = new Land[3];
        listings = new Listings[size / perSize, size / perSize];
        {
            Listings currentListing = listings[0, 0];
            if (currentListing.listings == null)
            {
                currentListing.listings = new List<Listing>();
            }
            Listing listingResult = new Listing();
            listingResult.id = 1;
            listingResult.price = "1000000000000000000";
            listingResult.inUSD = false;
            listingResult.isValid = true;
            listingResult.isAuction = false;
            listingResult.auctionTime = 0;
            listingResult.xIndex = 0;
            listingResult.yIndex = 0;
            listingResult.sellerAddress = "0x0";
            currentListing.listings.Add(listingResult);
            listings[0, 0] = currentListing;
        }

        {
            Listings currentListing = listings[1, 2];
            if (currentListing.listings == null)
            {
                currentListing.listings = new List<Listing>();
            }
            Listing listingResult = new Listing();
            listingResult.price = "5000000000000000000";
            listingResult.inUSD = true;
            listingResult.isValid = true;
            listingResult.isAuction = false;
            listingResult.auctionTime = 0;
            listingResult.xIndex = 1;
            listingResult.yIndex = 2;
            listingResult.sellerAddress = "0x0";
            currentListing.listings.Add(listingResult);
            listings[1, 2] = currentListing;
        }

        {
            Listings currentListing = listings[2, 0];
            if (currentListing.listings == null)
            {
                currentListing.listings = new List<Listing>();
            }
            Listing listingResult = new Listing();
            listingResult.price = "80000000000000000000";
            listingResult.inUSD = false;
            listingResult.isValid = true;
            listingResult.isAuction = true;
            listingResult.auctionTime = 60;
            listingResult.xIndex = 2;
            listingResult.yIndex = 0;
            listingResult.sellerAddress = "0x0";
            currentListing.listings.Add(listingResult);
            listings[2, 0] = currentListing;
        }

        listingLands[0] = new Land { xIndex = 0, yIndex = 0 };
        listingLands[1] = new Land { xIndex = 1, yIndex = 2 };
        listingLands[2] = new Land { xIndex = 2, yIndex = 0 };
        mapManager.highlightListings(listingLands, perSize);
        return true;*/
        try
        {
            loadingText.enabled = true;
            loadingText.text = "Loading: 0%";
            int listingCount = await marketplaceContract.Read<int>("listingCount");
            listings = new Listings[size / perSize, size / perSize];
            Debug.Log("ListingCount: " + listingCount);
            List<Land> listingLands = new List<Land>();
            for (int i = 1; i <= listingCount; i++)
            {
                Debug.Log("ListingID: " + i);
                List<object> result = await marketplaceContract.Read<List<object>>("listings", i);
                loadingText.text = "Loading: " + ((2 * i - 1) * 100) / (listingCount * 2) + "%";
                Debug.Log("seller: " + result[0].ToString());
                Debug.Log("inUSD: " + result[1].ToString());
                Debug.Log("tokenId: " + result[2].ToString());
                Debug.Log("price: " + result[3].ToString());
                Debug.Log("timestamp: " + result[4].ToString());
                Debug.Log("isValid: " + result[5].ToString());
                Debug.Log("isAuction: " + result[6].ToString());
                Debug.Log("aucionTime: " + result[7].ToString());
                Listing listingResult = new Listing();
                listingResult.id = i;

                listingResult.sellerAddress = result[0].ToString();
                listingResult.inUSD = result[1].ToString() == "True";

                listingResult.landId = Int32.Parse(result[2].ToString());
                List<object> indexes = await mapContract.Read<List<object>>("land", listingResult.landId);
                loadingText.text = "Loading: " + ((2 * i) * 100) / (listingCount * 2) + "%";
                Debug.Log("xIndex: " + indexes[0].ToString());
                Debug.Log("yIndex: " + indexes[1].ToString());
                listingResult.xIndex = Int32.Parse(indexes[0].ToString());
                listingResult.yIndex = Int32.Parse(indexes[1].ToString());

                // listingResult.price = await marketplaceContract.Read<string>("getPrice", i);
                listingResult.price = result[3].ToString();
                listingResult.timestamp = Int32.Parse(result[4].ToString());
                listingResult.isValid = result[5].ToString() == "True";
                listingResult.isAuction = result[6].ToString() == "True";
                listingResult.auctionTime = Int32.Parse(result[7].ToString());
                if (listingResult.isValid)
                {
                    Debug.Log("Add listing: " + listingResult.xIndex + " | " + listingResult.yIndex); ;
                    listingLands.Add(new Land { xIndex = listingResult.xIndex, yIndex = listingResult.yIndex });
                }

                Debug.Log("seller1: " + listingResult.sellerAddress);
                Debug.Log("inUSD1: " + listingResult.inUSD);
                Debug.Log("tokenId1: " + listingResult.landId);
                Debug.Log("price1: " + listingResult.price);
                Debug.Log("timestamp1: " + listingResult.timestamp);
                Debug.Log("isValid1: " + listingResult.isValid);
                Debug.Log("isAuction1: " + listingResult.isAuction);
                Debug.Log("aucionTime1: " + listingResult.auctionTime);
                Debug.Log("xIndex1: " + listingResult.xIndex);
                Debug.Log("yIndex1: " + listingResult.yIndex);

                Listings currentListing = listings[listingResult.xIndex, listingResult.yIndex];
                if (currentListing.listings == null)
                {
                    currentListing.listings = new List<Listing>();
                }
                currentListing.listings.Add(listingResult);
                listings[listingResult.xIndex, listingResult.yIndex] = currentListing;
            }
            Debug.Log("calling highlights: " + listingLands.Count);
            mapManager.highlightListings(listingLands.ToArray(), perSize);
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
            return true;
        }
        catch (Exception ex)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogException(ex);
            return false;
        }
    }

    public async Task<bool> BuyListing(int listingId, string price)
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        try
        {
            await marketplaceContract.Write("buyListing", new TransactionRequest() { value = price }, listingId);
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
            return true;
        }
        catch (Exception e)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogException(e);
            return false;
        }
    }

    public async Task<bool> BidListing(int listingId, string highestBid, string bid)
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        try
        {
            if (bid == "")
            {
                errorText = "Invalid Bid";
                isError = true;
                errorText = "Invalid Bid";
                Debug.Log("Invalid Bid");
                return false;
            }
            if (BigInteger.Parse(bid.ToWei()) <= BigInteger.Parse(highestBid))
            {
                errorText = "Low Bid";
                isError = true;
                errorText = "Low Bid";
                Debug.Log("Low Bid");
                return false;
            }
            await marketplaceContract.Write("bid", new TransactionRequest() { value = bid.ToWei() }, listingId);
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
            return true;
        }
        catch (Exception e)
        {
            loadingText.text = "ERROR!!!";
            isError = true;
            Debug.LogException(e);
            return false;
        }
    }

    public async Task<bool> TransferUtilsCrossChain(int sourceChain, int destChain, int tokenId, string amount)
    {
        loadingText.enabled = true;
        loadingText.text = "Loading: 0%";
        try
        {
            if (amount == "" || amount == "0" || BigInteger.Parse(amount) <= 0)
            {
                errorText = "Invalid Amount";
                isError = true;
                errorText = "Invalid Amount";
                Debug.Log("Invalid Amount");
                return false;
            }

            int tokenAmount = 0;
            if (tokenId == 0)
            {
                tokenAmount = roadBalance;
            }
            else if (tokenId == 1)
            {
                tokenAmount = houseBalance;
            }
            else if (tokenId == 2)
            {
                tokenAmount = specialBalance;
            }

            if (BigInteger.Parse(amount) > tokenAmount)
            {
                errorText = "Insufficient Amount";
                isError = true;
                errorText = "Insufficient Amount";
                Debug.Log("Insufficient Amount");
                return false;
            }
            TransactionResult res;
            // res = await utilsContract.Write("crossChainTransfer", new TransactionRequest() { value = amountNativeToken }, destChain, tokenId, amount);

            // Polygon ZKEVM LxLy Bridge uses chainId 1 for Polygon ZKEVM and 0 for Ethereum
            // Chainlink CCIP Bridge uses chainId 12532609583862916517 for Polygon Mumbai, 16015286601757825753 for Sepolia and 14767482510784806043 for Avalanche Fuji
            // Can't set chain selectors for CCIP as they are very large so have updated smart contract to accomadate this issue
            BigInteger destChainId = 0;
            if (destChain == 5)
            {
                destChainId = 0;
            }
            else if (destChain == 1442)
            {
                destChainId = 1;
            }
            else if (destChain == 80001)
            {
                destChainId = destChain;
            }
            else if (destChain == 11155111)
            {
                destChainId = destChain;
            }
            else if (destChain == 43113)
            {
                destChainId = destChain;
            }

            if (isChainLxLy)
            {
                loadingText.text = "Loading: 30%";
                res = await utilsContract.Write("crossChainTransfer", destChainId, tokenId, amount, true /* fast bridge */ );
            }
            else
            {
                string amountLinkToken = await utilsContract.Read<string>("getLinkFees", destChainId, tokenId, amount, walletAddress);
                loadingText.text = "Loading: 20%";
                // Approve amount
                int allowance = await linkTokenContract.Read<int>("allowance", walletAddress, utilsContractAddress);
                loadingText.text = "Loading: 25%";
                if (allowance < BigInteger.Parse(amountLinkToken))
                {
                    await linkTokenContract.Write("approve", marketplaceContractAddress, (amountLinkToken).ToString());
                }
                loadingText.text = "Loading: 30%";
                res = await utilsContract.Write("crossChainTransfer", destChainId, tokenId, amount);
            }
            Debug.Log("Transaction Hash: " + res.receipt.transactionHash);
            loadingText.text = "Loading: 100%";
            loadingText.enabled = false;
            return true;
        }
        catch (Exception e)
        {
            errorText = "ERROR!!!";
            isError = true;
            Debug.LogException(e);
            return false;
        }
    }

    public async Task<string> GetPrice(int listingId)
    {
        string price = await marketplaceContract.Read<string>("getPrice", listingId);
        return price;
    }

    public async Task<string> GetHighestBid(int listingId)
    {
        List<object> highestBid = await mapContract.Read<List<object>>("highestBid", listingId);
        return highestBid[1].ToString();
    }

    public List<Listing> GetListingsIndex(int xIndex, int yIndex)
    {
        if (listings == null)
        {
            return null;
        }
        return listings[xIndex, yIndex].listings;
    }

    public void ResetListingHighlights()
    {
        mapManager.destroyListingHighlights();
    }

    public async void OnWalletConnect()
    {

        Debug.Log("TEST111!!!");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    SignIn ();
#endif
        Debug.Log("TEST222!!!");
        uiController.connectWalletText.enabled = false;
        await setData();
        Debug.Log("TEST333!!!");
    }

    public void OnWalletDisconnect()
    {
        uiController.connectWalletText.enabled = true;
        ResetData();
    }

    public async void OnSwitchNetwork()
    {
        ResetData();
        Start();
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    SignIn ();
#endif
        await setData();
    }
}
