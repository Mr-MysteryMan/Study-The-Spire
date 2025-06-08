
using System.Collections.Generic;
using Cards.CardEffect;
using Cards.CardEffect.Cycle;
using UnityEngine;

/// <summary>
/// 这一类卡牌用来抽牌，或者回费相关。
/// <summary>
namespace Cards.CardDatas.Cycle
{

    public abstract class OneValueNotModifyCardData : CardData
    {
        public OneValueNotModifyCardData(string cardName, int cardValue, int cardCost, CardCategory cardCategory, CardEffectTarget cardEffectTarget,
            Sprite sprite)
        {
            this.cardValue = cardValue;
            this.cardCost = cardCost; // 设置卡牌费用
            this.cardName = cardName; // 设置卡牌名称
            this.cardCategory = cardCategory; // 设置卡牌分类
            this.cardEffectTarget = cardEffectTarget; // 设置卡牌效果目标
            this.sprite = sprite; // 设置卡牌图片
        }

        protected int cardCost; // 卡牌费用
        protected string cardName; // 卡牌名称

        protected CardCategory cardCategory; // 卡牌分类

        protected CardEffectTarget cardEffectTarget; // 卡牌效果目标

        private Sprite sprite; // 卡牌图片
        public override CardCategory CardCategory => cardCategory; // 卡牌分类属性
        public override CardEffectTarget CardEffectTarget => cardEffectTarget; // 卡牌效果目标属性
        public override Sprite Sprite => sprite; // 卡牌图片属性
        public override int Cost => cardCost; // 卡牌费用属性
        public override string CardName => cardName; // 卡牌名称属性
        protected int cardValue;
    }

    public class DrawCardData : OneValueNotModifyCardData
    {
        public DrawCardData(int cardValue, int cardCost) : base("抽牌", cardValue, cardCost, CardCategory.Skill, CardEffectTarget.AllySelf,
            sprite: CardResources.DrawCardSprite)
        { }

        public override string Desc => $"抽取{cardValue}张牌";

        public override IEffect Effect => new DrawCardEffect(cardValue);

        public override ICardData Clone()
        {
            return new DrawCardData(cardValue, cardCost);
        }
    }

    public class DiscardAndDrawCardData : OneValueNotModifyCardData
    {
        public DiscardAndDrawCardData(int cardValue, int cardCost) : base("制衡", cardValue, cardCost, CardCategory.Skill, CardEffectTarget.AllySelf,
            sprite: CardResources.DrawCardSprite)
        { }

        public override string Desc => $"丢弃随机1张牌并抽取{cardValue}张牌";

        public override IEffect Effect => new ListSyncEffect
        {
            new DiscardRandomCardEffect(1),
            new DrawCardEffect(cardValue)
        };

        public override ICardData Clone()
        {
            return new DiscardAndDrawCardData(cardValue, cardCost);
        }
    }
}