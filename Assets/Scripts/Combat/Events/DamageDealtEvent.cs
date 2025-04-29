namespace Combat.Events
{
    public class DamageDealtEvent
    {
        public int AmmorDamage { get; } // 护甲伤害
        public int HPDamage { get; } // 生命值伤害
        public DamageType DamageType { get; } // 伤害类型
        public Character Source { get; } // 伤害来源
        public Character Target { get; } // 伤害目标

        public DamageDealtEvent(int ammorDamage, int hpDamage, DamageType damageType, Character source, Character target)
        {
            AmmorDamage = ammorDamage;
            HPDamage = hpDamage;
            DamageType = damageType;
            Source = source;
            Target = target;
        }
    }
}