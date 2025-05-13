using System;
using Combat.Command;
using Combat.Processor;

namespace Combat.Buffs.TurnBuff
{
    public class Vulnerable : EndTurnDecreaseBuffBase<Vulnerable>
    {
        public override string Name => "Vulnerable";

        protected override string EffectDescription => $"受到的伤害增加{(int)(DamageIncreaseRate * 100)}%。";

        public override BuffType BuffType => BuffType.Debuff;

        public float DamageIncreaseRate;

        public Vulnerable()
        {
            DamageIncreaseRate = 0.5f;
        }
    }

    public class VulnerableProcessor : IDamageProcessor
    {
        public int Priority => 20;

        public int TimeStamp => 0;

        public Type CommandType => typeof(DamageType);

        public ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Target;

        public void Process(ref AttackCommand command)
        {
            if (command.Target.HasBuff<Vulnerable>())
            {
                var buff = command.Target.GetBuff<Vulnerable>();
                if (buff != null)
                {
                    command.BaseDamage += (int)(command.BaseDamage * buff.DamageIncreaseRate);
                }
            }
        }
    }
}