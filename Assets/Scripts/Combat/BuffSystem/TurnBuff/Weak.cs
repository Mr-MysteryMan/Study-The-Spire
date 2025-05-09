using System;
using Combat.Command;
using Combat.Processor;

namespace Combat.Buffs.TurnBuff
{
    public class Weak : EndTurnDecreaseBuffBase<Weak>
    {
        public override string Name => "Weak";

        protected override string EffectDescription => $"受到虚弱的影响，受到的伤害减少{(int)(DamageDecreaseRate * 100)}%。";

        public override BuffType BuffType => BuffType.Debuff;

        public float DamageDecreaseRate;

        public Weak()
        {
            DamageDecreaseRate = 0.25f;
        }
    }

    public class WeakProcessor : IDamageProcessor
    {
        public int Priority => 10;

        public int TimeStamp => 0;

        public Type CommandType => typeof(DamageType);

        public ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Source;

        public void Process(ref AttackCommand command)
        {
            if (command.Source.HasBuff<Weak>())
            {
                var buff = command.Source.GetBuff<Weak>();
                if (buff != null)
                {
                    command.BaseDamage -= (int)(command.BaseDamage * buff.DamageDecreaseRate);
                }
            }
        }
    }
}