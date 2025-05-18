using Cards.CardEffect;
using Cards.CardEffect.Buffs;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;
using UnityEngine;
using Cards.Modifier;

namespace Cards.CardDatas
{
    public abstract class OneValueCardData : BasicCardData
    {
        public OneValueCardData(string cardName, int cardValue, int cardCost, CardCategory cardCategory, CardEffectTarget cardEffectTarget,
            Sprite sprite) : base(cardName, cardCost, cardCategory, cardEffectTarget, sprite)
        {
            this.cardValue = cardValue;
        }

        [Modifier.ModifyAttribute.Basic]
        protected int cardValue;

        public override void Modify(float factor)
        {
            BasicCardModifier.Modify(this, factor);
        }
    }

    public class GainStrength : OneValueCardData
    {
        public GainStrength(int cardValue, int CardCost) : base("强壮", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.AdventurerSelf,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"获得{cardValue}层力量";
        public override IEffect Effect => new AddBuffEffect<Strength>(new Strength(), cardValue);

    }

    public class ApplyWeak : OneValueCardData
    {
        public ApplyWeak(int cardValue, int CardCost) : base("威吓", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层虚弱";
        public override IEffect Effect => new AddBuffEffect<Weak>(new Weak(), cardValue);
    }

    public class ApplyVulnerable : OneValueCardData
    {
        public ApplyVulnerable(int cardValue, int CardCost) : base("易伤", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层易伤";
        public override IEffect Effect => new AddBuffEffect<Vulnerable>(new Vulnerable(), cardValue);
    }

    public class ApplyFragil : OneValueCardData
    {
        public ApplyFragil(int cardValue, int CardCost) : base("脆弱", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层脆弱";
        public override IEffect Effect => new AddBuffEffect<Fragil>(new Fragil(), cardValue);
    }

    public class ApplyPoison : OneValueCardData
    {
        public ApplyPoison(int cardValue, int CardCost) : base("中毒", cardValue, CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("CardUI/AttackCardSprite"))
        { }

        public override string Desc => $"给予一名敌人{cardValue}层中毒";
        public override IEffect Effect => new AddBuffEffect<Poison>(new Poison(), cardValue);
    }
}