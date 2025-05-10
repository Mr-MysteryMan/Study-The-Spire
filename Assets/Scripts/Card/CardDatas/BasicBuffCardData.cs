using Cards.CardEffect;
using Cards.CardEffect.Buffs;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;
using UnityEngine;

namespace Cards.CardDatas
{
    public class GainStrength : BasicCardData
    {
        public GainStrength(int cardValue, int CardCost) : base("强壮", $"获得{cardValue}层力量", CardCost, CardCategory.Skill, CardEffectTarget.AdventurerSelf,
        sprite: Resources.Load<Sprite>("Sprites/Card/Strength"),
        effect: new AddBuffEffect<Strength>(new Strength(), cardValue))
        { }
    }

    public class ApplyWeak : BasicCardData
    {
        public ApplyWeak(int cardValue, int CardCost) : base("威吓", $"给予一名敌人{cardValue}层虚弱", CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("Sprites/Card/Weak"),
        effect: new AddBuffEffect<Weak>(new Weak(), cardValue))
        { }
    }

    public class ApplyVulnerable : BasicCardData
    {
        public ApplyVulnerable(int cardValue, int CardCost) : base("易伤", $"给予一名敌人{cardValue}层易伤", CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("Sprites/Card/Vulnerable"),
        effect: new AddBuffEffect<Vulnerable>(new Vulnerable(), cardValue))
        { }
    }

    public class ApplyFragil : BasicCardData
    {
        public ApplyFragil(int cardValue, int CardCost) : base("脆弱", $"给予一名敌人{cardValue}层脆弱", CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("Sprites/Card/Fragil"),
        effect: new AddBuffEffect<Fragil>(new Fragil(), cardValue))
        { }
    }

    public class ApplyPoison : BasicCardData
    {
        public ApplyPoison(int cardValue, int CardCost) : base("中毒", $"给予一名敌人{cardValue}层中毒", CardCost, CardCategory.Skill, CardEffectTarget.EnemyOne,
        sprite: Resources.Load<Sprite>("Sprites/Card/Poison"),
        effect: new AddBuffEffect<Poison>(new Poison(), cardValue))
        { }
    }
}