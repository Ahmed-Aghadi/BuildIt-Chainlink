const { ethers, network } = require("hardhat");
const hre = require("hardhat");

// Run: npx hardhat run script/addChain.js --network sourceChain
// Example: npx hardhat run script/addChain.js --network arbitrumGoerli
// you can get sourceChain from hardhat.config.js
async function addChain() {
  accounts = await hre.ethers.getSigners();
  deployer = accounts[0];
  const chainId = network.config.chainId;
  console.log("Chain ID : " + chainId);
  console.log("Creating Utils contract");

  const mumbaiChainId = 80001;
  const seopliaChainId = 11155111;
  const avalancheFujiChainId = 43113;

  // Chainlink CCIP Bridge uses chainId 12532609583862916517 for Polygon Mumbai, 16015286601757825753 for Sepolia and 14767482510784806043 for Avalanche Fuji
  const replaceChainId = (_chainId) =>
    _chainId == mumbaiChainId
      ? "12532609583862916517"
      : _chainId == seopliaChainId
      ? "16015286601757825753"
      : "14767482510784806043";

  // Utils contract addresses
  const mumbaiAddress = "0x82BFe300311f0324423406382fC6bdccbC2BaB47";
  const seopliaAddress = "0xE9cEAe69B724F4340Ca3D6D2F0D147b0Bc3E1978";
  const avalancheFujiAddress = "0x11DA0f57086a19977E46B548b64166411d839a30";

  const mumbai = {
    chainId: mumbaiChainId,
    chainAddress: mumbaiAddress,
  };
  const seoplia = {
    chainId: seopliaChainId,
    chainAddress: seopliaAddress,
  };
  const avalancheFuji = {
    chainId: avalancheFujiChainId,
    chainAddress: avalancheFujiAddress,
  };

  // Bridge is available between mumbai, seoplia and avalanche fuji AND between polygonZKEVMTestnet and goerli
  const addresses = [
    {
      source: {
        ...(chainId == mumbai.chainId
          ? mumbai
          : chainId == seoplia.chainId
          ? seoplia
          : avalancheFuji),
      },
      destination: {
        ...(chainId == mumbai.chainId
          ? seoplia
          : chainId == seoplia.chainId
          ? mumbai
          : mumbai),
      },
    },
    {
      source: {
        ...(chainId == mumbai.chainId
          ? mumbai
          : chainId == seoplia.chainId
          ? seoplia
          : avalancheFuji),
      },
      destination: {
        ...(chainId == mumbai.chainId
          ? avalancheFuji
          : chainId == seoplia.chainId
          ? avalancheFuji
          : seoplia),
      },
    },
  ];

  const isChainCCIP =
    chainId == mumbai.chainId ||
    chainId == seoplia.chainId ||
    chainId == avalancheFuji.chainId;

  if (!isChainCCIP) {
    console.log("Chain is not CCIP");
    return;
  }

  console.log("addresses: ", addresses);

  const utilsContractFactory = await hre.ethers.getContractFactory(
    "src/UtilsCCIP.sol:Utils"
  );

  for (let i = 0; i < addresses.length; i++) {
    const utilsContract = await utilsContractFactory.attach(
      addresses[i].source.chainAddress
    );

    console.log("Utils contract created");
    console.log("Connecting user to Utils contract");
    const utils = await utilsContract.connect(deployer);
    console.log("User connected to Utils contract");

    console.log("Setting chain id to chain selector");

    console.log(
      "transaction arguments: ",
      addresses[i].destination.chainId,
      replaceChainId(addresses[i].destination.chainId)
    );

    let tx = await utils.setChainSelector(
      addresses[i].destination.chainId,
      replaceChainId(addresses[i].destination.chainId)
    );

    console.log("----------------------------------");
    console.log(tx);
    let response = await tx.wait();
    console.log("----------------------------------");
    console.log(response);

    console.log("Setting selector chain to chain selector");

    console.log(
      "transaction arguments: ",
      replaceChainId(addresses[i].destination.chainId),
      replaceChainId(addresses[i].destination.chainId)
    );

    tx = await utils.setChainSelector(
      replaceChainId(addresses[i].destination.chainId),
      replaceChainId(addresses[i].destination.chainId)
    );

    console.log("----------------------------------");
    console.log(tx);
    response = await tx.wait();
    console.log("----------------------------------");
    console.log(response);
  }
}

addChain()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
