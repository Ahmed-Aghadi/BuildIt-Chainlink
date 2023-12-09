using UnityEngine;
using Thirdweb;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ChainData
{
    public string identifier;
    public string chainId;
    public string rpcOverride;

    public ChainData(string identifier, string chainId, string rpcOverride)
    {
        this.identifier = identifier;
        this.chainId = chainId;
        this.rpcOverride = rpcOverride;
    }
}

public class ThirdwebManager : MonoBehaviour
{
    [Header("For gasless custom options")]
    [Tooltip("The gasless option to initialize the SDK with")]
    public Toggle gaslessToggle;

    [Header("REQUIRED SETTINGS")]
    [Tooltip("The chain to initialize the SDK with")]
    public string chain = "mumbai";

    [Header("CHAIN DATA")]
    [Tooltip("Support any chain by adding it to this list from the inspector")]
    public List<ChainData> supportedChains = new List<ChainData>()
    {
        new ChainData("mumbai", "80001", null),
/*        new ChainData("fantom-testnet", "4002", null),
        new ChainData("okexchain-testnet", "65", null),
        new ChainData("xdc-apothem-network", "51", null),
        new ChainData("theta-testnet", "365", null),*/
        new ChainData("polygon-zkevm-testnet", "1442", null),
        new ChainData("sepolia", "11155111", null),
/*        new ChainData("mantle-testnet", "5001", null),
        new ChainData("arbitrum-goerli", "421613", null),
        new ChainData("opbnb-testnet", "5611", null),
        new ChainData("binance-testnet", "97", null),*/
        new ChainData("goerli", "5", null),
        new ChainData("avalanche-fuji", "43113", null),
    };

    [Header("APP METADATA")]
    public string appName = "Thirdweb Game";
    public string appDescription = "Thirdweb Game Demo";
    public string[] appIcons = new string[] { "https://thirdweb.com/favicon.ico" };
    public string appUrl = "https://thirdweb.com";

    [Header("STORAGE OPTIONS")]
    [Tooltip("IPFS Gateway Override")]
    public string storageIpfsGatewayUrl = "https://gateway.ipfscdn.io/ipfs/";

    [Header("OZ DEFENDER OPTIONS")]
    [Tooltip("Autotask URL")]
    public string relayerUrl = null;

    [Tooltip("Forwarders can be found here https://github.com/thirdweb-dev/ozdefender-autotask")]
    public string forwarderAddress = null;

    [Tooltip("Forwarder Domain Override (Defaults to GSNv2 Forwarder if left empty)")]
    public string forwarderDomainOverride = null;

    [Tooltip("Forwarder Version (Defaults to 0.0.1 if left empty)")]
    public string forwaderVersionOverride = null;

    [Header("MAGIC LINK OPTIONS")]
    [Tooltip("Magic Link API Key (https://dashboard.magic.link)")]
    public string magicLinkApiKey = null;

    [Header("SMART WALLET OPTIONS")]
    [Tooltip("Factory Contract Address")]
    public string factoryAddress;

    [Tooltip("Thirdweb API Key (https://thirdweb.com/dashboard/api-keys)")]
    public string thirdwebApiKey;

    [Tooltip("Whether it should use a paymaster for gasless transactions or not")]
    public bool gasless;

    [Tooltip("Optional - If you want to use a custom relayer, you can provide the URL here")]
    public string bundlerUrl;

    [Tooltip("Optional - If you want to use a custom paymaster, you can provide the URL here")]
    public string paymasterUrl;

    [Tooltip("Optional - If you want to use a custom paymaster, you can provide the API key here")]
    public string paymasterAPI;

    [Tooltip("Optional - If you want to use a custom entry point, you can provide the contract address here")]
    public string entryPointAddress;

    [Header("NATIVE PREFABS (DANGER ZONE)")]
    [Tooltip("Instantiates the WalletConnect SDK for Native platforms.")]
    public GameObject WalletConnectPrefab;

    [Tooltip("Instantiates the MagicAuth SDK for Native platforms.")]
    public GameObject MagicAuthPrefab;

    [Tooltip("Instantiates the Metamask SDK for Native platforms.")]
    public GameObject MetamaskPrefab;

    public ThirdwebSDK SDK;

    public static ThirdwebManager Instance { get; private set; }

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
            Debug.LogWarning("Two ThirdwebManager instances were found, removing this one.");
            Destroy(this.gameObject);
            return;
        }
        InitializeSDKOnNetworkChange();
    }

    private void Start()
    {
        gaslessToggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(gaslessToggle);
        });
    }
    async void ToggleValueChanged(Toggle change)
    {
        if (!(await SDK.wallet.IsConnected()))
        {
            InitializeSDKOnNetworkChange();
        }
    }

    public ChainData GetChainData(string chainIdentifier)
    {
        return supportedChains.Find(x => x.identifier == chainIdentifier);
    }

    public ChainData GetCurrentChainData()
    {
        return supportedChains.Find(x => x.identifier == chain);
    }

    public int GetCurrentChainID()
    {
        return int.Parse(GetCurrentChainData().chainId);
    }

    public string GetCurrentChainIdentifier()
    {
        return chain;
    }

    public void InitializeSDKOnNetworkChange()
    {
        // Inspector chain data dictionary.

        ChainData currentChain = GetChainData(chain);

        // Chain ID must be provided on native platforms.

        int chainId = -1;

        if (!Utils.IsWebGLBuild())
        {
            if (string.IsNullOrEmpty(currentChain.chainId))
                throw new UnityException("You must provide a Chain ID on native platforms!");

            if (!int.TryParse(currentChain.chainId, out chainId))
                throw new UnityException("The Chain ID must be a non-negative integer!");
        }

        // Must provide a proper chain identifier (https://thirdweb.com/dashboard/rpc) or RPC override.

        string chainOrRPC = null;

        if (!string.IsNullOrEmpty(currentChain.rpcOverride))
        {
            if (!currentChain.rpcOverride.StartsWith("https://"))
                throw new UnityException("RPC overrides must start with https:// !");
            else
                chainOrRPC = currentChain.rpcOverride;
        }
        else
        {
            if (string.IsNullOrEmpty(currentChain.identifier))
                throw new UnityException("When not providing an RPC, you must provide a chain identifier!");
            else
                chainOrRPC = currentChain.identifier;
        }

        // Set up storage and gasless options (if an)

        var options = new ThirdwebSDK.Options();

        if (!string.IsNullOrEmpty(storageIpfsGatewayUrl))
        {
            options.storage = new ThirdwebSDK.StorageOptions() { ipfsGatewayUrl = storageIpfsGatewayUrl };
        }

        Debug.Log("Gasless Toggle: " + gaslessToggle.isOn.ToString());
        Debug.Log("ChainId Check: " + currentChain.chainId);
        if (gaslessToggle.isOn && (currentChain.chainId == "11155111" || currentChain.chainId == "250" || !string.IsNullOrEmpty(relayerUrl) && !string.IsNullOrEmpty(forwarderAddress)))
        {
            Debug.Log("Found ChainId: " + currentChain.chainId);
            string relayerUrlSepolia = "https://api.defender.openzeppelin.com/autotasks/57396929-b8bf-4078-83b4-b44c4f588809/runs/webhook/8db4ba89-3c75-4a75-9f0e-98d36b4337a3/LqU2XjBemGpVYS3CqXFCd4";
            string forwarderAddressSepolia = "0x3EC31e8B991FF0b3FfffD480e2A5F259B51DdF5c";

            string relayerUrlFantom = "https://api.defender.openzeppelin.com/autotasks/6c73d722-9ac6-4e23-b9f2-2d7217c69394/runs/webhook/8db4ba89-3c75-4a75-9f0e-98d36b4337a3/UCeui23vKEPi4b7ZG7zLLg";
            string forwarderAddressFantom = "0x65D84C0883e0e0c9c41B044b4523cd07999924Fe";

            string relayerUrl = currentChain.chainId == "11155111" ? relayerUrlSepolia : relayerUrlFantom;
            string forwarderAddress = currentChain.chainId == "11155111" ? forwarderAddressSepolia : forwarderAddressFantom;
            options.gasless = new ThirdwebSDK.GaslessOptions()
            {
                openzeppelin = new ThirdwebSDK.OZDefenderOptions()
                {
                    relayerUrl = currentChain.chainId == "11155111" || currentChain.chainId == "250" ? relayerUrl : this.relayerUrl,
                    relayerForwarderAddress = currentChain.chainId == "11155111" || currentChain.chainId == "250" ? forwarderAddress : this.forwarderAddress,
                    domainName = string.IsNullOrEmpty(this.forwarderDomainOverride) ? "GSNv2 Forwarder" : this.forwarderDomainOverride,
                    domainVersion = string.IsNullOrEmpty(this.forwaderVersionOverride) ? "0.0.1" : this.forwaderVersionOverride
                }
            };
        }

        // Set up wallet data
        options.wallet = new ThirdwebSDK.WalletOptions()
        {
            appName = string.IsNullOrEmpty(appName) ? "Thirdweb Game" : appName,
            appDescription = string.IsNullOrEmpty(appDescription) ? "Thirdweb Game Demo" : appDescription,
            appIcons = appIcons.Length == 0 ? new string[] { "https://thirdweb.com/favicon.ico" } : appIcons,
            appUrl = string.IsNullOrEmpty(appUrl) ? "https://thirdweb.com" : appUrl,
            magicLinkApiKey = string.IsNullOrEmpty(magicLinkApiKey) ? null : magicLinkApiKey,
        };

        options.smartWalletConfig =
            string.IsNullOrEmpty(factoryAddress) || string.IsNullOrEmpty(thirdwebApiKey)
                ? null
                : new ThirdwebSDK.SmartWalletConfig()
                {
                    factoryAddress = factoryAddress,
                    thirdwebApiKey = thirdwebApiKey,
                    gasless = gasless,
                    bundlerUrl = bundlerUrl,
                    paymasterUrl = paymasterUrl,
                    paymasterAPI = paymasterAPI,
                    entryPointAddress = entryPointAddress
                };

        SDK = new ThirdwebSDK(chainOrRPC, chainId, options);
    }
}
