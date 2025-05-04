using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackpackPanelManager : MonoBehaviour
{
    private CardManager cardManager;
    private Transform topCenter;
    private Transform attackBtn, defenceBtn, healBtn, closeBtn;
    private Transform attackIcon1, attackIcon2, attackSelect;
    private Transform defenceIcon1, defenceIcon2, defenceSelect;
    private Transform healIcon1, healIcon2, healSelect;
    private Transform centerPanel;
    private Transform scrollViewContent;
    private Transform detailPanel;
    private Transform bottomPanel;
    private Transform bottomMenus;
    private Transform detailBtn;
    private CardType currentType;

    // 卡牌展示用的预制体
    public GameObject BackpackUIItem;

    void Start()
    {
        cardManager = CardManager.Instance;
        // Debug.Log(cardManager == null ? "CardManager 为 null!" : "CardManager 初始化成功");
        CacheUI();
        InitClick();
        ShowCardsByType(CardType.Attack);
    }

    private void CacheUI()
    {
        var root = transform;

        topCenter = root.Find("TopCenter");

        attackBtn = topCenter.Find("Menu/Attack");
        attackIcon1 = attackBtn.Find("icon1");
        attackIcon2 = attackBtn.Find("icon2");
        attackSelect = attackBtn.Find("Select");

        defenceBtn = topCenter.Find("Menu/Defence");
        defenceIcon1 = defenceBtn.Find("icon1");
        defenceIcon2 = defenceBtn.Find("icon2");
        defenceSelect = defenceBtn.Find("Select");

        healBtn = topCenter.Find("Menu/Heal");
        healIcon1 = healBtn.Find("icon1");
        healIcon2 = healBtn.Find("icon2");
        healSelect = healBtn.Find("Select");

        closeBtn = root.Find("RightTop/Close");

        centerPanel = root.Find("Center");
        scrollViewContent = centerPanel.Find("Scroll View/Viewport/Content");
        detailPanel = centerPanel.Find("DetailPanel");

        bottomPanel = root.Find("Bottom");
        bottomMenus = bottomPanel.Find("BottomMenus");
        detailBtn = bottomMenus.Find("DetailBtn");

        detailPanel.gameObject.SetActive(false);
        bottomMenus.Find("Gold/amount").GetComponent<Text>().text = cardManager.Gold.ToString();
    }

    private void InitClick()
    {
        attackBtn.GetComponent<Button>().onClick.AddListener(OnClickAttack);
        defenceBtn.GetComponent<Button>().onClick.AddListener(OnClickDefence);
        healBtn.GetComponent<Button>().onClick.AddListener(OnClickHeal);
        closeBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        detailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);
    }

    // 点击关闭按钮
    private void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    // 点击Attack分类
    private void OnClickAttack()
    {
        Debug.Log("进入attack卡牌界面");
        ShowCardsByType(CardType.Attack);
    }

    // 点击Defence分类
    private void OnClickDefence()
    {
        Debug.Log("进入defence卡牌界面");
        ShowCardsByType(CardType.Defense);
    }

    // 点击Heal分类
    private void OnClickHeal()
    {
        Debug.Log("进入heal卡牌界面");
        ShowCardsByType(CardType.Heal);
    }

    // 点击查看详情按钮（后续扩展）
    private void OnDetail()
    {
        if (selectedCard != null) {
            ShowCardDetail(selectedCard);
        }
    }

    // 展示指定类型的卡牌，并刷新 Scroll View
    private void ShowCardsByType(CardType type)
    {
        currentType = type;
        UpdateTopIcons(type);
        RefreshCardList(type);
    }

    // 根据卡牌类型更新顶部按钮图标状态
    private void UpdateTopIcons(CardType selectedType)
    {
        bool isAttack = selectedType == CardType.Attack;
        bool isDefence = selectedType == CardType.Defense;
        bool isHeal = selectedType == CardType.Heal;

        attackIcon1.gameObject.SetActive(!isAttack);
        attackIcon2.gameObject.SetActive(isAttack);
        attackSelect.gameObject.SetActive(isAttack);

        defenceIcon1.gameObject.SetActive(!isDefence);
        defenceIcon2.gameObject.SetActive(isDefence);
        defenceSelect.gameObject.SetActive(isDefence);

        healIcon1.gameObject.SetActive(!isHeal);
        healIcon2.gameObject.SetActive(isHeal);
        healSelect.gameObject.SetActive(isHeal);
    }

    // 刷新Scroll View中的卡牌列表（根据卡牌类型）
    private void RefreshCardList(CardType type)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        List<CardData> cards = cardManager.GetCardsByType(type);

        foreach (var card in cards)
        {
            GameObject itemGO = Instantiate(BackpackUIItem, scrollViewContent);

            Image icon = itemGO.transform.Find("Top/Icon").GetComponent<Image>();
            Sprite sprite = CardUI.GetCardBackground(type);
            if (sprite) icon.sprite = sprite;
            Text text = itemGO.transform.Find("Bottom/Text").GetComponent<Text>();
            text.text = card.cardValue.ToString();

            itemGO.transform.Find("Select").gameObject.SetActive(false);

            Button btn = itemGO.GetComponent<Button>();
            if (btn != null) {
                btn.onClick.AddListener(() => {
                    OnCardClicked(itemGO, card);
                });
            }
        }
        Debug.Log($"展示{type}类卡牌，共 {cards.Count} 张");
    }

    private GameObject selectedCardGO = null;
    private CardData selectedCard = null;

    private void OnCardClicked(GameObject itemGO, CardData card) {
        if (selectedCardGO != null) {
            selectedCardGO.transform.Find("Select").gameObject.SetActive(false);
        }

        selectedCardGO = itemGO;
        selectedCard = card;
        selectedCardGO.transform.Find("Select").gameObject.SetActive(true);
    }

    private void ShowCardDetail(CardData card) {
        detailPanel.gameObject.SetActive(true);

        Text nameText = detailPanel.Find("Top/Title").GetComponent<Text>();
        Image iconImage = detailPanel.Find("Center/Icon").GetComponent<Image>();
        Text descText = detailPanel.Find("Bottom/Description").GetComponent<Text>();
        Sprite icon = CardUI.GetCardBackground(card.cardType);
        if (icon) iconImage.sprite = icon;
        if (card.cardType == CardType.Attack) {
            nameText.text = "攻击卡牌";
            descText.text = $"该卡牌攻击属性为{card.cardValue},cost为{card.cost}";
        } else if (card.cardType == CardType.Defense) {
            nameText.text = "防御卡牌";
            descText.text = $"该卡牌防御属性为{card.cardValue},cost为{card.cost}";
        } else if (card.cardType == CardType.Heal)  {
            nameText.text = "治愈卡牌";
            descText.text = $"该卡牌治愈属性为{card.cardValue},cost为{card.cost}";
        }
    }
}