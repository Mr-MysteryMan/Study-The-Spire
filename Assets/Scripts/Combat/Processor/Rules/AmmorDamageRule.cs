using Type = System.Type;

using Combat.Command;
using System;
using UnityEngine;

namespace Combat.Processor.Rules
{
    [CreateAssetMenu(fileName = "AmmorDamageRule", menuName = "Combat/Processor/Rules/AmmorDamageRule")]
    public class AmmorDamageRule : ScriptableProcessor<AttackCommand>
    {
        [SerializeField] private int _priority = 0;
        [SerializeField] private int _timeStamp = 0;

        public override int Priority => _priority;

        public override int TimeStamp => _timeStamp;

        public override Type CommandType => typeof(AttackCommand);

        public override ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Target;
        public override void Process(ref AttackCommand context)
        {
            if (context.BaseDamage > 0 && context.Type == DamageType.Normal) {
                int ammorDamage = Math.Min(context.BaseDamage, context.Target.Ammor);
                context.AmmorDamage = ammorDamage; // 计算护甲伤害
                context.HPDamage = context.BaseDamage - ammorDamage; // 计算生命值伤害
                context.FinalDamage = context.HPDamage; // 最终伤害等于生命值伤害
            }
        }
    }
} 