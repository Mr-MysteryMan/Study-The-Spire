using Cards.CardDatas;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int gold;

    public ICardData cardData;

    public ItemData(int gold, ICardData cardData)
    {
        this.cardData = cardData;
        this.gold = gold;
    }

    public ItemData(int gold, int cardValue, int cost, CardType cardType)
    {
        this.cardData = new TypedCardData(cardType, cardValue, cost);
        this.gold = gold;
    }
}