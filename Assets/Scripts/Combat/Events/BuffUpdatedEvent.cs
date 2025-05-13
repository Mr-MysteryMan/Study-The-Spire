using Combat.Buffs;

namespace Combat.Events
{
    public class BuffUpdatedEvent
    {
        public BuffUpdatedEvent(Character source, Character target, IBuff buff)
        {
            Source = source;
            Target = target;
            Buff = buff;
        }

        public Character Source { get; }

        public Character Target { get; }

        public IBuff Buff { get; }
    }
}