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
  const polygonZKEVMTestnetChainId = 1442;
  const seopliaChainId = 11155111;
  const goerliChainId = 5;
  const avalancheFujiChainId = 43113;

  // Polygon ZKEVM LxLy Bridge uses chainId 1 for Polygon ZKEVM and 0 for Ethereum
  // Chainlink CCIP Bridge uses chainId 12532609583862916517 for Polygon Mumbai, 16015286601757825753 for Sepolia and 14767482510784806043 for Avalanche Fuji
  const replaceChainId = (_chainId) =>
    _chainId == polygonZKEVMTestnetChainId
      ? "1"
      : _chainId == goerliChainId
      ? "0"
      : _chainId == mumbaiChainId
      ? "12532609583862916517"
      : _chainId == seopliaChainId
      ? "16015286601757825753"
      : "14767482510784806043";

  // Utils contract addresses
  const mumbaiAddress = "0x82BFe300311f0324423406382fC6bdccbC2BaB47";
  const polygonZKEVMTestnetAddress =
    "0x4f033bF08e610DDeBe5fA9707d5334Ad5c5A893e";
  const seopliaAddress = "0xE9cEAe69B724F4340Ca3D6D2F0D147b0Bc3E1978";
  const goerliAddress = "0x52Cb4B27503848ABd8dd3629474835299E1E99af";
  const avalancheFujiAddress = "0x11DA0f57086a19977E46B548b64166411d839a30";

  const mumbai = {
    chainId: mumbaiChainId,
    chainAddress: mumbaiAddress,
  };
  const polygonZKEVMTestnet = {
    chainId: polygonZKEVMTestnetChainId,
    chainAddress: polygonZKEVMTestnetAddress,
  };
  const seoplia = {
    chainId: seopliaChainId,
    chainAddress: seopliaAddress,
  };
  const goerli = {
    chainId: goerliChainId,
    chainAddress: goerliAddress,
  };
  const avalancheFuji = {
    chainId: avalancheFujiChainId,
    chainAddress: avalancheFujiAddress,
  };

  const isChainLxLy =
    chainId == polygonZKEVMTestnet.chainId || chainId == goerli.chainId;

  // Bridge is available between mumbai, seoplia and avalanche fuji AND between polygonZKEVMTestnet and goerli
  const addresses = isChainLxLy
    ? [
        {
          source: {
            ...(chainId == polygonZKEVMTestnet.chainId
              ? polygonZKEVMTestnet
              : goerli),
          },
          destination: {
            ...(chainId == polygonZKEVMTestnet.chainId
              ? goerli
              : polygonZKEVMTestnet),
          },
        },
      ]
    : [
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

  console.log("addresses: ", addresses);

  const utilsContractFactory = await hre.ethers.getContractFactory(
    isChainLxLy ? "src/UtilsLxLy.sol:Utils" : "src/UtilsCCIP.sol:Utils"
  );

  for (let i = 0; i < addresses.length; i++) {
    const utilsContract = await utilsContractFactory.attach(
      addresses[i].source.chainAddress
    );

    console.log("Utils contract created");
    console.log("Connecting user to Utils contract");
    const utils = await utilsContract.connect(deployer);
    console.log("User connected to Utils contract");

    console.log(
      "transaction arguments: ",
      replaceChainId(addresses[i].destination.chainId),
      addresses[i].destination.chainAddress
    );

    const tx = await utils.setChain(
      replaceChainId(addresses[i].destination.chainId),
      addresses[i].destination.chainAddress
    );

    console.log("----------------------------------");
    console.log(tx);
    const response = await tx.wait();
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
