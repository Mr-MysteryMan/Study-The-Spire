using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    [Header("卡片游戏组件")]
    //----------------------游戏组件---------------------//
    public GameObject cardObj;
    public Image background; // 卡片背景
    public Text cardValueText; // 卡片数值文本

    public Text cardCostText; // 卡片费用文本

    [Header("卡片基本信息")]
    //----------------------基本信息---------------------//
    private CardData cardData;

    public CardData CardData => cardData; // 卡片数据

    public CardType cardType => cardData.cardType; // 卡牌类型

    public int cardValue => cardData.cardValue; // 卡牌数值

    public int cardCost => cardData.cost; // 卡牌费用

    public int cardId => cardData.cardId; // 卡牌ID

    public CardEffect effect; // 卡牌效果接口
    public bool isDiscarded => cardData.isDiscarded; // 是否是弃牌
    private Combat.CardManager manager; // 卡片管理器

    //----------------------更新方法---------------------//

    public void updateCardStatus(CardData data) {
        cardData = data;
        Debug.Log(cardData.cost);
        Debug.Log(cardData.cardValue);
        // 生成卡片效果
        this.effect = getCardEffect(); // 生成卡片效果
        // 更新UI
        UpdateCardUI();
    }

    //----------------------鼠标放上时升起, 离开时落下---------------------//
    private float animationDuration = 0.2f; // 动画时间
    private int originalSiblingIndex; // 原始的层级索引
    
    private Coroutine animationCoroutine; // 用于存储协程

    private Vector3 originalScale = new Vector3(1f,1f,1f); // 原始缩放比例
    private Vector3 targetScale = new Vector3(1.1f,1.1f,1.1f); // 放大比例
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 如果已经有动画在播放，先停止它
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        // 记录原始的层级索引
        originalSiblingIndex = transform.GetSiblingIndex();
        // 将当前卡片的层级提升到最上面
        transform.SetAsLastSibling();
        // 开始放大动画
        animationCoroutine = StartCoroutine(AnimateTransform(originalScale * 1.1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 如果已经有动画在播放，先停止它
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        // 恢复原始的层级索引
        transform.SetSiblingIndex(originalSiblingIndex);
        // 开始还原大小和层级动画
        animationCoroutine = StartCoroutine(AnimateTransform(originalScale));
    }

    public void addManager(Combat.CardManager manager) {
        this.manager = manager; // 设置卡片管理器
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (manager == null) return; // 如果没有卡片管理器, 则不执行
        
        var from = manager.getUser(this); // 获取使用者
        var to = manager.getTarget(this); // 获取目标
        if (from && to) { // 如果有使用者和目标
            this.effect.work(from, to); // 执行卡牌效果
            manager.reportUse(this); // 向卡片管理器报告使用
        }
    }

    private IEnumerator AnimateTransform(Vector3 targetScale)
    {
        float timer = 0f;
        Vector3 startingScale = transform.localScale;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, timer / animationDuration);
            yield return null;
        }

        // 确保最终大小和层级正确
        transform.localScale = targetScale;

        // 清除协程引用
        animationCoroutine = null;
    }

    //----------------------更新UI---------------------//
    public void UpdateCardUI() {
        // 背景
        this.background.sprite = CardUI.GetCardBackground(cardType); // 设置卡片背景
        // 数字
        this.cardValueText.text = cardValue.ToString(); // 设置卡片数值文本
        this.cardCostText.text = cardCost.ToString(); // 设置卡片数值颜色
        // 弃牌
        if (isDiscarded) { //弃置时背景半透明
            Color color = this.background.color;
            color.a = 0.7f;
            this.background.color = color;
        } else {
            Color color = this.background.color;
            color.a = 1f;
            this.background.color = color;
        }
    }

    //----------------------卡牌效果---------------------//
    public CardEffect getCardEffect() {
        switch (cardType) {
            case CardType.Attack:
                return new AttackEffect(cardValue);
            case CardType.Defense:
                return new DefenseEffect(cardValue);
            case CardType.Heal:
                return new HealEffect(cardValue);
            default:
                return null;
        }
    }
}
public interface CardEffect {
    public void work(Character from, Character to);
}
