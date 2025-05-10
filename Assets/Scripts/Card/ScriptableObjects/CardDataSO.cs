using UnityEngine;
using System.Collections.Generic;
using Cards.CardEffect;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardDataSO", menuName = "ScriptableObjects/CardDataSO", order = 1)]
    public class CardDataSO : ScriptableObject
    {
        public int cardCost; // 卡牌费用
        public string cardName; // 卡牌名称
        public string cardDesc; // 卡牌描述
        public EffectSOBase effect; // 卡牌效果

        public CardCategory cardCategory; // 卡牌分类

        public CardEffectTarget cardEffectTarget; // 卡牌效果目标

        public Sprite sprite; // 卡牌图片

        public ICardData ToCardData()
        {
            return new BasicCardData(cardName, cardDesc, cardCost, cardCategory, cardEffectTarget, sprite, effect);
        }
    }
}