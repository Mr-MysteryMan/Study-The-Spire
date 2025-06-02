using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Cards.CardDatas;

public enum ShopMode
{
    normal,
    buy,
    delete,
}

public class ShopPanelManager : MonoBehaviour
{
    private CardManager cardManager;
    private Transform topCenter;
    private Transform buyBtn;
    private Transform deleteBtn;
    private Transform closeBtn;

    private Transform buyIcon1, buyIcon2, buySelect;
    private Transform deleteIcon1, deleteIcon2, deleteSelect;
    
    private Transform searchInput;

    private Transform centerPanel;
    private Transform scrollViewContent;

    private GameObject detailPanel;
    private Text detailCostText, detailNameText, detailDescText;
    private Image detailIconImage;

    private Transform bottomPanel;
    private Transform bottomMenus;
    private Transform detailBtn;

    private Transform deletePanel;
    private Transform confirmBtn;
    private Transform infoText;
    private Transform backBtn;

    private Transform confirmPopup;
    private Transform popupConfirmBtn;
    private Transform popupCancelBtn;
    private Transform goldInsufficientPopup;
    private Transform goldTextParent;
    
    public ObjectEventSO loadMapEvent;
    public GameObject DetailPanel;  

    public ShopMode curMode = ShopMode.normal;
    public GameObject ShopUIItemPrefab;
    public GameObject goldFloatTextPrefab;
    public CardSearchBar cardSearchBar;

    private GameObject selectedItemGO = null;
    private ItemData selectedItemData = null;

    private List<ICardData> backCards = new List<ICardData>();

    void Start()
    {
        cardManager = CardManager.Instance;
        backCards = cardManager.GetAllCards();
        GenerateRandomShopItems();
        CacheUI();
        InitClick();
        RefreshUI();
    }

    private void CacheUI()
    {
        var root = transform;

        topCenter = root.Find("TopCenter");

        buyBtn = topCenter.Find("Menu/Buy");
        buyIcon1 = buyBtn.Find("icon1");
        buyIcon2 = buyBtn.Find("icon2");
        buySelect = buyBtn.Find("Select");

        deleteBtn = topCenter.Find("Menu/Delete");
        deleteIcon1 = deleteBtn.Find("icon1");
        deleteIcon2 = deleteBtn.Find("icon2");
        deleteSelect = deleteBtn.Find("Select");

        closeBtn = root.Find("RightTop/Close");

        searchInput = root.Find("BetweenTopAndCenter/SearchInput");

        centerPanel = root.Find("Center");
        scrollViewContent = centerPanel.Find("Scroll View/Viewport/Content");

        detailPanel = Instantiate(DetailPanel, centerPanel);
        detailCostText = detailPanel.transform.Find("Center/Cost/Text").GetComponent<Text>();
        detailNameText = detailPanel.transform.Find("Top/Title").GetComponent<Text>();
        detailIconImage = detailPanel.transform.Find("Center/Icon").GetComponent<Image>();
        detailDescText = detailPanel.transform.Find("Bottom/Description").GetComponent<Text>();

        bottomPanel = root.Find("Bottom");
        bottomMenus = bottomPanel.Find("BottomMenus");
        bottomMenus.Find("ShowGold/Text").GetComponent<Text>().text = $"Gold {cardManager.gold}";
        goldTextParent = bottomMenus.Find("ShowGold");
        // detailBtn = bottomMenus.Find("DetailBtn");

        deletePanel = bottomPanel.Find("DeletePanel");
        confirmBtn = deletePanel.Find("ConfirmBtn");
        infoText = deletePanel.Find("InfoText");
        backBtn = deletePanel.Find("Back");

        confirmPopup = root.Find("ConfirmPopup");
        popupConfirmBtn = confirmPopup.Find("ConfirmBtn");
        popupCancelBtn = confirmPopup.Find("CancelBtn");

        goldInsufficientPopup = root.Find("GoldInsufficientPopup");

        // 默认隐藏deletePanel,ConfirmPopup,GoldInsufficientPopup面板
        deletePanel.gameObject.SetActive(false);
        confirmPopup.gameObject.SetActive(false);
        goldInsufficientPopup.gameObject.SetActive(false);
    }

    private void InitClick()
    {
        buyBtn.GetComponent<Button>().onClick.AddListener(OnClickBuy);
        deleteBtn.GetComponent<Button>().onClick.AddListener(OnClickDelete);
        closeBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        confirmBtn.GetComponent<Button>().onClick.AddListener(OnConfirm);
        backBtn.GetComponent<Button>().onClick.AddListener(OnBack);
        cardSearchBar.OnSearchKeywordChanged += OnSearchChanged;
    }

    private void OnClickBuy()
    {
        curMode = ShopMode.buy;
        RefreshUI();
        LoadShopItems(shopItems);
    }

    private void OnClickDelete()
    {
        curMode = ShopMode.delete;
        RefreshUI();
        LoadBackItems(backCards);
    }

    private void OnClickClose()
    {
//        gameObject.SetActive(false);
        curMode = ShopMode.normal;
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnConfirm()
    {
        if (selectedItemData == null)
        {
            Debug.LogWarning("未选中物品");
            return;
        }

        confirmPopup.gameObject.SetActive(true);
        Text popupContent = confirmPopup.Find("Content").GetComponent<Text>();

        if (curMode == ShopMode.buy)
        {
            popupContent.text = $"是否花费 {selectedItemData.gold} 金币购买选中卡牌？";
            popupConfirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            popupConfirmBtn.GetComponent<Button>().onClick.AddListener(DoBuyItem);
        }
        else if (curMode == ShopMode.delete)
        {
            popupContent.text = $"是否花费 100 金币移除选中卡牌？";
            popupConfirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            popupConfirmBtn.GetComponent<Button>().onClick.AddListener(DoDeleteItem);
        }

        popupCancelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        popupCancelBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            confirmPopup.gameObject.SetActive(false);
        });
    }

    // 自动关闭弹窗的协程
    private IEnumerator AutoHidePopup(Transform popup, float delay) {
        yield return new WaitForSeconds(delay);
        popup.gameObject.SetActive(false);
    }

    private void UpdateGoldDisplayWithEffect(int amount) {
        // 更新显示文本
        bottomMenus.Find("ShowGold/Text").GetComponent<Text>().text = $"Gold {cardManager.gold}";

        // 创建浮动文字动画
        if (goldFloatTextPrefab != null && goldTextParent != null) {
            GameObject go = Instantiate(goldFloatTextPrefab, goldTextParent);
            go.transform.localPosition = Vector3.zero; // 居中显示
            go.GetComponent<GoldFloatText>().SetAmount(amount);
        }
    }

    private void DoBuyItem() {
        if (cardManager.SpendGold(selectedItemData.gold)) {
            UpdateGoldDisplayWithEffect(selectedItemData.gold);
            cardManager.AddCard(selectedItemData.cardData);

            shopItems.Remove(selectedItemData);
            // SaveItemDataToJson("ItemData/shop_items", shopItems);

            Destroy(selectedItemGO);
            selectedItemGO = null;
            selectedItemData = null;
        } else {
            goldInsufficientPopup.gameObject.SetActive(true);
            StartCoroutine(AutoHidePopup(goldInsufficientPopup, 1f)); // 1秒后自动关闭
        }

        confirmPopup.gameObject.SetActive(false);
    }

    private void DoDeleteItem() {
        // TODO 设置删除金币
        const int deleteCost = 100;
        if (cardManager.SpendGold(deleteCost)) {
            UpdateGoldDisplayWithEffect(deleteCost);
            cardManager.RemoveCard(selectedItemData.cardData);
            Destroy(selectedItemGO);
            selectedItemGO = null;
            selectedItemData = null;
        } else {
            goldInsufficientPopup.gameObject.SetActive(true);
            StartCoroutine(AutoHidePopup(goldInsufficientPopup, 1f));
        }

        confirmPopup.gameObject.SetActive(false);
    }

    private void OnBack()
    {
        curMode = ShopMode.normal;
        RefreshUI();
    }

    private void RefreshUI()
    {

        if (curMode == ShopMode.normal)
        {
            buyIcon1.gameObject.SetActive(true);
            buyIcon2.gameObject.SetActive(false);
            buySelect.gameObject.SetActive(false);

            deleteIcon1.gameObject.SetActive(true);
            deleteIcon2.gameObject.SetActive(false);
            deleteSelect.gameObject.SetActive(false);

            deletePanel.gameObject.SetActive(false);
            bottomMenus.gameObject.SetActive(true);

            cardSearchBar.gameObject.SetActive(false);
        }
        else if (curMode == ShopMode.buy)
        {
            buyIcon1.gameObject.SetActive(false);
            buyIcon2.gameObject.SetActive(true);
            buySelect.gameObject.SetActive(true);

            deleteIcon1.gameObject.SetActive(true);
            deleteIcon2.gameObject.SetActive(false);
            deleteSelect.gameObject.SetActive(false);

            deletePanel.gameObject.SetActive(true);
            bottomMenus.gameObject.SetActive(true);

            cardSearchBar.gameObject.SetActive(true);
        } else if (curMode == ShopMode.delete) {
            buyIcon1.gameObject.SetActive(true);
            buyIcon2.gameObject.SetActive(false);
            buySelect.gameObject.SetActive(false);

            deleteIcon1.gameObject.SetActive(false);
            deleteIcon2.gameObject.SetActive(true);
            deleteSelect.gameObject.SetActive(true);

            deletePanel.gameObject.SetActive(true);
            bottomMenus.gameObject.SetActive(true);

            cardSearchBar.gameObject.SetActive(true);
        }

        // 清空 Scroll View 内容
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
        selectedItemData = null;
        selectedItemGO = null;
        confirmPopup.gameObject.SetActive(false);
    }

    private List<ItemData> shopItems = new List<ItemData>();
    private void LoadShopItems(List<ItemData> cards)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
        
        Debug.Log("加载商店中的商品...");
        // shopItems = LoadItemDataFromJson("ItemData/shop_items");

        foreach (ItemData item in cards)
        {
            GameObject itemGO = Instantiate(ShopUIItemPrefab, scrollViewContent);

            // 设置图标
            Image icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            Sprite sprite = item.cardData.Sprite;
            if (sprite) icon.sprite = sprite;

            // 设置名称
            Text nameText = itemGO.transform.Find("Bottom/NameText").GetComponent<Text>();
            if (curMode != ShopMode.buy)
            {
                nameText.text = item.cardData.CardName;
            }
            // Debug.Log("加载了：" + item.name);

            // 设置消耗能量点
            Text costText = itemGO.transform.Find("Top/Cost/Text").GetComponent<Text>();
            costText.text = item.cardData.Cost.ToString();

            // 设置金币区域
            Transform golds = itemGO.transform.Find("Bottom/Golds");
            golds.gameObject.SetActive(curMode == ShopMode.buy);
            Text goldAmount = golds.Find("AmountText").GetComponent<Text>();
            goldAmount.text = item.gold.ToString();

            // 默认未选中
            itemGO.transform.Find("Select").gameObject.SetActive(false);

            // 设置选中按钮
            Button btn = itemGO.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() =>
                {
                    OnItemClicked(itemGO, item);
                });
            }
        }
    }

    private void LoadBackItems(List<ICardData> cards)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("加载背包中的物品...");

        foreach (ICardData item in cards)
        {
            GameObject itemGO = Instantiate(ShopUIItemPrefab, scrollViewContent);

            // 设置图标
            Image icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            Sprite sprite = item.Sprite;
            if (sprite) icon.sprite = sprite;

            // 设置名称
            Text nameText = itemGO.transform.Find("Bottom/NameText").GetComponent<Text>();
            if (curMode != ShopMode.buy)
            {
                nameText.text = item.CardName;
            }
            // Debug.Log("加载了：" + item.name);

            // 设置消耗能量点
            Text costText = itemGO.transform.Find("Top/Cost/Text").GetComponent<Text>();
            costText.text = item.Cost.ToString();

            // 设置金币区域
            Transform golds = itemGO.transform.Find("Bottom/Golds");
            golds.gameObject.SetActive(false);

            // 默认未选中
            itemGO.transform.Find("Select").gameObject.SetActive(false);

            // 设置选中按钮
            Button btn = itemGO.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() =>
                {
                    OnItemClicked(itemGO, new ItemData(0, item));
                });
            }
        }
    }


    private void OnItemClicked(GameObject itemGO, ItemData itemData)
    {
        if (selectedItemGO != null)
        {
            selectedItemGO.transform.Find("Select").gameObject.SetActive(false);
        }

        // 激活当前选中
        selectedItemGO = itemGO;
        selectedItemData = itemData;
        selectedItemGO.transform.Find("Select").gameObject.SetActive(true);

        // 显示详情面板
        ShowItemDetail(itemData);

        Text text = bottomPanel.Find("DeletePanel/ConfirmBtn/Text").GetComponent<Text>();
        if (curMode == ShopMode.buy)
        {
            text.text = itemData.gold.ToString();
        }
        else if (curMode == ShopMode.delete)
        {
            text.text = "100";
        }
    }

    private void ShowItemDetail(ItemData item)
    {
        detailCostText.text = item.cardData.Cost.ToString();
        detailNameText.text = item.cardData.CardName;
        detailIconImage.sprite = item.cardData.Sprite;
        detailDescText.text = item.cardData.Desc;

        detailPanel.SetActive(true);
    }

    private void GenerateRandomShopItems(int count = 5)
    {
        shopItems.Clear();
        for (int i = 0; i < count; i++)
        {
            CardType type = (CardType)Random.Range(0, 3); // Attack, Defence, Heal
            int value = Random.Range(1, 10);
            int gold = Random.Range(50, 200);
            int cost = Random.Range(1, 5);
            shopItems.Add(new ItemData(gold, value, cost, type));
        }
    }

    private void OnSearchChanged(string keyword)
    {
        RefreshUI();
        if (curMode == ShopMode.buy) {
            LoadShopItems(CardFilterUtils.FilterItems(keyword, shopItems));
        } else if (curMode == ShopMode.delete) {
            LoadBackItems(CardFilterUtils.FilterCards(keyword, backCards));
        }
    }

}
