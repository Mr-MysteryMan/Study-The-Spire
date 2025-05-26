using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyHeal", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyHeal")]
    public class EnemyHeal : TypedEffectSOBase
    {
        public int HealAmount;
        public override EnemyEffectType EffectType => EnemyEffectType.Buff;
        public override CardEffectTarget TargetType => CardEffectTarget.EnemyOne;

        public override void Work(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.Heal(target, HealAmount);
            }
        }
    }
}