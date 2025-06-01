namespace Combat.Events
{
    public class HealDealtEvent
    {
        public int Heal { get; } // 治疗量
        public Character Source { get; } // 治疗来源
        public Character Target { get; } // 治疗目标

        public HealDealtEvent(int heal, Character source, Character target)
        {
            Heal = heal;
            Source = source;
            Target = target;
        }
    }
}