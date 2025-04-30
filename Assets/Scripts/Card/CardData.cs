using UnityEngine;

public enum CardType
{
    Attack, // 攻击
    Defense, // 防御
    Heal // 治疗
}

public class CardData
{
    public CardType cardType; // 卡牌类型

    public int cardValue; // 卡牌数值

    public bool isDiscarded; // 是否是弃牌

    public int cardId; // 卡牌ID

    public int cost; // 卡牌费用

    public CardData(CardType cardType, int cardValue, int CardCost)
    {
        this.cardType = cardType;
        this.cardValue = cardValue;
        this.cost = CardCost; 
        isDiscarded = false; // 创建时不是弃牌

        // 生成唯一的卡牌ID 使用系统时间戳
        cardId = (int)((System.DateTime.Now.Ticks + Random.Range(99, 99999)) % int.MaxValue);
    }

    public void Discard()
    {
        isDiscarded = true; // 标记为弃牌
    }

    public void Reset()
    {
        isDiscarded = false; // 重置为非弃牌
    }
}
