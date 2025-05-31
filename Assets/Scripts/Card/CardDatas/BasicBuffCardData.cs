using Cards.CardEffect;
using Cards.CardEffect.Buffs;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;
using UnityEngine;
using Cards.Modifier;
using Combat;
using Combat.Characters;

namespace Cards.CardDatas
{
    public abstract class OneValueCardData : CardData
    {
        public OneValueCardData(string cardName, int cardValue, int cardCost, CardCategory cardCategory, CardEffectTarget cardEffectTarget,
            Sprite sprite)
        {
            this.cardValue = cardValue;
            this.cardCost = cardCost; // 设置卡牌费用
            this.cardName = cardName; // 设置卡牌名称
            this.cardCategory = cardCategory; // 设置卡牌分类
            this.cardEffectTarget = cardEffectTarget; // 设置卡牌效果目标
            this.sprite = sprite; // 设置卡牌图片
        }

        private int cardCost; // 卡牌费用
        private string cardName; // 卡牌名称

        private CardCategory cardCategory; // 卡牌分类

        private CardEffectTarget cardEffectTarget; // 卡牌效果目标

        private Sprite sprite; // 卡牌图片
        public override CardCategory CardCategory => cardCategory; // 卡牌分类属性
        public override CardEffectTarget CardEffectTarget => cardEffectTarget; // 卡牌效果目标属性
        public override Sprite Sprite => sprite; // 卡牌图片属性
        public override int Cost => cardCost; // 卡牌费用属性
        public override string CardName => cardName; // 卡牌名称属性

        [Modifier.ModifyAttribute.Basic(ModifyType.All),
         Modifier.ModifyAttribute.CharacterPower(CharacterPowerType.Attack, 1)]
        protected int cardValue;

        public override void Modify(float factor, ModifyType modifyType)
        {
            BasicCardModifier.Modify(this, factor, modifyType);
        }

        public override void Modify(Character character)
        {
            CharacterCardModifier.Modify(this, character);
        }
    }

    public class GainStrength : OneValueCardData
    {
        public GainStrength(int cardValue, int CardCost) : base("强壮", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.AdventurerSelf,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"获得{cardValue}层力量";
        public override IEffect Effect => new AddBuffEffect<Strength>(new Strength(), cardValue);

        public override ICardData Clone()
        {
            return new GainStrength(cardValue, Cost);
        }
    }

    public class ApplyWeak : OneValueCardData
    {
        public ApplyWeak(int cardValue, int CardCost) : base("威吓", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层虚弱";
        public override IEffect Effect => new AddBuffEffect<Weak>(new Weak(), cardValue);

        public override ICardData Clone()
        {
            return new ApplyWeak(cardValue, Cost);
        }
    }

    public class ApplyVulnerable : OneValueCardData
    {
        public ApplyVulnerable(int cardValue, int CardCost) : base("易伤", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层易伤";
        public override IEffect Effect => new AddBuffEffect<Vulnerable>(new Vulnerable(), cardValue);

        public override ICardData Clone()
        {
            return new ApplyVulnerable(cardValue, Cost);
        }
    }

    public class ApplyFragil : OneValueCardData
    {
        public ApplyFragil(int cardValue, int CardCost) : base("脆弱", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层脆弱";
        public override IEffect Effect => new AddBuffEffect<Fragil>(new Fragil(), cardValue);

        public override ICardData Clone()
        {
            return new ApplyFragil(cardValue, Cost);
        }
    }

    public class ApplyPoison : OneValueCardData
    {
        public ApplyPoison(int cardValue, int CardCost) : base("中毒", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层中毒";
        public override IEffect Effect => new AddBuffEffect<Poison>(new Poison(), cardValue);

        public override ICardData Clone()
        {
            return new ApplyPoison(cardValue, Cost);
        }
    }
}