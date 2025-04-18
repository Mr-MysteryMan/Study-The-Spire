namespace Combat.Events
{
    public class DamageDealtEvent
    {
        public int FinalDamage { get; } // 伤害值
        public int AmmorDamage { get; } // 护甲伤害
        public int HPDamage { get; } // 生命值伤害
        public DamageType DamageType { get; } // 伤害类型
        public Character Source { get; } // 伤害来源
        public Character Target { get; } // 伤害目标

        public DamageDealtEvent(int finalDamage, int ammorDamage, int hpDamage, DamageType damageType, Character source, Character target)
        {
            FinalDamage = finalDamage;
            AmmorDamage = ammorDamage;
            HPDamage = hpDamage;
            DamageType = damageType;
            Source = source;
            Target = target;
        }
    }
}