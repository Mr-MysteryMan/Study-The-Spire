using UnityEngine;
using System.Collections.Generic;
using Cards.CardEffect;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardDataSO", menuName = "ScriptableObjects/CardDataSO", order = 1)]
    public class CardDataSO : ScriptableObject
    {
        public CardType CardType; // 卡牌类型

        public int CardValue; // 卡牌数值

        public int CardId; // 卡牌ID

        public int Cost; // 卡牌费用

        public EffectSOBase effect;

        public ICardData ToCardData() {
            return new CardData(CardType, CardValue, Cost, effect); // 创建新的卡牌数据对象
        }
    }
}