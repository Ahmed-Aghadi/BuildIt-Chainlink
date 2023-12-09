using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Thirdweb.Contracts.DirectListingsLogic.ContractDefinition;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
public class DesignJsonStructure
{
    public string label
    {
        get;
        set;
    }
    // value can be : 0 - empty, 1 - road, 2 - house, 3 - special
    public List<int> design // 0th index means 0,0 and from 0,0 to 0,1 , ... 0,n and then 1,0 to 1,1 , ... 1,n and so on
    {
        get;
        set;
    }
}

public class EnsLookupResponse
{
    public string ensName
    {
        get;
        set;
    }
    public string ensAvatarUrl
    {
        get;
        set;
    }
}

public class CanvasManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetDesigns();
    [DllImport("__Internal")]
    private static extern void SaveDesign(string design);
    [DllImport("__Internal")]
    private static extern void GetENS(string address);

    public Canvas canvas;
    public RectTransform buildingMenuRect;
    public Button editButton;
    public Button confirmButton;
    public Button cancelButton;
    public Prefab_ConnectWallet prefab_connectWallet;
    public UIController uiController;
    public GameObject buildingMenu;
    public GameObject faucetButton;
    public Button marketplaceButton;
    public Text marketplaceButtonText;
    bool isMarketplaceOpen = false;
    public InputManager inputManager;
    public PlacementManager placementManager;
    public GameObject lightBlueHighlightPrefab;
    public GameObject redHighlightPrefab;
    List<GameObject> highlightPrefabs;

    [Header("For Marketplace Panel")]
    public GameObject marketplacePanel;
    public GameObject marketplaceLoadingPanel;
    public TMP_Text errorText;
    public TMP_Dropdown typeDropdown;
    public TMP_InputField priceInput;
    public TMP_Text priceInputLabel;
    public Toggle usdToggle;
    public TMP_InputField durationInput;
    public TMP_InputField linkAmountInput;
    public GameObject buttonsContainer;
    public Button createListingButton;
    public Button cancelListingButton;

    [Header("For Listing Panel")]
    public GameObject listingsPanel;
    public GameObject listingsContainer;
    public GameObject listingPrefab;
    public RawImage ensImage;
    List<GameObject> listingPrefabs;

    [Header("For Listing Handle Panel")]
    public GameObject listingHandlePanel;
    public TMP_InputField bidInput;
    public TMP_Text priceText;
    public Button confirmListingButton;
    public TMP_Text confirmListingButtonText;

    [Header("For Transfer Panel (cross chain)")]
    public Button transferUtilsButton; // to activate transferPanel
    public GameObject transferPanel;
    public TMP_Dropdown chainDropdown;
    public TMP_Dropdown tokenDropdown;
    public TMP_InputField amountInput;
    public Button transferButton;
    public Button cancelTransferButton;

    [Header("For Saving Land Desing")]
    public Button landDesignSaveButton;
    public Text landDesignSaveButtonText;
    public GameObject landDesignSavePanel;
    public TMP_Dropdown loadSaveTypeDropdown;
    public TMP_Dropdown selectDesignDropdown;
    public Button saveLoadButton;
    public TMP_Text saveLoadButtonText;
    public TMP_InputField labelInput;
    bool isLandDesignSaveOpen = false;
    bool isLandDesignSaveSelectOpen = false; // probably not needed
    List<DesignJsonStructure> designs;
    int xIndexSelected, yIndexSelected;


    // for marketplace panel
    bool isError = false;
    string errorTextString = "ERROR!!!";
    float timePassed = 0;
    float errorDuration = 5f;
    int xIndex, yIndex;

    // Start is called before the first frame update
    void Start()
    {
        highlightPrefabs = new List<GameObject>();
        marketplacePanel.SetActive(false);
        marketplaceLoadingPanel.SetActive(false);
        errorText.gameObject.SetActive(false);
        durationInput.gameObject.SetActive(false);
        linkAmountInput.gameObject.SetActive(false);
        typeDropdown.onValueChanged.AddListener(delegate
        {
            OnTypeDropdownValueChanged(typeDropdown);
        });
        createListingButton.onClick.AddListener(delegate
        {
            OnCreateListing();
        });
        cancelListingButton.onClick.AddListener(delegate
        {
            OnCancelListing();
        });

        listingPrefabs = new List<GameObject>();
        listingsPanel.SetActive(false);

        transferPanel.SetActive(false);
        transferUtilsButton.onClick.AddListener(delegate
        {
            ShowTransferPanel();
        });
        transferButton.onClick.AddListener(delegate
        {
            OnTransfer();
        });
        cancelTransferButton.onClick.AddListener(delegate
        {
            HideTransferPanel();
        });

        listingHandlePanel.SetActive(false);
        // SellLand();
        landDesignSaveButton.gameObject.SetActive(false);
        landDesignSavePanel.SetActive(false);
        selectDesignDropdown.gameObject.SetActive(false);
        loadSaveTypeDropdown.onValueChanged.AddListener(delegate
        {
            OnSelectDesignDropdownValueChanged(loadSaveTypeDropdown);
        });
        saveLoadButton.onClick.AddListener(delegate
        {
            OnSaveLoad();
        });
    }

    void OnSaveLoad()
    {
        if (loadSaveTypeDropdown.value == 0) // save
        {
            if (labelInput.text == "")
            {
                // uiController.ShowError("Please enter a label for the design");
                return;
            }
            string label = labelInput.text;
            DesignJsonStructure design = new DesignJsonStructure();
            design.label = label;
            design.design = new List<int>();
            // int count = 0;
            for (int i = (xIndex * ContractManager.Instance.perSize) + 0; i < ((xIndex + 1) * ContractManager.Instance.perSize); i++)
            {
                for (int j = (yIndex * ContractManager.Instance.perSize) + 0; j < ((yIndex + 1) * ContractManager.Instance.perSize); j++)
                {
                    design.design.Add(ContractManager.Instance.editedMap[i, j]);
                }
            }
            string designJson = JsonConvert.SerializeObject(design);
            Debug.Log("designJson: " + designJson);
            SaveDesign(designJson);
            HideLandDesignSaveLoadPanel();
        }
        else if (loadSaveTypeDropdown.value == 1) // load
        {
            if (selectDesignDropdown.options.Count == 0)
            {
                // uiController.ShowError("No designs found");
                return;
            }
            string designLabel = selectDesignDropdown.options[selectDesignDropdown.value].text;
            Debug.Log("designLabel: " + designLabel);
            DesignJsonStructure design = designs.Find((currentDesign) => currentDesign.label == designLabel);
            if (design == null)
            {
                Debug.Log("design not found");
            }
            Debug.Log("designLabel found: " + design.label);
            int count = 0;
            for (int i = (xIndex * ContractManager.Instance.perSize) + 0; i < ((xIndex + 1) * ContractManager.Instance.perSize); i++)
            {
                for (int j = (yIndex * ContractManager.Instance.perSize) + 0; j < ((yIndex + 1) * ContractManager.Instance.perSize); j++)
                {
                    Vector3Int position = new Vector3Int(i, 0, j);
                    ContractManager.Instance.placeItem(position, design.design[count], true);
                    count++;
                }
            }
            HideLandDesignSaveLoadPanel();
        }
    }

    void OnSelectDesignDropdownValueChanged(TMP_Dropdown change)
    {
        Debug.Log("OnSelectDesignDropdownValueChanged: " + change.value);
        Debug.Log("saveLoadButtonLabel: " + saveLoadButtonText.text);
        if (change.value == 0) // save
        {
            labelInput.gameObject.SetActive(true);
            selectDesignDropdown.gameObject.SetActive(false);
            saveLoadButtonText.text = "Save";
        }
        else if (change.value == 1) // load
        {
            labelInput.gameObject.SetActive(false);
            selectDesignDropdown.gameObject.SetActive(true);
            saveLoadButtonText.text = "Load";
        }
    }

    async void OnTransfer()
    {
        int destinationChainId = -1;
        bool success = false;
        var chainId = await ThirdwebManager.Instance.SDK.wallet.GetChainId();
        int sourceChainId = chainId;
        if (chainId != 80001 && chainId != 11155111 && chainId != 5 && chainId != 1442)
        {
            return;
        }
        if (chainDropdown.options[chainDropdown.value].text == "Polygon Mumbai")
        {
            destinationChainId = 80001;
        }
        else if (chainDropdown.options[chainDropdown.value].text == "Sepolia")
        {
            destinationChainId = 11155111;
        }
        else if (chainDropdown.options[chainDropdown.value].text == "Goerli")
        {
            destinationChainId = 5;
        }
        else if (chainDropdown.options[chainDropdown.value].text == "Polygon ZKEVM Testnet")
        {
            destinationChainId = 1442;
        }
        else if (chainDropdown.options[chainDropdown.value].text == "Avalanche Fuji Testnet")
        {
            destinationChainId = 43113;
        }
        if (destinationChainId != -1)
        {
            int tokenId = ContractManager.EMPTY;
            if (tokenDropdown.value == 0)
            {
                tokenId = ContractManager.ROAD;
            }
            else if (tokenDropdown.value == 1)
            {
                tokenId = ContractManager.HOUSE;
            }
            else if (tokenDropdown.value == 2)
            {
                tokenId = ContractManager.SPECIAL;
            }

            if (tokenId == ContractManager.EMPTY)
            {
                return;
            }

            success = await ContractManager.Instance.TransferUtilsCrossChain(sourceChainId, destinationChainId, tokenId, amountInput.text);
        }
        else
        {
            return;
        }
        if (success)
        {
            HideTransferPanel();
            await ContractManager.Instance.setItemBalances();
        }
    }

    async void ShowTransferPanel()
    {
        var chainId = await ThirdwebManager.Instance.SDK.wallet.GetChainId();
        if (chainId != 80001 && chainId != 5 && chainId != 11155111 && chainId != 1442)
        {
            return;
        }
        transferPanel.SetActive(true);
        chainDropdown.ClearOptions();
        var optionDatas = new List<TMP_Dropdown.OptionData>();
        if (chainId == 80001)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Sepolia" });
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Avalanche Fuji Testnet" });
        }
        else if (chainId == 11155111)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Polygon Mumbai" });
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Avalanche Fuji Testnet" });
        }
        else if (chainId == 43113)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Sepolia" });
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Polygon Mumbai" });
        }
        else if (chainId == 5)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Polygon ZKEVM Testnet" });
        }
        else if (chainId == 1442)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData() { text = "Goerli" });
        }
        else
        {
            return;
        }
        chainDropdown.AddOptions(optionDatas);
    }

    void HideTransferPanel()
    {
        transferPanel.SetActive(false);
        chainDropdown.ClearOptions();
    }

    public void ShowTransferUtilsButton()
    {
        transferUtilsButton.gameObject.SetActive(true);
    }

    public void HideTransferUtilsButton()
    {
        transferUtilsButton.gameObject.SetActive(false);
    }

    void OnTypeDropdownValueChanged(TMP_Dropdown dropdown)
    {
        if (dropdown.value == 0)
        {
            durationInput.gameObject.SetActive(false);
            linkAmountInput.gameObject.SetActive(false);
            buttonsContainer.transform.localPosition = new Vector3(0, 0, 0);
            priceInputLabel.text = "Price";
        }
        else if (dropdown.value == 1)
        {
            durationInput.gameObject.SetActive(true);
            linkAmountInput.gameObject.SetActive(true);
            buttonsContainer.transform.localPosition = new Vector3(0, -80, 0);
            priceInputLabel.text = "Minimum Bid";
        }
    }

    // Update is called once per frame
    void Update()
    {

        // For adjust the scaling of Prefab Connect Wallet
        /*var canvasHeight = canvas.pixelRect.height;
        var canvasWidth = canvas.pixelRect.width;
        var canvasScale = canvas.scaleFactor;*/
        var canvasRect = canvas.GetComponent<RectTransform>().rect;
        /*var canvasRectWidth = canvasRect.width;*/
        var canvasRectHeight = canvasRect.height;
        var scaleCalc = canvasRectHeight / 540;
        prefab_connectWallet.transform.localScale = new Vector3(scaleCalc, scaleCalc, scaleCalc);

        // For handling timing for Error Text of Marketplace panel
        if (isError)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= errorDuration)
            {
                isError = false;
                timePassed = 0;
                // errorTextString = "ERROR!!!";
                errorText.gameObject.SetActive(false);
            }
        }

        // For highlight
        handleMarketplaceHighlightPanel();
        handleMapDesignHighlightPanel();
    }

    private void handleMarketplaceHighlightPanel()
    {
        if (isMarketplaceOpen && !marketplacePanel.activeSelf && !listingsPanel.activeSelf)
        {
            var position = inputManager.RaycastGround();
            if (position != null)
            {
                if (placementManager.CheckIfPositionInBound(position.Value) == true)
                {
                    if (ContractManager.Instance.userOwnsIndex(position.Value.x, position.Value.z))
                    {
                        highlightLand(position.Value.x, position.Value.z, lightBlueHighlightPrefab);
                    }
                    else
                    {
                        highlightLand(position.Value.x, position.Value.z, redHighlightPrefab);
                    }
                }
            }
            else
            {
                if (highlightPrefabs.Count != 0)
                {
                    ResetHighlightPrefabsList();
                }
            }

        }

        if (isMarketplaceOpen && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false && !marketplacePanel.activeSelf && !listingsPanel.activeSelf)
        {
            var position = inputManager.RaycastGround();
            if (position != null)
            {
                if (placementManager.CheckIfPositionInBound(position.Value) == true)
                {
                    Debug.Log("Before Pos" + position.Value.x + ", " + position.Value.z);
                    int perSize = ContractManager.Instance.perSize;
                    if (perSize == 0)
                    {
                        return;
                    }
                    int xIndex = position.Value.x / perSize;
                    int yIndex = position.Value.z / perSize;
                    Debug.Log("After Pos" + xIndex + ", " + yIndex);

                    if (ContractManager.Instance.landListedIndex(xIndex, yIndex))
                    {
                        Debug.Log("Buy Land");
                        ShowBuyLandPanel();
                    }
                    else if (ContractManager.Instance.userOwnsIndex(position.Value.x, position.Value.z))
                    {
                        Debug.Log("Sell Land");
                        SellLand();
                    }
                }
            }
        }
    }

    private void ShowLandDesignSaveLoadPanel()
    {
        landDesignSavePanel.SetActive(true);
        // load all the designs and add in the dropdown
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        GetDesigns();
#endif
    }

    public void SetDesigns(string designsJson)
    {
        Debug.Log("SetDesigns: " + designsJson);
        designs = JsonConvert.DeserializeObject<List<DesignJsonStructure>>(designsJson);
        selectDesignDropdown.ClearOptions();
        var optionsToAdd = new List<TMP_Dropdown.OptionData>();
        foreach (var design in designs)
        {
            optionsToAdd.Add(new TMP_Dropdown.OptionData() { text = design.label });
        }
        selectDesignDropdown.AddOptions(optionsToAdd);
    }

    public void HideLandDesignSaveLoadPanel()
    {
        landDesignSavePanel.SetActive(false);
        ResetLandDesignSaveLoadPanel();
    }

    private void ResetLandDesignSaveLoadPanel()
    {
        loadSaveTypeDropdown.value = 0;
        selectDesignDropdown.value = 0;
        labelInput.text = "";
    }

    private void SelectLand(int xIndex, int yIndex)
    {
        xIndexSelected = xIndex;
        yIndexSelected = yIndex;
        ShowLandDesignSaveLoadPanel();
    }

    private void handleMapDesignHighlightPanel()
    {
        if (isLandDesignSaveOpen && !landDesignSavePanel.activeSelf)
        {
            var position = inputManager.RaycastGround();
            if (position != null)
            {
                if (placementManager.CheckIfPositionInBound(position.Value) == true)
                {
                    if (ContractManager.Instance.userOwnsIndex(position.Value.x, position.Value.z))
                    {
                        highlightLand(position.Value.x, position.Value.z, lightBlueHighlightPrefab);
                    }
                    else
                    {
                        highlightLand(position.Value.x, position.Value.z, redHighlightPrefab);
                    }
                }
            }
            else
            {
                if (highlightPrefabs.Count != 0)
                {
                    ResetHighlightPrefabsList();
                }
            }

        }

        if (isLandDesignSaveOpen && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false && !landDesignSavePanel.activeSelf)
        {
            var position = inputManager.RaycastGround();
            if (position != null)
            {
                if (placementManager.CheckIfPositionInBound(position.Value) == true)
                {
                    Debug.Log("Before Pos" + position.Value.x + ", " + position.Value.z);
                    int perSize = ContractManager.Instance.perSize;
                    if (perSize == 0)
                    {
                        return;
                    }
                    int xIndex = position.Value.x / perSize;
                    int yIndex = position.Value.z / perSize;
                    Debug.Log("After Pos" + xIndex + ", " + yIndex);

                    if (ContractManager.Instance.userOwnsIndex(position.Value.x, position.Value.z))
                    {
                        Debug.Log("Save/Load Land");
                        SelectLand(xIndex, yIndex);
                    }
                }
            }
        }
    }

    void ShowError(string text)
    {
        errorTextString = text;
        isError = true;
        timePassed = 0;
        errorText.gameObject.SetActive(true);
        errorText.text = errorTextString;
    }
    void highlightLand(int x, int y, GameObject prefab)
    {
        if (highlightPrefabs.Count != 0)
        {
            ResetHighlightPrefabsList();
        }
        int perSize = ContractManager.Instance.perSize;
        if (perSize == 0)
        {
            return;
        }
        int x1 = (int)(x / perSize) * perSize;
        int y1 = (int)(y / perSize) * perSize;
        int x2 = x1 + perSize - 1;
        int y2 = y1 + perSize - 1;
        int tmpY1 = y1;
        while (x1 <= x2)
        {
            while (y1 <= y2)
            {

                var current = GameObject.Instantiate(prefab);
                current.transform.localPosition = new Vector3(x1, 0.002f, y1);
                highlightPrefabs.Add(current);
                y1++;
            }
            y1 = tmpY1;
            x1++;
        }
    }

    void SellLand()
    {
        marketplacePanel.SetActive(true);
        ResetMarketplacePanel();
    }

    public void SetEnsDetails(string ensDetails)
    {
        Debug.Log("SetEnsDetails: " + ensDetails);
        var ensDetailsJson = JsonConvert.DeserializeObject<EnsLookupResponse>(ensDetails);
        SetSellerEnsName(ensDetailsJson.ensName, ensDetailsJson.ensAvatarUrl);
    }

    void SetSellerEnsName(string ensName, string ensAvatarUrl)
    {
        var userAddressTransform = listingsPanel.transform.Find("UserAddress_ENS");
        TMP_Text userAddressText = userAddressTransform.gameObject.GetComponent<TMP_Text>();
        userAddressText.text = ensName;
        StartCoroutine(LoadAvatar(ensAvatarUrl, ensImage));
    }

    private IEnumerator LoadAvatar(string url, RawImage ensImage)
    {
        Debug.Log("LoadAvatar: " + url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Debug.Log("setting texture: " + ((DownloadHandlerTexture)request.downloadHandler).texture);
            ensImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log("set texture");
        }
    }

    void ShowBuyLandPanel()
    {
        List<Listing> listingsList = ContractManager.Instance.GetListingsIndex(xIndex, yIndex);
        if (listingsList == null) return;
        Listing[] listings = listingsList.ToArray();
        listingsPanel.SetActive(true);
        var userAddressTransform = listingsPanel.transform.Find("UserAddress_ENS");
        TMP_Text userAddressText = userAddressTransform.gameObject.GetComponent<TMP_Text>();
        var sellerAddress = listings[0].sellerAddress;
        userAddressText.text = sellerAddress.Substring(0, 6) + "..." + sellerAddress.Substring(sellerAddress.Length - 4);
        bool isSellerAddressSet = false;
        int i = 1;
        float height = 0;
        for (int j = 1; j <= listings.Length; j++)
        {
            Debug.Log("Listing: " + i);
            Listing listing = listings[j - 1];
            if (listing.isValid)
            {
                if (!isSellerAddressSet)
                {
                    GetENS(listing.sellerAddress);
                    isSellerAddressSet = true;
                }
                Debug.Log("Listing Valid");
                Debug.Log("Instantiating: " + i);
                var listingPanel = GameObject.Instantiate(listingPrefab, listingsContainer.transform);
                listingPrefabs.Add(listingPanel);
                Debug.Log("Instantiated!!!");
                var rectListingPanel = listingPanel.GetComponent<RectTransform>().rect;
                if (rectListingPanel == null) return;
                float currentHeight = (25 * i) + ((i - 1) * rectListingPanel.height);
                rectListingPanel.position = new Vector2(rectListingPanel.x, currentHeight);
                height = currentHeight + rectListingPanel.height;

                Debug.Log("1");
                var headerPanelTransform = listingPanel.transform.Find("HeaderPanel");
                if (headerPanelTransform == null) return;
                Debug.Log("2");
                var priceLabelTransform = headerPanelTransform.Find("PriceLabel");
                if (priceLabelTransform == null) return;
                Debug.Log("3");
                TMP_Text priceLabel = priceLabelTransform.gameObject.GetComponent<TMP_Text>();
                if (priceLabel == null) return;

                Debug.Log("4");
                var valuePanelTransform = listingPanel.transform.Find("ValuePanel");
                if (valuePanelTransform == null) return;
                Debug.Log("5");
                var priceValueTransform = valuePanelTransform.Find("PriceValue");
                if (priceValueTransform == null) return;
                Debug.Log("6");
                TMP_Text priceValue = priceValueTransform.gameObject.GetComponent<TMP_Text>();
                if (priceValue == null) return;
                Debug.Log("7");
                var typeValueTransform = valuePanelTransform.Find("TypeValue");
                if (typeValueTransform == null) return;
                Debug.Log("8");
                TMP_Text typeValue = typeValueTransform.gameObject.GetComponent<TMP_Text>();
                if (typeValue == null) return;
                Debug.Log("9");
                var buttonTransform = valuePanelTransform.Find("Button");
                if (buttonTransform == null) return;
                Debug.Log("10");
                Button button = buttonTransform.gameObject.GetComponent<Button>();
                if (button == null) return;
                Debug.Log("11");
                var buttonLabelTransform = buttonTransform.Find("ButtonLabel");
                if (buttonLabelTransform == null) return;
                Debug.Log("12");
                TMP_Text buttonLabel = buttonLabelTransform.gameObject.GetComponent<TMP_Text>();
                if (buttonLabel == null) return;

                Debug.Log("13");
                Debug.Log("Price: " + listing.price);
                var price = Web3.Convert.FromWei(System.Numerics.BigInteger.Parse(listing.price));
                Debug.Log("Price Converted: " + price);
                priceValue.text = listing.inUSD ? price.ToString() + " USD" : price.ToString();

                if (listing.isAuction)
                {
                    Debug.Log("Auction");
                    priceLabel.text = "Minimum Bid";
                    typeValue.text = "Auction";
                    buttonLabel.text = "Bid";
                    button.onClick.AddListener(delegate
                    {
                        BidLand(listing.id);
                    });
                }
                else
                {
                    Debug.Log("Direct");
                    priceLabel.text = "Price";
                    typeValue.text = "Direct";
                    buttonLabel.text = "Buy";
                    button.onClick.AddListener(delegate
                    {
                        BuyLand(listing.id);
                    });
                }
            }
            i++;
        }
        if (height != 0)
        {
            var rectListingsContainer = listingsContainer.GetComponent<RectTransform>().rect;
            rectListingsContainer.height = height;
        }

    }

    async void BuyLand(int listingId)
    {
        listingHandlePanel.SetActive(true);
        bidInput.gameObject.SetActive(false);
        string price = await ContractManager.Instance.GetPrice(listingId);
        priceText.text = "Price: " + Web3.Convert.FromWei(System.Numerics.BigInteger.Parse(price));
        confirmListingButton.onClick.AddListener(delegate
        {
            ConfirmBuy(listingId, price);
        });
    }

    async void ConfirmBuy(int listingId, string price)
    {
        bool success = await ContractManager.Instance.BuyListing(listingId, price);
        OnCancelListingHandle();
        ResetListingPanel();
        if (success)
        {
            ToggleMarketplace();
        }
        ContractManager.Instance.OnSwitchNetwork();
    }
    async void BidLand(int listingId)
    {
        listingHandlePanel.SetActive(true);
        bidInput.gameObject.SetActive(true);
        string highestBid = await ContractManager.Instance.GetHighestBid(listingId);
        priceText.text = "Highest Bid: " + Web3.Convert.FromWei(System.Numerics.BigInteger.Parse(highestBid));
        confirmListingButton.onClick.AddListener(delegate
        {
            ConfirmBid(listingId, highestBid);
        });
    }
    async void ConfirmBid(int listingId, string highestBid)
    {
        bool success = await ContractManager.Instance.BidListing(listingId, highestBid, bidInput.text);
        OnCancelListingHandle();
        ResetListingPanel();
        if (success)
        {
            ToggleMarketplace();
        }
        ContractManager.Instance.OnSwitchNetwork();
    }

    public void OnCancelListingHandle()
    {
        listingHandlePanel.SetActive(false);
        bidInput.text = "";
        bidInput.gameObject.SetActive(false);
        confirmListingButton.onClick.RemoveAllListeners();
    }

    async void OnCreateListing()
    {
        if (priceInput.text == "" || Convert.ToDouble(priceInput.text) <= 0)
        {
            ShowError("Invalid " + priceInputLabel.text);
            return;
        }
        if (typeDropdown.value == 1)
        {
            if (durationInput.text == "" || Convert.ToDouble(durationInput.text) <= 0)
            {
                ShowError("Invalid Duration");
                return;
            }

            if (linkAmountInput.text == "" || Convert.ToDouble(linkAmountInput.text) <= 0)
            {
                ShowError("Invalid Amount of Link");
                return;
            }
        }
        marketplaceLoadingPanel.SetActive(true);
        Debug.Log("TRYING...");
        Debug.Log("Details1: " + usdToggle.isOn);
        Debug.Log("Details2: " + priceInput.text);
        Debug.Log("Details3: " + (typeDropdown.value == 1).ToString());
        Debug.Log("Details4: " + durationInput.text);
        Debug.Log("Details5: " + linkAmountInput.text);
        // await ContractManager.Instance.CreateListing(usdToggle.isOn, xIndex, yIndex, priceInput.text, typeDropdown.value == 1, durationInput.text, linkAmountInput.text);
        if (typeDropdown.value == 1)
        {
            await ContractManager.Instance.CreateListing(usdToggle.isOn, xIndex, yIndex, priceInput.text, typeDropdown.value == 1, durationInput.text, linkAmountInput.text);
        }
        else
        {
            await ContractManager.Instance.CreateListing(usdToggle.isOn, xIndex, yIndex, priceInput.text, typeDropdown.value == 1, "0", "0");
        }
        // await ContractManager.Instance.CreateListing(usdToggle.isOn, 0, 0, priceInput.text, typeDropdown.value == 1, durationInput.text, linkAmountInput.text);
        OnCancelListing();
        ToggleMarketplace();
    }

    void OnCancelListing()
    {
        marketplacePanel.SetActive(false);
        marketplaceLoadingPanel.SetActive(false);
        ResetMarketplacePanel();
    }

    void ResetMarketplacePanel()
    {
        isError = false;
        typeDropdown.value = 0;
        priceInput.text = "";
        usdToggle.isOn = false;
        durationInput.text = "";
        linkAmountInput.text = "";
    }

    public void ResetListingPanel()
    {
        listingsPanel.SetActive(false);
        ResetListingPrefabsList();
    }

    void ResetHighlightPrefabsList()
    {
        foreach (var item in highlightPrefabs)
        {
            Destroy(item);
        }
        highlightPrefabs.Clear();
        highlightPrefabs = new List<GameObject>();
    }

    void ResetListingPrefabsList()
    {
        foreach (var item in listingPrefabs)
        {
            Destroy(item);
        }
        listingPrefabs.Clear();
        listingPrefabs = new List<GameObject>();
    }

    public void AttachClickListeners()
    {
        editButton.onClick.AddListener(OnEditClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    public void RemoveClickListeners()
    {
        uiController.RemoveEventListeners();
        editButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }

    void OnEditClicked()
    {
        uiController.AddEventListeners();
        buildingMenuRect.sizeDelta = new Vector2(180f, 240f);
        editButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        landDesignSaveButton.gameObject.SetActive(true);
    }

    void OnCancelClicked()
    {
        uiController.RemoveEventListeners();
        buildingMenuRect.sizeDelta = new Vector2(180f, 210f);
        editButton.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        landDesignSaveButton.gameObject.SetActive(false);
        ContractManager.Instance.CancelClicked();
    }

    async void OnConfirmClicked()
    {
        Debug.Log("updating...");
        await ContractManager.Instance.confirmMapUpdates();
        Debug.Log("updated!!!");
        OnCancelClicked();
    }

    public async void ToggleMarketplace()
    {
        if (!isMarketplaceOpen)
        {
            bool isSuccess = await ContractManager.Instance.SetMarketplaceData();
            if (!isSuccess)
            {
                return;
            }
        }
        isMarketplaceOpen = !isMarketplaceOpen;
        buildingMenu.SetActive(!isMarketplaceOpen);
        faucetButton.SetActive(!isMarketplaceOpen);
        marketplaceButtonText.text = !isMarketplaceOpen ? "Marketplace" : "Go Back";
        if (!isMarketplaceOpen)
        {
            if (highlightPrefabs.Count != 0)
            {
                ResetHighlightPrefabsList();
            }
            ContractManager.Instance.ResetListingHighlights();
        }
    }

    public void ToggleLandDesignSave()
    {
        isLandDesignSaveOpen = !isLandDesignSaveOpen;
        buildingMenu.SetActive(!isLandDesignSaveOpen);
        faucetButton.SetActive(!isLandDesignSaveOpen);
        marketplaceButton.gameObject.SetActive(!isLandDesignSaveOpen);
        landDesignSaveButtonText.text = !isLandDesignSaveOpen ? "Save/Load Design" : "Go Back";
        if (!isLandDesignSaveOpen)
        {
            if (highlightPrefabs.Count != 0)
            {
                ResetHighlightPrefabsList();
            }
        }
    }

    public void ToggleLandDesignSaveSelect()
    {
        isLandDesignSaveSelectOpen = !isLandDesignSaveSelectOpen;
    }
}
