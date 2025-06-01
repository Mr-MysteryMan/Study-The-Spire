using System.Collections;
using System.Collections.Generic;
using Combat;
using Combat.Buffs;
using UnityEngine;

namespace Cards.CardEffect.Buffs
{
    public class AddBuffEffect<T> : IEffect where T : IBuff
    {
        private int count;

        private T buff;

        public AddBuffEffect(T buff, int count = 1)
        {
            this.buff = buff;
            this.count = count;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var target in targets)
            {
                source.AddBuff(target, buff, count);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}