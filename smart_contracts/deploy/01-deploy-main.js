const { network } = require("hardhat");
const hre = require("hardhat");

module.exports = async ({ getNamedAccounts, deployments }) => {
  const { deploy, log } = deployments;
  const { deployer } = await getNamedAccounts();
  const chainId = network.config.chainId;
  const accounts = await hre.ethers.getSigners();
  const account = accounts[0];
  const waitBlockConfirmations = 6;
  const utilsBaseUri = "https://www.example.com/utils/";
  const mapBaseUri = "https://www.example.com/map/";
  const size = 15;
  const perSize = 5;

  const utilsMintCount = 3;
  const utilsMintAmount = 1000;
  const transferUtilsAmount = 500;

  // marketplace can only be deployed on goerli or sepolia testnet. As price feed is not available on other testnets
  let registryAddress = "0x0000000000000000000000000000000000000000";
  let registrarAddress = "0x0000000000000000000000000000000000000000";
  let eth_usd_priceFeedAddress = "0x0000000000000000000000000000000000000000";
  let linkAddress = "0x0000000000000000000000000000000000000000";
  let gasLimit = 999999;
  let routerAddress = "0x0000000000000000000000000000000000000000";
  let polygonZkevmBridgeAddress = "0x0000000000000000000000000000000000000000";
  let isPolygonZkvemBridgeRequired = false;
  if (chainId == 80001) {
    // mumbai
    routerAddress = "0x70499c328e1e2a3c41108bd3730f6670a44595d1";
    registryAddress = "0xE16Df59B887e3Caa439E0b29B42bA2e7976FD8b2";
    registrarAddress = "0x57A4a13b35d25EE78e084168aBaC5ad360252467";
    linkAddress = "0x326C977E6efc84E512bB9C30f76E30c160eD06FB";
  } /* else if (chainId == 4002) {
    // fantom testnet
    registryAddress = "0xE16Df59B887e3Caa439E0b29B42bA2e7976FD8b2";
    registrarAddress = "0x57A4a13b35d25EE78e084168aBaC5ad360252467";
    linkAddress = "0xfaFedb041c0DD4fA2Dc0d87a6B0979Ee6FA7af5F";
  } */ else if (chainId == 11155111) {
    // sepolia
    routerAddress = "0xd0daae2231e9cb96b94c8512223533293c3693bf";
    registryAddress = "0xE16Df59B887e3Caa439E0b29B42bA2e7976FD8b2";
    registrarAddress = "0x9a811502d843E5a03913d5A2cfb646c11463467A";
    eth_usd_priceFeedAddress = "0x694AA1769357215DE4FAC081bf1f309aDC325306";
    linkAddress = "0x779877A7B0D9E8603169DdbD7836e478b4624789";
  } /* else if (chainId == 250) {
    // fantom
    registryAddress = "0x02777053d6764996e594c3E88AF1D58D5363a2e6";
    registrarAddress = "0xDb8e8e2ccb5C033938736aa89Fe4fa1eDfD15a1d";
    linkAddress = "0x6F43FF82CCA38001B6699a8AC47A2d0E66939407";
  } else if (chainId == 421613) {
    // arbitrum goerli
    registryAddress = "0x291093864bafc9aA517eF90ce954dD7D95D68C80";
    registrarAddress = "0x263ae9E522707D5A2B317026358d7f33ceC4ccc5";
    linkAddress = "0xd14838A68E8AFBAdE5efb411d5871ea0011AFd28";
  } */ else if (chainId == 97) {
    // bsc testnet
    routerAddress = "0x9527e2d01a3064ef6b50c1da1c0cc523803bcff2";
    registryAddress = "0xE16Df59B887e3Caa439E0b29B42bA2e7976FD8b2";
    registrarAddress = "0x57A4a13b35d25EE78e084168aBaC5ad360252467";
    linkAddress = "0x84b9B910527Ad5C03A9Ca831909E21e236EA7b06";
  } else if (chainId == 5) {
    // goerli testnet
    isPolygonZkvemBridgeRequired = true;
    registryAddress = "0xE16Df59B887e3Caa439E0b29B42bA2e7976FD8b2";
    registrarAddress = "0x57A4a13b35d25EE78e084168aBaC5ad360252467";
    linkAddress = "0x326C977E6efc84E512bB9C30f76E30c160eD06FB";
    polygonZkevmBridgeAddress = "0xF6BEEeBB578e214CA9E23B0e9683454Ff88Ed2A7";
  } else if (chainId == 1442) {
    // polygon zkevm testnet
    isPolygonZkvemBridgeRequired = true;
    polygonZkevmBridgeAddress = "0xF6BEEeBB578e214CA9E23B0e9683454Ff88Ed2A7";
  } else if (chainId == 43113) {
    // avalanche fuji testnet
    routerAddress = "0x554472a2720e5e7d5d3c817529aba05eed5f82d8";
    registryAddress = "0x819B58A646CDd8289275A87653a2aA4902b14fe6";
    registrarAddress = "0xD23D3D1b81711D75E1012211f1b65Cc7dBB474e2";
    linkAddress = "0x0b9d5D9136855f6FEc3c0993feE6E9CE8a297846";
  }

  console.log(
    "Is Polygon Zkevm Bridge Required : ",
    isPolygonZkvemBridgeRequired
  );

  log("----------------------------------------------------");
  const forwarderArg = [];
  const forwarder = await deploy("Forwarder", {
    from: deployer,
    args: forwarderArg,
    log: true,
    waitConfirmations: waitBlockConfirmations,
  });
  // const forwarder = {
  //   address: "0x65D84C0883e0e0c9c41B044b4523cd07999924Fe",
  // };
  console.log("forwarder deployed to:", forwarder.address);
  log("----------------------------------------------------");
  const utilsArg = isPolygonZkvemBridgeRequired
    ? [utilsBaseUri, forwarder.address, polygonZkevmBridgeAddress]
    : [utilsBaseUri, forwarder.address, linkAddress, routerAddress];
  const utils = await deploy(
    isPolygonZkvemBridgeRequired ? "UtilsLxLy.sol" : "UtilsCCIP.sol",
    {
      from: deployer,
      contract: isPolygonZkvemBridgeRequired
        ? "src/UtilsLxLy.sol:Utils"
        : "src/UtilsCCIP.sol:Utils",
      args: utilsArg,
      log: true,
      waitConfirmations: waitBlockConfirmations,
    }
  );
  // const utils = {
  //   address: "0x4a4e6cc94507b6ad2c91ad765d3f5b566b15d895",
  // };
  console.log("utils deployed to:", utils.address);
  log("----------------------------------------------------");
  const mapArg = [size, perSize, mapBaseUri, utils.address, forwarder.address];
  const map = await deploy("Map", {
    from: deployer,
    args: mapArg,
    log: true,
    waitConfirmations: waitBlockConfirmations,
  });
  // const map = {
  //   address: "0x91db12f3ea6f4598c982d46e8fdc72b53c333afb",
  // };
  console.log("map deployed to:", map.address);
  log("----------------------------------------------------");
  const faucetArg = [forwarder.address];
  const faucet = await deploy("Faucet", {
    from: deployer,
    args: faucetArg,
    log: true,
    waitConfirmations: waitBlockConfirmations,
  });
  // const faucet = {
  //   address: "0x724257edfe7f3bbf8c06a01ae3becb48dc5e220a",
  // };
  console.log("faucet deployed to:", faucet.address);
  log("----------------------------------------------------");
  const marketplaceArg = [
    eth_usd_priceFeedAddress,
    map.address,
    utils.address,
    linkAddress,
    registrarAddress,
    registryAddress,
    gasLimit,
    forwarder.address,
  ];
  const marketplace = await deploy("Marketplace", {
    from: deployer,
    args: marketplaceArg,
    log: true,
    waitConfirmations: waitBlockConfirmations,
  });
  // const marketplace = {
  //   address: "0x20294525826458177030954af848d783f733a80a",
  // };
  console.log("marketplace deployed to:", marketplace.address);
  log("----------------------------------------------------");
  console.log("Minting Utils...");
  await mintUtils(
    account,
    utils.address,
    utilsMintCount,
    utilsMintAmount,
    isPolygonZkvemBridgeRequired
      ? "src/UtilsLxLy.sol:Utils"
      : "src/UtilsCCIP.sol:Utils"
  );
  log("----------------------------------------------------");
  console.log("Transfering Utils to Faucet...");
  await transferToFaucet(
    account,
    utils.address,
    faucet.address,
    utilsMintCount,
    transferUtilsAmount,
    isPolygonZkvemBridgeRequired
      ? "src/UtilsLxLy.sol:Utils"
      : "src/UtilsCCIP.sol:Utils"
  );
  log("----------------------------------------------------");
  try {
    console.log("Verifying for Forwarder...");
    await verify(forwarder.address, forwarderArg);
    console.log("Verifying for Utils...");
    await verify(utils.address, utilsArg);
    console.log("Verifying for Map...");
    await verify(map.address, mapArg);
    console.log("Verifying for Faucet...");
    await verify(faucet.address, faucetArg);
    console.log("Verifying for Marketplace...");
    await verify(marketplace.address, marketplaceArg);
  } catch (error) {
    console.log(error);
  }
  log("----------------------------------------------------");
};

const verify = async (contractAddress, args) => {
  console.log("Verifying contract...");
  try {
    await run("verify:verify", {
      address: contractAddress,
      constructorArguments: args,
    });
    console.log("verified");
  } catch (e) {
    if (e.message.toLowerCase().includes("already verified")) {
      console.log("Already Verified!");
    } else {
      console.log(e);
    }
  }
};

const mintUtils = async (
  account,
  utilsContractAddress,
  count,
  amount,
  utilsName
) => {
  const utilsContract = await hre.ethers.getContractAt(
    utilsName,
    utilsContractAddress,
    account
  );
  for (let i = 1; i <= count; i++) {
    const tx = await utilsContract.mint(amount);
    console.log("Minted Utils " + i + " TX:", tx.hash);
    const receipt = await tx.wait();
    console.log("Minted Utils " + i + " RECEIPT:", receipt.transactionHash);

    // wait for 10 second as in fantom testnet, it throws error (even at 3 seconds):
    // Error: nonce has already been used [ See: https://links.ethers.org/v5-errors-NONCE_EXPIRED ]
    await new Promise((r) => setTimeout(r, 10000));
  }
};

const transferToFaucet = async (
  account,
  utilsContractAddress,
  faucetContractAddress,
  count,
  amount,
  utilsName
) => {
  const utilsContract = await hre.ethers.getContractAt(
    utilsName,
    utilsContractAddress,
    account
  );
  for (let i = 1; i <= count; i++) {
    const tx = await utilsContract.safeTransferFrom(
      account.address,
      faucetContractAddress,
      i,
      amount,
      "0x"
      // { gasPrice: 1600000008 }
    );
    console.log("Transfered Utils " + i + " TX:", tx.hash);
    const receipt = await tx.wait();
    console.log("Transfered Utils " + i + " RECEIPT:", receipt.transactionHash);

    // wait for 10 second as in fantom testnet, it throws error:
    // Error: nonce has already been used [ See: https://links.ethers.org/v5-errors-NONCE_EXPIRED ]
    await new Promise((r) => setTimeout(r, 10000));
  }
};

module.exports.tags = ["all", "main"];
