using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum BackpackMode
{
    normal,
    card,
}

public class BackpackPanelManager : MonoBehaviour
{
    public GameObject BackpackUIItem;
    public GameObject DetailPanel;
    public CardSearchBar cardSearchBar;
    public ObjectEventSO loadMapEvent;

    private CardManager cardManager;
    private Transform cardBtn, cardIcon1, cardIcon2, cardSelect;
    private Transform closeBtn, scrollViewContent;
    private Transform detailBtn, goldAmount;
    private GameObject detailPanel;
    private Text detailCostText, detailNameText, detailDescText;
    private Image detailIconImage;

    private GameObject selectedCardGO;
    private ICardData selectedCard;

    public BackpackMode curMode = BackpackMode.normal;

    void Start()
    {
        cardManager = CardManager.Instance;
        CacheUI();
        InitEvents();
        RefreshUI();
    }

    private void CacheUI()
    {
        var root = transform;

        cardBtn = root.Find("TopCenter/Menu/Card");
        cardIcon1 = cardBtn.Find("icon1");
        cardIcon2 = cardBtn.Find("icon2");
        cardSelect = cardBtn.Find("Select");

        closeBtn = root.Find("RightTop/Close");

        scrollViewContent = root.Find("Center/Scroll View/Viewport/Content");

        detailPanel = Instantiate(DetailPanel, root.Find("Center"));
        detailPanel.SetActive(false);

        detailCostText = detailPanel.transform.Find("Center/Cost/Text").GetComponent<Text>();
        detailNameText = detailPanel.transform.Find("Top/Title").GetComponent<Text>();
        detailIconImage = detailPanel.transform.Find("Center/Icon").GetComponent<Image>();
        detailDescText = detailPanel.transform.Find("Bottom/Description").GetComponent<Text>();

        detailBtn = root.Find("Bottom/BottomMenus/DetailBtn");
        goldAmount = root.Find("Bottom/BottomMenus/Gold/amount");
        goldAmount.GetComponent<Text>().text = $"{cardManager.gold}";
    }

    private void InitEvents()
    {
        cardBtn.GetComponent<Button>().onClick.AddListener(ShowCards);
        closeBtn.GetComponent<Button>().onClick.AddListener(() => loadMapEvent.RaiseEvent(null, this));
        detailBtn.GetComponent<Button>().onClick.AddListener(ShowCardDetail);
        cardSearchBar.OnSearchKeywordChanged += OnSearchChanged;
    }

    private void ShowCards()
    {
        curMode = BackpackMode.card;
        RefreshUI();
        DisplayCardList(cardManager.GetAllCards());
    }

    private void RefreshUI()
    {
        cardIcon1.gameObject.SetActive(curMode != BackpackMode.card);
        cardIcon2.gameObject.SetActive(curMode == BackpackMode.card);
        cardSelect.gameObject.SetActive(curMode == BackpackMode.card);
        cardSearchBar.gameObject.SetActive(curMode == BackpackMode.normal ? false : true);

        foreach (Transform child in scrollViewContent){
            Destroy(child.gameObject);
        }
    }

    private void DisplayCardList(List<ICardData> cards)
    {
        RefreshUI();

        foreach (var card in cards)
        {
            var itemGO = Instantiate(BackpackUIItem, scrollViewContent);

            itemGO.transform.Find("Top/Icon").GetComponent<Image>().sprite = card.Sprite;
            itemGO.transform.Find("Bottom/Text").GetComponent<Text>().text = card.CardName;
            itemGO.transform.Find("Top/Cost/Text").GetComponent<Text>().text = card.Cost.ToString();
            itemGO.transform.Find("Select").gameObject.SetActive(false);

            itemGO.GetComponent<Button>().onClick.AddListener(() => OnCardClicked(itemGO, card));
        }

        Debug.Log($"展示卡牌，共 {cards.Count} 张");
    }

    private void OnCardClicked(GameObject itemGO, ICardData card)
    {
        if (selectedCardGO != null){
            selectedCardGO.transform.Find("Select").gameObject.SetActive(false);
        }

        selectedCardGO = itemGO;
        selectedCard = card;
        selectedCardGO.transform.Find("Select").gameObject.SetActive(true);
    }

    private void ShowCardDetail()
    {
        if (selectedCard == null) return;

        detailCostText.text = selectedCard.Cost.ToString();
        detailNameText.text = selectedCard.CardName;
        detailIconImage.sprite = selectedCard.Sprite;
        detailDescText.text = selectedCard.Desc;

        detailPanel.SetActive(true);
    }

    private void OnSearchChanged(string keyword)
    {
        var filtered = CardFilterUtils.FilterCards(keyword, cardManager.GetAllCards());
        DisplayCardList(filtered);
    }
}
