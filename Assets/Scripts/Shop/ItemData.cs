using UnityEngine;

[System.Serializable]
public class ItemData : CardData
{
    public int gold;

    // TODO: 卡片费用
    public ItemData(CardType cardType, int cardValue, int gold)
        : base(cardType, cardValue, Random.Range(2, 5)) // 随机生成费用
    {
        this.gold = gold;
    }
}