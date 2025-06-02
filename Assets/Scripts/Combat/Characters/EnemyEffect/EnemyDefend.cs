using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyDefend", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyDefend")]
    public class EnemyDefend : TypedEffectSOBase
    {
        public int AmmorAmount;

        public override EnemyEffectType EffectType => EnemyEffectType.Defend;
        public override CardEffectTarget TargetType => CardEffectTarget.AllySelf;

        public override IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var target in targets)
            {
                source.AddAmmor(target, AmmorAmount);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}