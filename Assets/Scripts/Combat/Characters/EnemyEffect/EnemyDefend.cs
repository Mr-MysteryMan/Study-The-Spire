using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyDefend", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyDefend")]
    public class EnemyDefend : TypedEffectSOBase
    {
        public int AmmorAmount;

        public override EnemyEffectType EffectType => EnemyEffectType.Defend;
        public override CardEffectTarget TargetType => CardEffectTarget.EnemyOne;

        public override void Work(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.AddAmmor(target, AmmorAmount);
            }
        }
    }
}