using System.Collections.Generic;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;

namespace Combat.Processor.Rules
{
    public class BasicRuleLib
    {
        public List<IProcessor> Rules;

        public BasicRuleLib()
        {
            Rules = new List<IProcessor>() {
                new VulnerableProcessor(),
                new WeakProcessor(),
                new FragilProcessor(),
                new StrengthProcessor()
            };
        }
    }
}