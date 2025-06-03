using System.Collections;
using System.Collections.Generic;
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

    public interface IEnemyEffect
    {
        EnemyEffectType EffectType { get; }
        CardEffectTarget TargetType { get; }

        IEffect Effect { get; }
        string Desc { get; }
    }

    public static class EnemyEffectExtensions
    {
        public static IEnumerator Work(this IEnemyEffect effect, Character source, List<Character> targets)
        {
            if (effect == null || effect.Effect == null)
            {
                yield break;
            }

            yield return effect.Effect.Work(source, targets);
        }
    }

    public enum EnemyEffectTargetType
    {
        Self, All, RandomOne
    }
}