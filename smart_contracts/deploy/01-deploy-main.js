const { network } = require("hardhat");
const hre = require("hardhat");

module.exports = async ({ getNamedAccounts, deployments }) => {
  const { deploy, log } = deployments;
  const { deployer } = await getNamedAccounts();
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

  // log("----------------------------------------------------");
  const utilsArg = [utilsBaseUri];
  // const utils = await deploy("Utils", {
  //   from: deployer,
  //   args: utilsArg,
  //   log: true,
  //   waitConfirmations: waitBlockConfirmations,
  // });
  // console.log("utils deployed to:", utils.address);
  // log("----------------------------------------------------");
  const mapArg = [
    size,
    perSize,
    mapBaseUri,
    "0x4b22e4f5cfCb3e648a6F42Fa9D4E55985f9647D1",
  ];
  // const map = await deploy("Map", {
  //   from: deployer,
  //   args: mapArg,
  //   log: true,
  //   waitConfirmations: waitBlockConfirmations,
  // });
  // console.log("map deployed to:", map.address);
  // log("----------------------------------------------------");
  const faucetArg = [];
  // const faucet = await deploy("Faucet", {
  //   from: deployer,
  //   args: faucetArg,
  //   log: true,
  //   waitConfirmations: waitBlockConfirmations,
  // });
  // console.log("faucet deployed to:", faucet.address);
  // log("----------------------------------------------------");
  // console.log("Minting Utils...");
  // await mintUtils(
  //   account,
  //   "0xCA34FF4068f042203087D475805c4DD8347cE958",
  //   utilsMintCount,
  //   utilsMintAmount
  // );
  // log("----------------------------------------------------");
  // console.log("Transfering Utils to Faucet...");
  // await transferToFaucet(
  //   account,
  //   "0xCA34FF4068f042203087D475805c4DD8347cE958",
  //   "0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E",
  //   utilsMintCount,
  //   transferUtilsAmount
  // );
  // log("----------------------------------------------------");
  try {
    console.log("Verifying for Utils...");
    await verify("0xCA34FF4068f042203087D475805c4DD8347cE958", utilsArg);
    console.log("Verifying for Map...");
    await verify("0x11DA0f57086a19977E46B548b64166411d839a30", mapArg);
    console.log("Verifying for Faucet...");
    await verify("0xdF78D5A57DCFf31Ca18978b56760867010AEBC2E", faucetArg);
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

const mintUtils = async (account, utilsContractAddress, count, amount) => {
  const utilsContract = await hre.ethers.getContractAt(
    "Utils",
    utilsContractAddress,
    account
  );
  for (let i = 1; i <= count; i++) {
    const tx = await utilsContract.mint(amount);
    console.log("Minted Utils " + i + " TX:", tx.hash);
    const receipt = await tx.wait();
    console.log("Minted Utils " + i + " RECEIPT:", receipt.transactionHash);
  }
};

const transferToFaucet = async (
  account,
  utilsContractAddress,
  faucetContractAddress,
  count,
  amount
) => {
  const utilsContract = await hre.ethers.getContractAt(
    "Utils",
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
    );
    console.log("Transfered Utils " + i + " TX:", tx.hash);
    const receipt = await tx.wait();
    console.log("Transfered Utils " + i + " RECEIPT:", receipt.transactionHash);
  }
};

module.exports.tags = ["all", "main"];
