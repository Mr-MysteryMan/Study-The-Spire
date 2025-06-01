using Combat.Command;

namespace Combat.Events
{
    public class BeforeAttackEvent
    {
        public Character Attacker { get; } // 攻击者
        public Character Target { get; } // 目标
        public AttackCommand AttackCommand { get; } // 攻击指令

        public BeforeAttackEvent(Character attacker, Character target, AttackCommand command)
        {
            Attacker = attacker;
            Target = target;
            AttackCommand = command;
        }
    }
}