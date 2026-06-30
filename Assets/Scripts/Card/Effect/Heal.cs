using Combat;
using Cards.CardEffect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.CardEffect
{
    class HealEffect : IEffect, ISyncEffect
    {
        private int health; // 生命值
        public HealEffect(int health)
        {
            this.health = health;
        }
        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return WaitForAllEnd(source, targets);
            WorkSync(source, targets); // 执行治疗
            yield return new WaitForSeconds(0.2f);
        }

        public void WorkSync(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.Heal(target, health); // 执行治疗
            }
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