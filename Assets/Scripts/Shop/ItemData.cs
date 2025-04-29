[System.Serializable]
public class ItemData : CardData
{
    public int gold;

    public ItemData(CardType cardType, int cardValue, int gold)
        : base(cardType, cardValue)
    {
        this.gold = gold;
    }
}