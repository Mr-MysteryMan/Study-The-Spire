using Type = System.Type;
using Unity.Mathematics;

using Combat.Command;
namespace Combat.Processor.Rules
{
    public class AmmorDamageRule : IDamageProcessor
    {
        public int Priority => 0;

        public int TimeStamp => 0;

        public Type CommandType => typeof(AttackCommand);
        public void Process(ref AttackCommand context)
        {
            if (context.BaseDamage > 0 && context.Type == DamageType.Normal) {
                int ammorDamage = math.min(context.BaseDamage, context.Target.Ammor);
                context.AmmorDamage = ammorDamage; // 计算护甲伤害
                context.HPDamage = context.BaseDamage - ammorDamage; // 计算生命值伤害
                context.FinalDamage = context.HPDamage; // 最终伤害等于生命值伤害
            }
        }
    }
} 