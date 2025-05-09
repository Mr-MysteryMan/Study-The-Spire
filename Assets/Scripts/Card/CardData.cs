using Cards.CardEffect;
using UnityEngine;

public enum CardType
{
    Attack, // 攻击
    Defense, // 防御
    Heal // 治疗
}

public interface ICardData
{
    CardType CardType { get; } // 卡牌类型
    int CardValue { get; } // 卡牌数值
    int CardId { get; } // 卡牌ID
    int Cost { get; } // 卡牌费用

    bool IsDiscarded { get; } // 是否是弃牌

    IEffect Effect { get; } // 卡牌效果

    void Discard(); // 弃牌方法
    void Reset(); // 重置方法
}

public class CardData : ICardData
{
    private CardType cardType; // 卡牌类型

    private int cardValue; // 卡牌数值

    private bool isDiscarded; // 是否是弃牌

    private int cardId; // 卡牌ID

    private int cost; // 卡牌费用

    private IEffect effect; // 卡牌效果

    public CardData(CardType cardType, int cardValue, int cardCost, IEffect effect = null)
    {
        this.cardType = cardType;
        this.cardValue = cardValue;
        this.cost = cardCost; 
        this.effect = effect; // 设置卡牌效果
        isDiscarded = false; // 创建时不是弃牌

        // 生成唯一的卡牌ID 使用系统时间戳
        cardId = (int)((System.DateTime.Now.Ticks + Random.Range(99, 99999)) % int.MaxValue);
    }

    public CardType CardType => cardType;

    public int CardValue => cardValue;

    public bool IsDiscarded => isDiscarded;

    public int CardId => cardId;

    public int Cost => cost;

    public IEffect Effect => effect;

    public void Discard()
    {
        isDiscarded = true; // 标记为弃牌
    }

    public void Reset()
    {
        isDiscarded = false; // 重置为非弃牌
    }

    public void SetEffect(IEffect effect)
    {
        this.effect = effect; // 设置卡牌效果
    }
}