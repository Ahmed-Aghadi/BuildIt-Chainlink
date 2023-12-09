require("@nomicfoundation/hardhat-foundry");
// require("@nomiclabs/hardhat-waffle")
require("@nomiclabs/hardhat-etherscan");
require("hardhat-deploy");
// require("solidity-coverage")
// require("hardhat-gas-reporter")
// require("hardhat-contract-sizer")
require("@nomiclabs/hardhat-ethers");
require("dotenv").config();

const MUMBAI_RPC_URL =
  process.env.MUMBAI_RPC_URL || "https://rpc-mumbai.matic.today";
const SEPOLIA_RPC_URL =
  process.env.SEPOLIA_RPC_URL || "https://rpc-mumbai.matic.today";
const FANTOM_TESTNET_RPC_URL =
  process.env.FANTOM_TESTNET_RPC_URL || "https://rpc.testnet.fantom.network";
const FANTOM_RPC_URL =
  process.env.FANTOM_RPC_URL || "https://rpc.testnet.fantom.network";
const GOERLI_RPC_URL =
  process.env.GOERLI_RPC_URL ||
  "https://eth-rinkeby.alchemyapi.io/v2/your-api-key";
const MAINNET_RPC_URL =
  process.env.MAINNET_RPC_URL ||
  process.env.ALCHEMY_MAINNET_RPC_URL ||
  "https://eth-mainnet.alchemyapi.io/v2/your-api-key";
const RINKEBY_RPC_URL =
  process.env.RINKEBY_RPC_URL ||
  "https://eth-rinkeby.alchemyapi.io/v2/your-api-key";
const KOVAN_RPC_URL =
  process.env.KOVAN_RPC_URL ||
  "https://eth-kovan.alchemyapi.io/v2/your-api-key";
const POLYGON_MAINNET_RPC_URL =
  process.env.POLYGON_MAINNET_RPC_URL ||
  "https://polygon-mainnet.alchemyapi.io/v2/your-api-key";
const BSC_TESTNET_RPC_URL =
  process.env.BSC_TESTNET_RPC_URL ||
  "https://polygon-mainnet.alchemyapi.io/v2/your-api-key";
const OP_BNB_TESTNET_RPC_URL =
  process.env.OP_BNB_TESTNET_RPC_URL ||
  "https://polygon-mainnet.alchemyapi.io/v2/your-api-key";
const POLYGONZKEVM_TESTNET_RPC_URL =
  process.env.POLYGONZKEVM_TESTNET_RPC_URL ||
  "https://rpc.public.zkevm-test.net";
const AVALANCHE_FUJI_TESTNET_RPC_URL =
  process.env.AVALANCHE_FUJI_TESTNET_RPC_URL ||
  "https://rpc.ankr.com/avalanche_fuji";
const PRIVATE_KEY = process.env.PRIVATE_KEY || "0x";
const MAIN_PRIVATE_KEY = process.env.MAIN_PRIVATE_KEY || "0x";

// Your API key for Etherscan, obtain one at https://etherscan.io/
const ETHERSCAN_API_KEY =
  process.env.ETHERSCAN_API_KEY || "Your etherscan API key";
const POLYGONSCAN_API_KEY =
  process.env.POLYGONSCAN_API_KEY || "Your polygonscan API key";
const FANTOMSCAN_API_KEY =
  process.env.FANTOMSCAN_API_KEY || "Your polygonscan API key";
const POLYGONZKEVM_TESTNET_API_KEY =
  process.env.POLYGONZKEVM_TESTNET_API_KEY || "Your polygonscan API key";
const POLYGONSCAN_ZKEVM_API_KEY =
  process.env.POLYGONSCAN_ZKEVM_API_KEY || "Your polygonscan API key";
const MANTLE_TESTNET_API_KEY =
  process.env.MANTLE_TESTNET_API_KEY || "Your polygonscan API key";
const ARBITRUM_GOERLI_TESTNET_API_KEY =
  process.env.ARBITRUM_GOERLI_TESTNET_API_KEY || "Your polygonscan API key";
const AVALANCHE_FUJI_TESTNET_API_KEY =
  process.env.AVALANCHE_FUJI_TESTNET_API_KEY || "Your polygonscan API key";
const BSCSCAN_API_KEY =
  process.env.BSCSCAN_API_KEY || "Your polygonscan API key";
const NODE_REAL_API_KEY =
  process.env.BSCSCAN_API_KEY || "Your polygonscan API key";

/** @type import('hardhat/config').HardhatUserConfig */
module.exports = {
  defaultNetwork: "hardhat",
  networks: {
    hardhat: {
      // // If you want to do some forking, uncomment this
      // forking: {
      //   url: MAINNET_RPC_URL
      // }
      chainId: 31337,
    },
    localhost: {
      chainId: 31337,
    },
    kovan: {
      url: KOVAN_RPC_URL,
      accounts: PRIVATE_KEY !== undefined ? [PRIVATE_KEY] : [],
      //accounts: {
      //     mnemonic: MNEMONIC,
      // },
      saveDeployments: true,
      chainId: 42,
    },
    rinkeby: {
      url: RINKEBY_RPC_URL,
      accounts: PRIVATE_KEY !== undefined ? [PRIVATE_KEY] : [],
      //   accounts: {
      //     mnemonic: MNEMONIC,
      //   },
      saveDeployments: true,
      chainId: 4,
    },
    goerli: {
      url: GOERLI_RPC_URL,
      accounts: PRIVATE_KEY !== undefined ? [PRIVATE_KEY] : [],
      //   accounts: {
      //     mnemonic: MNEMONIC,
      //   },
      saveDeployments: true,
      chainId: 5,
    },
    mainnet: {
      url: MAINNET_RPC_URL,
      accounts: PRIVATE_KEY !== undefined ? [PRIVATE_KEY] : [],
      //   accounts: {
      //     mnemonic: MNEMONIC,
      //   },
      saveDeployments: true,
      chainId: 1,
    },
    polygon: {
      url: POLYGON_MAINNET_RPC_URL,
      accounts: PRIVATE_KEY !== undefined ? [PRIVATE_KEY] : [],
      saveDeployments: true,
      chainId: 137,
    },
    // Mumbai testnet
    mumbai: {
      url: MUMBAI_RPC_URL,
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 80001,
      // gas: 100000000000,
    },
    // fantom testnet
    fantomtest: {
      url: FANTOM_TESTNET_RPC_URL,
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 4002,
      // gas: 500000,
    },
    thetatest: {
      chainId: 365,
      url: "https://eth-rpc-api-testnet.thetatoken.org/rpc",
      accounts: [PRIVATE_KEY],
      timeout: 300000, // 300 seconds
    },
    okextest: {
      chainId: 65,
      url: "https://exchaintestrpc.okex.org",
      accounts: [PRIVATE_KEY],
      timeout: 300000, // 300 seconds
    },
    xdcapothem: {
      chainId: 51,
      url: "https://erpc.apothem.network",
      accounts: [PRIVATE_KEY],
      timeout: 300000, // 300 seconds
    },
    polygonzkevmtest: {
      chainId: 1442,
      url: "https://rpc.public.zkevm-test.net",
      accounts: [PRIVATE_KEY],
      timeout: 300000, // 300 seconds
    },
    sepolia: {
      url: SEPOLIA_RPC_URL,
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 11155111,
    },
    mantletest: {
      chainId: 5001,
      url: "https://rpc.testnet.mantle.xyz",
      accounts: [PRIVATE_KEY],
      timeout: 300000, // 300 seconds
    },
    // fantom mainnet
    fantom: {
      url: FANTOM_RPC_URL,
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 250,
      // gas: 500000,
    },
    arbitrumGoerli: {
      url: "https://arbitrum-goerli.blockpi.network/v1/rpc/public",
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 421613,
      timeout: 300000, // 300 seconds
    },
    bscTestnet: {
      url: BSC_TESTNET_RPC_URL,
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 97,
      timeout: 300000, // 300 seconds
    },
    opBNBTestnet: {
      url: OP_BNB_TESTNET_RPC_URL,
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 5611,
      timeout: 300000, // 300 seconds
    },
    polygonzkevmtest: {
      url: POLYGONZKEVM_TESTNET_RPC_URL,
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 1442,
      timeout: 300000, // 300 seconds
    },
    avalancheFujiTestnet: {
      url: AVALANCHE_FUJI_TESTNET_RPC_URL,
      // accounts: [MAIN_PRIVATE_KEY],
      accounts: [PRIVATE_KEY],
      saveDeployments: true,
      chainId: 43113,
      timeout: 300000, // 300 seconds
    },
  },
  etherscan: {
    // To list networks supported by default: npx hardhat verify --list-networks
    // You can manually add support for it by following these instructions: https://hardhat.org/verify-custom-networks
    // npx hardhat verify --network <NETWORK> <CONTRACT_ADDRESS> <CONSTRUCTOR_PARAMETERS>
    apiKey: {
      goerli: ETHERSCAN_API_KEY,
      polygonMumbai: POLYGONSCAN_API_KEY,
      fantomtest: FANTOMSCAN_API_KEY,
      ftmTestnet: FANTOMSCAN_API_KEY,
      polygonzkevmtest: POLYGONSCAN_ZKEVM_API_KEY,
      sepolia: ETHERSCAN_API_KEY,
      mantletest: MANTLE_TESTNET_API_KEY,
      fantom: FANTOMSCAN_API_KEY,
      opera: FANTOMSCAN_API_KEY, // fantom opera
      arbitrumGoerli: ARBITRUM_GOERLI_TESTNET_API_KEY,
      bscTestnet: BSCSCAN_API_KEY,
      opBNBTestnet: NODE_REAL_API_KEY,
      avalancheFujiTestnet: AVALANCHE_FUJI_TESTNET_API_KEY,
    },
    customChains: [
      {
        network: "polygonzkevmtest",
        chainId: 1442,
        urls: {
          apiURL: "https://api-testnet-zkevm.polygonscan.com/api",
          browserURL: "https://testnet-zkevm.polygonscan.com/",
        },
      },
      {
        network: "mantletest",
        chainId: 5001,
        urls: {
          apiURL: "https://explorer.testnet.mantle.xyz/api",
          browserURL: "https://explorer.testnet.mantle.xyz",
        },
      },
      {
        network: "opBNBTestnet",
        chainId: 5611,
        urls: {
          apiURL: "https://opbnbscan.com/api",
          browserURL: "https://opbnbscan.com/",
        },
      },
      {
        // https://docs.avascan.info/tutorial/smart-contract-submission
        network: "avalancheFujiTestnet",
        chainId: 43113,
        urls: {
          apiURL:
            "https://api.avascan.info/v2/network/testnet/evm/43113/etherscan",
          browserURL: "https://testnet.avascan.info/blockchain/c/",
        },
      },
    ],
  },
  // gasReporter: {
  //   enabled: REPORT_GAS,
  //   currency: "USD",
  //   outputFile: "gas-report.txt",
  //   noColors: true,
  //   // coinmarketcap: process.env.COINMARKETCAP_API_KEY,
  // },
  // contractSizer: {
  //   runOnCompile: false,
  //   // only: ["Raffle"],
  // },
  namedAccounts: {
    deployer: {
      default: 0, // here this will by default take the first account as deployer
      1: 0, // similarly on mainnet it will take the first account as deployer. Note though that depending on how hardhat network are configured, the account 0 on one network can be different than on another
    },
    player: {
      default: 1,
    },
  },
  solidity: {
    version: "0.8.13",
    settings: {
      optimizer: {
        enabled: true,
        runs: 200,
      },
    },
  },
  mocha: {
    timeout: 300000, // 300 seconds max for running tests
  },
};
