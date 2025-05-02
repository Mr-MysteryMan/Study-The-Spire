namespace Combat.Events
{
    public class AmmorBrokenEvent
    {
        public int AmmorDamage { get; } // 护甲伤害
        public int HPDamage { get; } // 生命值伤害

        public Character Source { get; } // 伤害来源
        public Character Target { get; } // 伤害目标

        public AmmorBrokenEvent(int ammorDamage, int hpDamage, Character source, Character target)
        {
            AmmorDamage = ammorDamage;
            HPDamage = hpDamage;
            Source = source;
            Target = target;
        }
    }
}