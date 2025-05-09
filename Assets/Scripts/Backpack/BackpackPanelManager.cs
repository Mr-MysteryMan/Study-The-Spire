using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class BackpackPanelManager : MonoBehaviour
{
    private CardManager cardManager;
    private Transform attackBtn, defenceBtn, healBtn, closeBtn, detailBtn;
    private Transform attackSelect, defenceSelect, healSelect;
    private Transform scrollViewContent, detailPanel;
    private CardCategory currentCategory;

    public GameObject BackpackUIItem;

    private GameObject selectedCardGO = null;
    private ICardData selectedCard = null;

    void Start()
    {
        cardManager = CardManager.Instance;
        CacheUI();
        InitClick();
        ShowCardsByCategory(CardCategory.Attack);
    }

    private void CacheUI()
    {
        var root = transform;

        attackBtn = root.Find("TopCenter/Menu/Attack");
        attackSelect = attackBtn.Find("Select");

        defenceBtn = root.Find("TopCenter/Menu/Defence");
        defenceSelect = defenceBtn.Find("Select");

        healBtn = root.Find("TopCenter/Menu/Heal");
        healSelect = healBtn.Find("Select");

        closeBtn = root.Find("RightTop/Close");
        detailBtn = root.Find("Bottom/BottomMenus/DetailBtn");

        scrollViewContent = root.Find("Center/Scroll View/Viewport/Content");
        detailPanel = root.Find("Center/DetailPanel");

        detailPanel.gameObject.SetActive(false);
    }

    private void InitClick()
    {
        attackBtn.GetComponent<Button>().onClick.AddListener(() => ShowCardsByCategory(CardCategory.Attack));
        defenceBtn.GetComponent<Button>().onClick.AddListener(() => ShowCardsByCategory(CardCategory.Skill));
        healBtn.GetComponent<Button>().onClick.AddListener(() => ShowCardsByCategory(CardCategory.Status));
        closeBtn.GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
        detailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);
    }

    private void ShowCardsByCategory(CardCategory category)
    {
        currentCategory = category;
        UpdateTopIcons(category);
        RefreshCardList(category);
    }

    private void UpdateTopIcons(CardCategory selectedCategory)
    {
        attackSelect.gameObject.SetActive(selectedCategory == CardCategory.Attack);
        defenceSelect.gameObject.SetActive(selectedCategory == CardCategory.Skill);
        healSelect.gameObject.SetActive(selectedCategory == CardCategory.Status);
    }

    private void RefreshCardList(CardCategory category)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        List<ICardData> cards = cardManager.GetCardsByCardCategory(category).ToList();

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

        Debug.Log($"展示{category}类卡牌，共 {cards.Count} 张");
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
}