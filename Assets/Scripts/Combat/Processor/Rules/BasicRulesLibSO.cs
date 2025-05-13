using System.Collections.Generic;
using UnityEngine;

namespace Combat.Processor.Rules
{
    [CreateAssetMenu(fileName = "BasicRulesLibSO", menuName = "Combat/Processor/Rules/BasicRulesLibSO")]
    public class BasicRulesLibSO : ScriptableObject {
        [SerializeField] public List<ScriptableProcessor> Rules;

        private List<IProcessor> _rules;

        public List<IProcessor> GetRules() {
            if (_rules == null) {
                Init();
            }
            return _rules;
        }

        public void Init() {
            if (_rules == null) {
                _rules = new List<IProcessor>();
                foreach (var rule in Rules) {
                    var processor = rule as IProcessor;
                    _rules.Add(processor);
                }
                BasicRuleLib ruleLib = new BasicRuleLib();
                foreach (var rule in ruleLib.Rules) {
                    _rules.Add(rule);
                }
            }
        }
    }
}