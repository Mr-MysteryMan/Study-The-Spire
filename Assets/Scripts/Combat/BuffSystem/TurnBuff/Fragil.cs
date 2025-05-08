using System;
using Combat.Command;
using Combat.Processor;

namespace Combat.Buffs.TurnBuff
{
    public class Fragil : EndTurnDecreaseBuffBase<Fragil>
    {
        public override string Name => "Fragil";

        protected override string EffectDescription => $"受到脆弱的影响，获得的护甲减少{(int)(ArmorDecreaseRate * 100)}%。";

        public override BuffType BuffType => BuffType.Debuff;

        public float ArmorDecreaseRate;

        public Fragil()
        {
            ArmorDecreaseRate = 0.25f;
        }
    }

    public class FragilProcessor : IAddAmmorProcessor
    {
        public int Priority => 10;

        public int TimeStamp => 0;

        public Type CommandType => typeof(AddAmmorCommand);

        public ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Source;

        public void Process(ref AddAmmorCommand command)
        {
            if (command.Source.HasBuff<Fragil>())
            {
                var buff = command.Source.GetBuff<Fragil>();
                if (buff != null)
                {
                    command.AmmorAmount -= (int)(command.AmmorAmount * buff.ArmorDecreaseRate);
                }
            }
        }
    }
}