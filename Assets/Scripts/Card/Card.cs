using System.Collections;
using Combat;
using Cards.CardEffect;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [Header("卡片游戏组件")]
    //----------------------游戏组件---------------------//
    public GameObject cardObj;
    public Image background; // 卡片背景
    public Image image; // 卡片内容图片

    public Text cardCostText; // 卡片费用文本
    public Text cardNameText; // 卡片名称文本
    public Text cardDescText; // 卡片描述文本

    [Header("卡片基本信息")]
    //----------------------基本信息---------------------//
    private ICardData cardData;

    public ICardData CardData => cardData; // 卡片数据

    public int CardCost => cardData.Cost; // 卡牌费用

    public int CardId => cardData.CardId; // 卡牌ID

    public IEffect Effect => cardData.Effect; // 卡牌效果接口
    public bool IsDiscarded => cardData.IsDiscarded; // 是否是弃牌
    private Combat.CardManager manager; // 卡片管理器

    //----------------------更新方法---------------------//

    public void updateCardStatus(ICardData data) {
        cardData = data;
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
        // 按照CardCategory加载背景spirit
        this.background.sprite = cardData.CardCategory switch {
            _ => Resources.Load<Sprite>("CardUI/CardBackGround"),
        };
        this.image.sprite = cardData.Sprite; // 设置卡片图片
        this.cardCostText.text = cardData.Cost.ToString();// 加载卡片费用
        this.cardNameText.text = cardData.CardName; // 加载卡片名称
        this.cardDescText.text = cardData.Desc; // 加载卡片描述
        // 弃牌
        if (IsDiscarded) { //弃置时背景半透明
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
    public IEffect getCardEffect() {
        return this.cardData.Effect; // 获取卡牌效果
    }
}