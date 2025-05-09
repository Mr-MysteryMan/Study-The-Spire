using Cards.CardEffect;

namespace Cards.CardDatas
{
    public class TypedCardData : CardData
    {
        public TypedCardData(CardType cardType, int cardValue, int CardCost) : base(cardType, cardValue, CardCost, GetTypedEffect(cardType, cardValue)) { }

        public static IEffect GetTypedEffect(CardType cardType, int value)
        {
            return cardType switch
            {
                CardType.Attack => new AttackEffect(value),// 攻击效果
                CardType.Defense => new DefenseEffect(value),// 防御效果
                CardType.Heal => new HealEffect(value),// 治疗效果
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }
    }
}