using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyHeal", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyHeal")]
    public class EnemyHeal : TypedEffectSOBase
    {
        public int HealAmount;
        public override EnemyEffectType EffectType => EnemyEffectType.Buff;
        public override CardEffectTarget TargetType => CardEffectTarget.AllySelf;

        public override IEnumerator Work(Character source, List<Character> targets)
        {
            yield return WaitForAllEnd(source, targets);
            foreach (var target in targets)
            {
                source.Heal(target, HealAmount);
            }
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator WaitForAllEnd(Character source, List<Character> targets)
        {
            Coroutine[] coroutines = new Coroutine[targets.Count];
            for (int i = 0; i < targets.Count; i++)
            {
                coroutines[i] = source.StartCoroutine(targets[i].vfxManager.PlayHealAsny());
            }
            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
        
    }
}