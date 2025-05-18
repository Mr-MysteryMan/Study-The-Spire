using Cards.CardEffect;
using UnityEngine;

namespace Cards.CardDatas
{
    public enum CardType
    {
        Attack,
        Defense,
        Heal
    }

    public class TypedCardData : CardData
    {
        public TypedCardData(CardType cardType, int cardValue, int cardCost)
        {
            this.cardType = cardType; // 设置卡牌类型
            this.cardValue = cardValue; // 设置卡牌数值
            this.cardCost = cardCost; // 设置卡牌费用
        }

        [Modifier.ModifyAttribute.Basic(ModifyType.All)]
        private int cardValue; // 卡牌数值
        private CardType cardType; // 卡牌类型
        private int cardCost; // 卡牌费用

        public CardType CardType => cardType; // 卡牌类型属性
        public override int Cost => cardCost; // 卡牌费用属性

        public override CardEffectTarget CardEffectTarget => GetCardEffectTarget(cardType); // 卡牌效果目标属性
        public override IEffect Effect => GetTypedEffect(cardType, cardValue); // 卡牌效果属性
        public override string Desc => GetTypedDesc(cardType, cardValue); // 卡牌描述属性
        public override string CardName => GetTypedName(cardType); // 卡牌名称属性
        public override Sprite Sprite => GetTypedSprite(cardType); // 卡牌图片属性
        public override CardCategory CardCategory => GetCardCategory(cardType); // 卡牌分类属性

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

        public static string GetTypedDesc(CardType cardType, int value)
        {
            return cardType switch
            {
                CardType.Attack => "造成" + value + "点伤害",// 攻击效果描述
                CardType.Defense => "获得" + value + "点护甲",// 防御效果描述
                CardType.Heal => "恢复" + value + "点生命值",// 治疗效果描述
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }

        public static string GetTypedName(CardType cardType)
        {
            return cardType switch
            {
                CardType.Attack => "攻击",// 攻击效果名称
                CardType.Defense => "防御",// 防御效果名称
                CardType.Heal => "治疗",// 治疗效果名称
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }

        public static CardEffectTarget GetCardEffectTarget(CardType cardType)
        {
            return cardType switch
            {
                CardType.Attack => CardEffectTarget.EnemyOne,// 攻击效果目标
                CardType.Defense => CardEffectTarget.AdventurerSelf,// 防御效果目标
                CardType.Heal => CardEffectTarget.AdventurerSelf,// 治疗效果目标
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }

        public static Sprite GetTypedSprite(CardType cardType)
        {
            return cardType switch
            {
                CardType.Attack => Resources.Load<Sprite>("CardUI/AttackCardSprite"),// 攻击效果图片
                CardType.Defense => Resources.Load<Sprite>("CardUI/DefenseCardSprite"),// 防御效果图片
                CardType.Heal => Resources.Load<Sprite>("CardUI/HealCardSprite"),// 治疗效果图片
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }

        public static CardCategory GetCardCategory(CardType cardType)
        {
            return cardType switch
            {
                CardType.Attack => CardCategory.Attack,// 攻击效果分类
                CardType.Defense => CardCategory.Skill,// 防御效果分类
                CardType.Heal => CardCategory.Status,// 治疗效果分类
                _ => throw new System.ArgumentOutOfRangeException(nameof(cardType), cardType, null) // 异常处理
            };
        }

        public override void Modify(float factor, ModifyType type)
        {
            Modifier.BasicCardModifier.Modify(this, factor, type); // 修改卡牌属性
        }
    }
}