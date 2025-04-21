using Type = System.Type;
using Combat.Command;
using System;
using UnityEngine;

namespace Combat.Processor.Rules {

    [CreateAssetMenu(fileName = "AmmorBasicRule", menuName = "Combat/Processor/Rules/AmmorBasicRule")]
    public class AmmorBasicRule : ScriptableProcessor<AddAmmorCommand> {
        public static int MaxAmmor = 999;

        // 优先级极低，用于保证最后执行。
        [SerializeField] private int _priority = 99999;
        [SerializeField] private int _timeStamp;

        public override int Priority => _priority;
        public override int TimeStamp => _timeStamp;

        public override Type CommandType => typeof(AddAmmorCommand);

        public override ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Target;

        public override void Process(ref AddAmmorCommand context) {

            // 限定护甲值的增加为正，且增加后不超过最大护甲值
            context.AmmorAmount = Math.Clamp(context.AmmorAmount, 0, MaxAmmor - context.Target.Ammor);
        }
    }
}

