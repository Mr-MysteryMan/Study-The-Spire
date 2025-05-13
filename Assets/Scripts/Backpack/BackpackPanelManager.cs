using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public enum BackpackMode
{
    normal,
    card,
}

public class BackpackPanelManager : MonoBehaviour
{
    private CardManager cardManager;
    private Transform topCenter;
    private Transform cardBtn;
    private Transform cardIcon1, cardIcon2, cardSelect;
    private Transform closeBtn;
    private Transform searchInput;
    private Transform centerPanel;
    private Transform scrollViewContent;
    private Transform detailPanel;
    private Transform bottomPanel;
    private Transform bottomMenus;
    private Transform detailBtn;
    private Transform goldAmount;
    public ObjectEventSO loadMapEvent;
    public GameObject BackpackUIItem;
    public BackpackMode curMode = BackpackMode.normal;

    private GameObject selectedCardGO = null;
    private ICardData selectedCard = null;

    void Start()
    {
        cardManager = CardManager.Instance;
        CacheUI();
        InitClick();
        RefreshUI();
    }

    private void CacheUI()
    {
        var root = transform;

        topCenter = root.Find("TopCenter");
        cardBtn = topCenter.Find("Menu/Card");
        cardIcon1 = cardBtn.Find("icon1");
        cardIcon2 = cardBtn.Find("icon2");
        cardSelect = cardBtn.Find("Select");

        closeBtn = root.Find("RightTop/Close");
        
        searchInput = root.Find("BetweenTopAndCenter/SearchInput");

        centerPanel = root.Find("Center");
        scrollViewContent = centerPanel.Find("Scroll View/Viewport/Content");
        detailPanel = centerPanel.Find("DetailPanel");

        bottomPanel = root.Find("Bottom");
        bottomMenus = bottomPanel.Find("BottomMenus");
        detailBtn = bottomMenus.Find("DetailBtn");
        goldAmount = bottomMenus.Find("Gold/amount");

        detailPanel.gameObject.SetActive(false);
    }

    private void InitClick()
    {
        cardBtn.GetComponent<Button>().onClick.AddListener(() => ShowCards());
        closeBtn.GetComponent<Button>().onClick.AddListener(() => loadMapEvent.RaiseEvent(null, this));
        detailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);
        goldAmount.GetComponent<Text>().text = $"{cardManager.Gold}";
        searchInput.GetComponent<InputField>().onValueChanged.AddListener(OnSearchChanged);
    }

    private void ShowCards()
    {
        curMode = BackpackMode.card;
        RefreshUI();
        ShowCardList();
    }

    private void RefreshUI()
    {
        cardIcon1.gameObject.SetActive(curMode != BackpackMode.card);
        cardIcon2.gameObject.SetActive(curMode == BackpackMode.card);
        cardSelect.gameObject.SetActive(curMode == BackpackMode.card);
        searchInput.gameObject.SetActive(curMode != BackpackMode.normal);
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowCardList()
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        List<ICardData> cards = cardManager.GetAllCards().ToList();

        // 根据搜索关键字过滤
        if (!string.IsNullOrEmpty(currentSearchKeyword))
        {
            cards = cards.Where( c => 
                c.CardName.ToLower().Contains(currentSearchKeyword) ||
                c.CardCategory.ToString().ToLower().Contains(currentSearchKeyword)
            ).ToList();
        }

        foreach (var card in cards)
        {
            GameObject itemGO = Instantiate(BackpackUIItem, scrollViewContent);

            var icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            icon.sprite = card.Sprite;

            var text = itemGO.transform.Find("Bottom/Text").GetComponent<Text>();
            text.text = card.CardName;

            itemGO.transform.Find("Select").gameObject.SetActive(false);

            var btn = itemGO.GetComponent<Button>();
            btn.onClick.AddListener(() => OnCardClicked(itemGO, card));
        }

        Debug.Log($"展示卡牌，共 {cards.Count} 张");
    }

    private void OnCardClicked(GameObject itemGO, ICardData card)
    {
        if (selectedCardGO != null)
        {
            selectedCardGO.transform.Find("Select").gameObject.SetActive(false);
        }

        selectedCardGO = itemGO;
        selectedCard = card;
        selectedCardGO.transform.Find("Select").gameObject.SetActive(true);
    }

    private void OnDetail()
    {
        if (selectedCard == null) return;

        detailPanel.gameObject.SetActive(true);

        var nameText = detailPanel.Find("Top/Title").GetComponent<Text>();
        var iconImage = detailPanel.Find("Center/Icon").GetComponent<Image>();
        var descText = detailPanel.Find("Bottom/Description").GetComponent<Text>();

        nameText.text = selectedCard.CardName;
        iconImage.sprite = selectedCard.Sprite;
        descText.text = selectedCard.Desc;
    }

    private String currentSearchKeyword = "";
    private void OnSearchChanged(string keyword)
    {
        currentSearchKeyword = keyword.Trim().ToLower();
        ShowCardList();
    }
}