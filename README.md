BuildIt is a metaverse project developed for the hackathon. It provides users with the ability to own virtual land within a map, place items on the land they own, and even sell the land to other users. The land is represented as ERC721 tokens, while the items are represented as ERC1155 tokens. All interactions within the metaverse are secured by smart contracts.

## Pre-requisites

You should have `Unity`, `npm`, `forge` and `foundry` installed.

## Installation

### Backend

Go to `backend` folder and install dependencies

```bash
cd backend/
npm install
```

To run the project:

```bash
npm run dev
```

Visit `localhost:3000` to run the backend api access point.

### Unity Project

Go to `client` folder.

```bash
cd client/
```

Open the `client` folder in Unity. Build the project and save build files in `frontend/public` directory.

### Frontend

Go to `frontend` folder and install dependencies

```bash
cd frontend/
npm install
```

To run the project:

```bash
npm run dev
```

Visit `localhost:3000` to play the game.

### Smart Contracts

Go to `smart_contracts` folder and install foundry and hardhat.

```bash
cd smart_contracts/
forge init
npm install
```

To compile smart contracts:

```bash
forge compile --skip test --skip script

# Or If you want to compile test files

forge compile --via-ir
```

To run test on smart contracts:

```bash
forge test --via-ir
```

To deploy smart contracts:

```bash
npx hardhat deploy --network $ChainName

# example: npx hardhat deploy --network mumbai
```

To add destination chain data for cross chain transfer of utils:

Update addresses to the latest deployed contract addresses in script/addChain.js and script/addChainSelector.js

( NOTE: No need to run script/addChainSelector.js incase of Polygon ZKEVM Testnet or Goerli )

```bash
npx hardhat run script/addChain.js --network $ChainName
npx hardhat run script/addChainSelector.js --network $ChainName

# example:
# npx hardhat run script/addChain.js --network mumbai
# npx hardhat run script/addChainSelector.js --network mumbai

```

Note: Don't forget to make .env file, refer .env.example file.

## Android APK

[Download apk file for Android](https://drive.google.com/drive/folders/1koUt3GFjGn5jITEy1MgrMTs4LI_h77DB?usp=sharing)

Website can also be used which is build for webgl and will works on both desktop and mobile.

## Smart Contracts ( Polygon ZKEVM Testnet )

| Contract                                                                                                 | Explorer Link                                                                                                                          |
| -------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| [Map.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Map.sol)                 | [0x9ea48FE0c9b0ae6e3E27Dec076800D9347c2E0D7](https://testnet-zkevm.polygonscan.com/address/0x9ea48FE0c9b0ae6e3E27Dec076800D9347c2E0D7) |
| [Utils.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Utils.sol)             | [0x4f033bF08e610DDeBe5fA9707d5334Ad5c5A893e](https://testnet-zkevm.polygonscan.com/address/0x4f033bF08e610DDeBe5fA9707d5334Ad5c5A893e) |
| [Faucet.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Faucet.sol)           | [0x4B03368f666fa7579BfeB49eF1c5E405389b174e](https://testnet-zkevm.polygonscan.com/address/0x4B03368f666fa7579BfeB49eF1c5E405389b174e) |
| [Marketplace.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Marketplace.sol) | [0x1b62E60b85678F5FF2fd6a8c27FB9dC7d5e1e2D4](https://testnet-zkevm.polygonscan.com/address/0x1b62E60b85678F5FF2fd6a8c27FB9dC7d5e1e2D4) |
| [Forwarder.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Forwarder.sol)     | [0x53D04942C2861B1FeB79300beAF26D1C10cC769f](https://testnet-zkevm.polygonscan.com/address/0x53D04942C2861B1FeB79300beAF26D1C10cC769f) |

## Smart Contracts ( Avalanche Fuji Testnet )

| Contract                                                                                                 | Explorer Link                                                                                                                              |
| -------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| [Map.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Map.sol)                 | [0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E](https://testnet.avascan.info/blockchain/c/address/0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E) |
| [Utils.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Utils.sol)             | [0x11DA0f57086a19977E46B548b64166411d839a30](https://testnet.avascan.info/blockchain/c/address/0x11DA0f57086a19977E46B548b64166411d839a30) |
| [Faucet.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Faucet.sol)           | [0x76cfdE04F691B93c9993Be24d5FE7667E7A8782C](https://testnet.avascan.info/blockchain/c/address/0x76cfdE04F691B93c9993Be24d5FE7667E7A8782C) |
| [Marketplace.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Marketplace.sol) | [0x489d47E592639Ba11107E84dd6CCA08F0892E27d](https://testnet.avascan.info/blockchain/c/address/0x489d47E592639Ba11107E84dd6CCA08F0892E27d) |
| [Forwarder.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Forwarder.sol)     | [0xCA34FF4068f042203087D475805c4DD8347cE958](https://testnet.avascan.info/blockchain/c/address/0xCA34FF4068f042203087D475805c4DD8347cE958) |

## Smart Contracts ( Polygon Mumbai )

| Contract                                                                                                 | Explorer Link                                                                                                                   |
| -------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| [Map.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Map.sol)                 | [0xEaC226cF2817F5565d6672A8E0C18410ec450991](https://mumbai.polygonscan.com/address/0xEaC226cF2817F5565d6672A8E0C18410ec450991) |
| [Utils.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Utils.sol)             | [0x07EE4358Dc25BAD471C56451cE4d11fCB6A7E3EF](https://mumbai.polygonscan.com/address/0x07EE4358Dc25BAD471C56451cE4d11fCB6A7E3EF) |
| [Faucet.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Faucet.sol)           | [0x2984288408926fe760673767804a8301d7DB6ae3](https://mumbai.polygonscan.com/address/0x2984288408926fe760673767804a8301d7DB6ae3) |
| [Marketplace.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Marketplace.sol) | [0x1674edA85F0cAd125B8f83383d339bea2E0b773D](https://mumbai.polygonscan.com/address/0x1674edA85F0cAd125B8f83383d339bea2E0b773D) |
| [Forwarder.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Forwarder.sol)     | [0xd19891D144a8EA7250Ece89560d1E408c11847CC](https://mumbai.polygonscan.com/address/0xd19891D144a8EA7250Ece89560d1E408c11847CC) |

## Smart Contracts ( Goerli )

| Contract                                                                                                 | Explorer Link                                                                                                                |
| -------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| [Map.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Map.sol)                 | [0x5720d7a63FeFf600fAA72A0C97361D490DEab903](https://goerli.etherscan.io/address/0x5720d7a63FeFf600fAA72A0C97361D490DEab903) |
| [Utils.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Utils.sol)             | [0x52Cb4B27503848ABd8dd3629474835299E1E99af](https://goerli.etherscan.io/address/0x52Cb4B27503848ABd8dd3629474835299E1E99af) |
| [Faucet.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Faucet.sol)           | [0x574F8B3f91d179F808bdDD879020164a1A317Daa](https://goerli.etherscan.io/address/0x574F8B3f91d179F808bdDD879020164a1A317Daa) |
| [Marketplace.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Marketplace.sol) | [0x14AADF21340053a7403B0D30625a0289E30bF8CF](https://goerli.etherscan.io/address/0x14AADF21340053a7403B0D30625a0289E30bF8CF) |
| [Forwarder.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Forwarder.sol)     | [0xbC9Da99F3c34E8e93134129A5466c3F5970cE210](https://goerli.etherscan.io/address/0xbC9Da99F3c34E8e93134129A5466c3F5970cE210) |

## Smart Contracts ( Sepolia )

| Contract                                                                                                 | Explorer Link                                                                                                                 |
| -------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| [Map.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Map.sol)                 | [0xdc915eD51B67EB524119b894e5D75CdA0F674a3E](https://sepolia.etherscan.io/address/0xdc915eD51B67EB524119b894e5D75CdA0F674a3E) |
| [Utils.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Utils.sol)             | [0x11e2e6353492e84605801849E74F3793c3A5251f](https://sepolia.etherscan.io/address/0x11e2e6353492e84605801849E74F3793c3A5251f) |
| [Faucet.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Faucet.sol)           | [0xA9b084B2Cc9918C5fF6c66138C1cC5D02954F442](https://sepolia.etherscan.io/address/0xA9b084B2Cc9918C5fF6c66138C1cC5D02954F442) |
| [Marketplace.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Marketplace.sol) | [0x2D1f1A043b9aBEf45F75Df85F797e1e51923f4A9](https://sepolia.etherscan.io/address/0x2D1f1A043b9aBEf45F75Df85F797e1e51923f4A9) |
| [Forwarder.sol](https://github.com/Ahmed-Aghadi/BuildIt/blob/main/smart_contracts/src/Forwarder.sol)     | [0xAdA21730026A539bDa27C2a2997E71023463C3D1](https://sepolia.etherscan.io/address/0xAdA21730026A539bDa27C2a2997E71023463C3D1) |

## Table of Contents

- [Inspiration](#inspiration)
- [What It Does](#what-it-does)
- [How We Built It](#how-we-built-it)
- [Challenges We Ran Into](#challenges-we-ran-into)
- [Accomplishments That We're Proud Of](#accomplishments-that-were-proud-of)
- [What We Learned](#what-we-learned)
- [What's Next for BuildIt](#whats-next-for-buildit)

## Inspiration

The inspiration behind BuildIt comes from the desire to create an immersive metaverse experience where users can explore, own, and customize virtual land. We wanted to empower users to express their creativity and engage with a virtual world where they have control over their own unique space.

## What It Does

BuildIt allows users to:

- Own virtual land within a map represented as ERC721 tokens.
- Place items on their owned land, such as buildings, roads, and other structures, represented as ERC1155 tokens.
- Sell their land to other users, transferring ownership and associated items.
- Connect their wallets (e.g., Metamask, Coinbase, WalletConnect) to interact with the metaverse.

When a user connects their wallet, the game fetches data from the smart contracts and highlights the portion of the map that the user owns. Users can then click the "Edit" button to place or remove items on their land. They have the option to cancel or confirm the changes, which updates the items in the appropriate locations. Smart contract checks ensure that users can only interact with the land they own.

In addition, BuildIt includes a marketplace where users can sell their land through direct listings or auctions. Chainlink automation can be utilized for auction listings, and if the chain supports Chainlink price feeds, the land can be sold in USD. The marketplace provides an easy and secure way for users to trade their land.

TheGraph is used to create and deploy a new subgraph for indexing data from smart contracts.

Also, users can transfer their util items from one chain to another ( using Polygon LxLy bridge and CCIP ). Polygon LxLy bridge is used to transfer utils items from Polygon ZKEVM Testnet to Goerli and vice versa whereas Chainlink CCIP is used to transfer utils items between Avalanche Fuji Testet, Polygon Mumbai and Sepolia.

While editing the map, user can also save/load private designs which is saved using sprucekit.

In marketplace listing, if seller owns an ENS account then it will display it so that it add more credibility about seller.

## How We Built It

BuildIt was built using the following technologies and tools:

- Unity: The game was developed using Unity and built for Webgl.
- Smart Contracts: Five smart contracts were developed using Foundry and Hardhat:
  - Map Contract: Responsible for the Lands in the Map, implemented as an ERC721 contract.
  - Utils Contract: Represents the items that can be placed on the land, implemented as an ERC1155 contract.
  - Faucet Contract: Allows users to obtain items for free initially. It is funded to provide items for judges and other participants.
  - Marketplace Contract: Facilitates land sales through direct listings and auctions.
  - Forwarder Contract: As all contracts implements ERC2771 context, Forwarder is used to provide gasless transactions for users.
- Map Size: The map size is determined in the smart contract, allowing the deployment of multiple maps with different sizes. The current deployment consists of a map with a size of 15 by 15 tiles, where each land is a 5 by 5 tile.
- Item Minting: Three items are minted in the Utils contract: road, house, and special item.
- Wallet Integration: Users can connect their wallets, such as Metamask, Coinbase, and WalletConnect, to interact with the metaverse.
- Gasless Transactions: All smart contracts implement ERC2771Context, enabling users to perform gasless transactions when the relayer is funded.
- Polygon LxLy bridge is used to transfer utils items from Polygon ZKEVM Testnet to Goerli and vice versa.
- Chainlink CCIP is used to transfer utils items between Avalanche Fuji Testet, Polygon Mumbai and Sepolia.
- Sprucekit was used to let User Save/Load private designs
- ENS was used to resolve custom name for users in marketplace
- TheGraph is used to create and deploy a new subgraph for indexing data from smart contracts.

## Challenges We Ran Into

During the development of BuildIt, we encountered several challenges, including:

- Integrating Unity with the blockchain and ensuring secure and efficient interactions between the game and smart contracts.
- Implementing ERC721 and ERC1155 token standards and handling the transfer of ownership between users and their land/items.
- Optimizing gas usage and transaction costs in smart contract deployments.
- Developing a user-friendly interface and seamless wallet integration for a smooth user experience.
- Sprucekit sdk was mainly for Reactjs project, so to pass message between game build for wasm to Reactjs was challenging.

## Accomplishments That We're Proud Of

Throughout the development process, we achieved several accomplishments that we're proud of, including:

- Successfully integrating the Unity game engine with the Blockchain and smart contracts.
- Creating a metaverse where users can own virtual land and customize it with various items.
- Implementing a marketplace where users can buy and sell land securely through direct listings and auctions.
- Enabling gasless transactions for users by implementing ERC2771Context in all smart contracts.
- Conducting comprehensive testing, including fuzz testing, to ensure the stability and reliability of the application.

## What We Learned

The development of BuildIt provided us with valuable learning experiences, including:

- Gaining in-depth knowledge of integrating smart contracts with Unity.
- Understanding the intricacies of token standards like ERC721 and ERC1155.
- Optimizing gas usage and transaction costs in smart contract deployments.
- Enhancing user experience through seamless wallet integration and fetching data from smart contracts.
- Polygon ZKEVM LxLy bridge and Chainlink services.

## What's Next for BuildIt

BuildIt is an ongoing project, and we have exciting plans testnet.bscscan.com

- Adding multiple maps with different sizes to expand the metaverse and accommodate more users.
- Conducting further research on gasless transactions to reduce transaction costs and improve user experience.
- Exploring cross-chain integrations to enable interoperability with other blockchain platforms.
- Enhancing the variety of items and customizations available to users.
- Engaging with the community to gather feedback and implement new features based on user suggestions.

We are dedicated to continuously improving and expanding BuildIt to create a vibrant and immersive metaverse experience for all users.
