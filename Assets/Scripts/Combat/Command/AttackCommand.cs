using Combat.Command;

namespace Combat.Command
{
    public struct AttackCommand : ICommand
    {
        public Character Source {get; set;}
        public Character Target {get; set;}
        public int BaseDamage; // 基础伤害
        public DamageType Type;
        public int FinalDamage; // 最终伤害
        public int AmmorDamage; // 护甲伤害
        public int HPDamage; // 生命值伤害

        public AttackCommand(Character attacker, Character target, int baseDamage, DamageType type)
        {
            Source = attacker;
            Target = target;
            BaseDamage = baseDamage;
            Type = type;
            FinalDamage = 0;
            AmmorDamage = 0;
            HPDamage = 0;
        }

        public void Execute() {
            Target._TakeAmmorDamage(AmmorDamage);
            Target._TakeHpDamage(HPDamage);
        }
    }
}