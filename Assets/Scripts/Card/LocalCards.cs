using System.Collections.Generic;
using Cards.CardDatas;

namespace Cards
{
    public class LocalCards
    {
        public static List<ICardData> cards = new() {
            new TypedCardData(CardType.Attack, 10, 1),
            new TypedCardData(CardType.Attack, 10, 1),
            new TypedCardData(CardType.Heal, 6, 1),
            new TypedCardData(CardType.Defense, 8, 1),
            new TypedCardData(CardType.Defense, 8, 1),

            

            new ApplyWeak(2, 0),
            new ApplyVulnerable(2, 0),
            new ApplyFragil(2, 0),
            new ApplyPoison(4, 1),
            new GainStrength(2, 1)
        };

        public static List<ICardData> GetCards()
        {
            return cards; // 获取本地卡片数据
        }
    }
}