using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public enum ShopMode {
    normal,
    buy,
    delete,
}

public class ShopPanelManager : MonoBehaviour {
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

    public ShopMode curMode = ShopMode.normal;

    void Start() {
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
        detailBtn = bottomMenus.Find("DetailBtn");

        deletePanel = bottomPanel.Find("DeletePanel");
        confirmBtn = deletePanel.Find("ConfirmBtn");
        infoText = deletePanel.Find("InfoText");
        backBtn = deletePanel.Find("Back");

        // 默认隐藏Bottom面板
        bottomPanel.gameObject.SetActive(false);
    }

    private void InitClick() {
        buyBtn.GetComponent<Button>().onClick.AddListener(OnClickBuy);
        deleteBtn.GetComponent<Button>().onClick.AddListener(OnClickDelete);
        closeBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        confirmBtn.GetComponent<Button>().onClick.AddListener(OnConfirm);
        detailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);
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
        RefreshUI();
    }

    private void OnConfirm() {
        if (curMode == ShopMode.buy) {
            Debug.Log("执行购买逻辑");
        } else if (curMode == ShopMode.delete) {
            Debug.Log("执行删除逻辑");
        }
    }

    private void OnDetail() {
        Debug.Log("点击查看详情");
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

        // 加载内容
        if (curMode == ShopMode.buy) {
            LoadShopItems();
        } else if (curMode == ShopMode.delete) {
            LoadDeleteItems();
        }
}


    private void LoadShopItems() {
        Debug.Log("加载商店中的商品...");
        // 示例：动态生成UI
        // Instantiate(shopItemPrefab, scrollViewContent);
    }

    private void LoadDeleteItems() {
        Debug.Log("加载背包中的可删除卡牌...");
    }
}
