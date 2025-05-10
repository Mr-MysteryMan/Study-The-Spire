using System.Collections.Generic;

namespace Combat.Trigger
{
    public class TriggerLib {

        private List<ITrigger> triggers;

        public IEnumerable<ITrigger> GetTriggers() {
            if (triggers == null) {
                Init();
            }
            return triggers;
        }

        private void Init() {
            triggers = new List<ITrigger>() {
                new AddAmmorTrigger(),
                new AmmorBrokenTrigger(),
                new CharacterDeathTrigger(),
                new DamageDealtTrigger(),
                new HealDealtTrigger(),
            };
        }
    }
}