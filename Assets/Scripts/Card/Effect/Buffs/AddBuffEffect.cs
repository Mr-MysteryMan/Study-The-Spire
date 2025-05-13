using Combat;
using Combat.Buffs;
using Combat.Buffs.PermanentBuff;

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

        public void Work(Character source, Character target)
        {
            source.AddBuff(target, buff, count);
        }
    }
}