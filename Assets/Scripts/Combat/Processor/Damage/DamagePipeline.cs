using System.Collections.Generic;

namespace Combat.Processor.Damage
{
    public class DamagePipeline
    {
        private List<IDamageProcessor> _damageProcessors = new List<IDamageProcessor>();

        public void AddDamageProcessor(IDamageProcessor processor)
        {
            _damageProcessors.Add(processor);
            _damageProcessors.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        public void RemoveDamageProcessor(IDamageProcessor processor)
        {
            _damageProcessors.Remove(processor);
        }

        public void Process(ref AttackCommand context)
        {
            foreach (var processor in _damageProcessors)
            {
                processor.Process(ref context);
            }
        }
    }
}