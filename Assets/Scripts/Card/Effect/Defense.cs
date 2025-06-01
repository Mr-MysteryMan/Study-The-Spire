using Combat;
using Cards.CardEffect;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Cards.CardEffect
{
    class DefenseEffect : IEffect, ISyncEffect
    {
        private int Defense; // 防御数值
        public DefenseEffect(int Defense)
        {
            this.Defense = Defense;
        }
        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            WorkSync(source, targets); // 执行防御
            yield return new WaitForSeconds(0.2f);
        }

        public void WorkSync(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.AddAmmor(target, Defense); // 执行防御
            }
        }
    }
}