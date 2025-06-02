using Cards.CardEffect;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{

    public interface IICon
    {
        public Sprite Icon { get; }
    }

    public enum EnemyEffectType
    {
        None,
        Attack,

        Defend,
        Buff,
        Debuff,
    }

    public interface ITypedEffect : IEffect
    {
        EnemyEffectType EffectType { get; }
        CardEffectTarget TargetType { get; }
    }

    public interface IEnemyEffect : IEffect, IICon, ITypedEffect
    {
    }
}