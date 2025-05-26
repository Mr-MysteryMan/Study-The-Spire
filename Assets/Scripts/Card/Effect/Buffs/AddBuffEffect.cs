using System.Collections.Generic;
using Combat;
using Combat.Buffs;

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

        public void Work(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.AddBuff(target, buff, count);
            }
        }
    }
}