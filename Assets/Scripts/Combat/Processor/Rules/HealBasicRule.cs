using Type = System.Type;
using Combat.Command;
using System;
using UnityEngine;

namespace Combat.Processor.Rules { 

    [CreateAssetMenu(fileName = "HealBasicRule", menuName = "Combat/Processor/Rules/HealBasicRule")]
    public class HealBasicRule : ScriptableProcessor<HealCommand> {
        [SerializeField] private int _maxAmmor = 999;
        [SerializeField] private int _priority = 99999;
        [SerializeField] private int _timeStamp = 0;

        // 仅当目标者匹配时，才能处理该命令
        public override ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Target;

        public int MaxAmmor => _maxAmmor;
        public override int Priority => _priority;
        public override int TimeStamp => _timeStamp;

        public override Type CommandType => typeof(HealCommand);

        public override void Process(ref HealCommand context) {
            // 限定治疗值的增加为正，且增加后不超过角色的最大生命值
            context.HealAmount = Math.Clamp(context.HealAmount, 0, context.Target.MaxHp - context.Target.CurHp);
        }
    }
}

