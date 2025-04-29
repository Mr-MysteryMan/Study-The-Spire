using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public enum ShopMode {
    normal,
    buy,
    delete,
}

public class ShopPanelManager : MonoBehaviour {
    private CardManager cardManager;
    private Transform topCenter;
    private Transform buyBtn;
    private Transform deleteBtn;
    private Transform closeBtn;

    private Transform buyIcon1, buyIcon2, buySelect;
    private Transform deleteIcon1, deleteIcon2, deleteSelect;

    private Transform centerPanel;
    private Transform scrollViewContent;
    private Transform detailPanel;

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

    public ShopMode curMode = ShopMode.normal;

    void Start() {
        cardManager = CardManager.Instance;
        CacheUI();
        InitClick();
        RefreshUI();
    }

    private void CacheUI() {
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

        centerPanel = root.Find("Center");
        scrollViewContent = centerPanel.Find("Scroll View/Viewport/Content");
        detailPanel = centerPanel.Find("DetailPanel");

        bottomPanel = root.Find("Bottom");
        bottomMenus = bottomPanel.Find("BottomMenus");
        // detailBtn = bottomMenus.Find("DetailBtn");

        deletePanel = bottomPanel.Find("DeletePanel");
        confirmBtn = deletePanel.Find("ConfirmBtn");
        infoText = deletePanel.Find("InfoText");
        backBtn = deletePanel.Find("Back");

        confirmPopup = root.Find("ConfirmPopup");
        popupConfirmBtn = confirmPopup.Find("ConfirmBtn");
        popupCancelBtn = confirmPopup.Find("CancelBtn");

        // 默认隐藏Bottom,Detail,ConfirmPopup面板
        bottomPanel.gameObject.SetActive(false);
        detailPanel.gameObject.SetActive(false);
        confirmPopup.gameObject.SetActive(false);
    }

    private void InitClick() {
        buyBtn.GetComponent<Button>().onClick.AddListener(OnClickBuy);
        deleteBtn.GetComponent<Button>().onClick.AddListener(OnClickDelete);
        closeBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        confirmBtn.GetComponent<Button>().onClick.AddListener(OnConfirm);
        // detailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);
        backBtn.GetComponent<Button>().onClick.AddListener(OnBack);
    }

    private void OnClickBuy() {
        curMode = ShopMode.buy;
        RefreshUI();
    }

    private void OnClickDelete() {
        curMode = ShopMode.delete;
        RefreshUI();
    }

    private void OnClickClose() {
        gameObject.SetActive(false);
        curMode = ShopMode.normal;
    }

    private void OnConfirm() {
        if (selectedItemData == null) {
            Debug.LogWarning("未选中物品");
            return;
        }

        confirmPopup.gameObject.SetActive(true);
        Text popupContent = confirmPopup.Find("Content").GetComponent<Text>();

        if (curMode == ShopMode.buy) {
            popupContent.text = $"是否花费 {selectedItemData.gold} 金币购买选中卡牌？";
            popupConfirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            popupConfirmBtn.GetComponent<Button>().onClick.AddListener(DoBuyItem);
        } else if (curMode == ShopMode.delete) {
            popupContent.text = $"是否花费 100 金币移除选中卡牌？";
            popupConfirmBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            popupConfirmBtn.GetComponent<Button>().onClick.AddListener(DoDeleteItem);
        }

        popupCancelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        popupCancelBtn.GetComponent<Button>().onClick.AddListener(() => {
            confirmPopup.gameObject.SetActive(false);
        });
    }

    private void DoBuyItem() {
        cardManager.SpendGold(selectedItemData.gold);
        cardManager.AddCard(selectedItemData);

        // 从 shopItems 中移除该商品
        shopItems.Remove(selectedItemData);
        // SaveItemDataToJson("ItemData/shop_items", shopItems);

        Destroy(selectedItemGO);
        selectedItemGO = null;
        selectedItemData = null;

        confirmPopup.gameObject.SetActive(false);
        detailPanel.gameObject.SetActive(false);
    }

    private void DoDeleteItem() {
        // 与CardManager交互
        const int deleteCost = 100;
        if (cardManager.SpendGold(deleteCost)) {
            cardManager.RemoveCard(selectedItemData.cardId);
            Destroy(selectedItemGO);
            selectedItemGO = null;
            selectedItemData = null;
        }

        confirmPopup.gameObject.SetActive(false);
        detailPanel.gameObject.SetActive(false);
    }

    private void SaveItemDataToJson(string path, List<ItemData> items) {
        string json = JsonConvert.SerializeObject(items, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.dataPath, "Resources", path + ".json"), json);
    }


    private void OnBack() {
        curMode = ShopMode.normal;
        RefreshUI();
    }

    private void RefreshUI() {

        if (curMode == ShopMode.normal) {
            buyIcon1.gameObject.SetActive(true);
            buyIcon2.gameObject.SetActive(false);
            buySelect.gameObject.SetActive(false);

            deleteIcon1.gameObject.SetActive(true);
            deleteIcon2.gameObject.SetActive(false);
            deleteSelect.gameObject.SetActive(false);

            bottomPanel.gameObject.SetActive(false);

        } else if (curMode == ShopMode.buy) {
            buyIcon1.gameObject.SetActive(false);
            buyIcon2.gameObject.SetActive(true);
            buySelect.gameObject.SetActive(true);

            deleteIcon1.gameObject.SetActive(true);
            deleteIcon2.gameObject.SetActive(false);
            deleteSelect.gameObject.SetActive(false);

            bottomPanel.gameObject.SetActive(true);
        } else if (curMode == ShopMode.delete) {
            buyIcon1.gameObject.SetActive(true);
            buyIcon2.gameObject.SetActive(false);
            buySelect.gameObject.SetActive(false);

            deleteIcon1.gameObject.SetActive(false);
            deleteIcon2.gameObject.SetActive(true);
            deleteSelect.gameObject.SetActive(true);

            bottomPanel.gameObject.SetActive(true);
        }

        // 清空 Scroll View 内容
        foreach (Transform child in scrollViewContent) {
            Destroy(child.gameObject);
        }
        selectedItemData = null;
        selectedItemGO = null;
        detailPanel.gameObject.SetActive(false);
        confirmPopup.gameObject.SetActive(false);

        // 加载内容
        if (curMode == ShopMode.buy) {
            LoadShopItems();
        } else if (curMode == ShopMode.delete) {
            LoadBackItems();
        }
    }

    public GameObject ShopUIItemPrefab;

    private List<ItemData> shopItems = new List<ItemData>();

    private List<ItemData> LoadItemDataFromJson(string resourcePath) {
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        if (jsonFile == null) {
            Debug.LogError($"找不到资源路径：Resources/{resourcePath}.json");
            return new List<ItemData>();
        }

        return JsonConvert.DeserializeObject<List<ItemData>>(jsonFile.text);
    }
    
    private GameObject selectedItemGO = null;
    private ItemData selectedItemData = null;

    private void LoadShopItems() {
        Debug.Log("加载商店中的商品...");
        // shopItems = LoadItemDataFromJson("ItemData/shop_items");

        foreach (ItemData item in shopItems) {
            GameObject itemGO = Instantiate(ShopUIItemPrefab, scrollViewContent);

            // 设置图标
            Image icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            Sprite sprite = CardUI.GetCardBackground(item.cardType);
            if (sprite) icon.sprite = sprite;

            // 设置名称
            Text nameText = itemGO.transform.Find("Bottom/NameText").GetComponent<Text>();
            if (curMode != ShopMode.buy) {
                nameText.text = item.cardValue.ToString();
            }
            // Debug.Log("加载了：" + item.name);

            // 设置金币区域
            Transform golds = itemGO.transform.Find("Bottom/Golds");
            golds.gameObject.SetActive(curMode == ShopMode.buy);
            Text goldAmount = golds.Find("AmountText").GetComponent<Text>();
            goldAmount.text = item.gold.ToString();

            // 默认未选中
            itemGO.transform.Find("Select").gameObject.SetActive(false);

            // 设置选中按钮
            Button btn = itemGO.GetComponent<Button>();
            if (btn != null) {
                btn.onClick.AddListener(() => {
                    OnItemClicked(itemGO, item);
                });
            }
        }
    }

    private List<CardData> backItems = new List<CardData>();
    private void LoadBackItems() {
        Debug.Log("加载背包中的物品...");
        backItems = cardManager.GetAllCards();

        foreach (CardData item in backItems) {
            GameObject itemGO = Instantiate(ShopUIItemPrefab, scrollViewContent);

            // 设置图标
            Image icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            Sprite sprite = CardUI.GetCardBackground(item.cardType);
            if (sprite) icon.sprite = sprite;

            // 设置名称
            Text nameText = itemGO.transform.Find("Bottom/NameText").GetComponent<Text>();
            if (curMode != ShopMode.buy) {
                nameText.text = item.cardValue.ToString();
            }
            // Debug.Log("加载了：" + item.name);

            // 设置金币区域
            Transform golds = itemGO.transform.Find("Bottom/Golds");
            golds.gameObject.SetActive(false);

            // 默认未选中
            itemGO.transform.Find("Select").gameObject.SetActive(false);

            // 设置选中按钮
            Button btn = itemGO.GetComponent<Button>();
            if (btn != null) {
                btn.onClick.AddListener(() => {
                    OnItemClicked(itemGO, new ItemData(item.cardType, item.cardValue, 0));
                });
            }
        }
    }


    private void OnItemClicked(GameObject itemGO, ItemData itemData) {
        if (selectedItemGO != null) {
            selectedItemGO.transform.Find("Select").gameObject.SetActive(false);
        }

        // 激活当前选中
        selectedItemGO = itemGO;
        selectedItemData = itemData;
        selectedItemGO.transform.Find("Select").gameObject.SetActive(true);

        // 显示详情面板
        ShowItemDetail(itemData);

        Text text = bottomPanel.Find("DeletePanel/ConfirmBtn/Text").GetComponent<Text>();
        if (curMode == ShopMode.buy) {
            text.text = itemData.gold.ToString();
        } else if (curMode == ShopMode.delete) {
            text.text = "100";
        }
    }

    private void ShowItemDetail(ItemData item) {
        detailPanel.gameObject.SetActive(true);

        Text nameText = detailPanel.Find("Top/Title").GetComponent<Text>();
        Image iconImage = detailPanel.Find("Center/Icon").GetComponent<Image>();
        Text descText = detailPanel.Find("Bottom/Description").GetComponent<Text>();

        // nameText.text = item.name;
        // descText.text = item.description;

        Sprite icon = CardUI.GetCardBackground(item.cardType);
        if (icon) iconImage.sprite = icon;
    }
}
