using Cards;
using Cards.CardDatas;
using Cards.CardEffect;
using UnityEngine;

namespace Cars.CardDatas
{
    public class QuestionCardData : OneValueCardData
    {
        public QuestionCardData(int cardValue, int cardCost) : base("回答", cardValue, cardCost, CardCategory.Skill, CardEffectTarget.AdventurerSelf,
        sprite: CardResources.QuestionCardSprite)
        { }

        public override string Desc => $"回答一道问题，如果回答正确，随机卡牌的数值提升为{cardValue / 100f}倍";

        public override ICardData Clone()
        {
            return new QuestionCardData(cardValue, Cost);
        }

        public override IEffect Effect => QuestionEffectFactory.AmpilfyQuestion(cardValue / 100f);
    }
}