using System.Collections.Generic;
using UnityEngine;

namespace Combat.Processor.Rules
{
    [CreateAssetMenu(fileName = "BasicRulesLibSO", menuName = "Combat/Processor/Rules/BasicRulesLibSO")]
    public class BasicRulesLibSO : ScriptableObject {
        [SerializeField] public List<ScriptableProcessor> Rules;
    }
}