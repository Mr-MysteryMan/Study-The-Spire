using Type = System.Type;
using Combat.Command;
using Unity.Mathematics;
namespace Combat.Processor.Rules { 
    public class HealBasicRule : IHealProcessor {
        public static int MaxAmmor = 999;

        public int Priority => 99999;
        public int TimeStamp => 0;

        public Type CommandType => typeof(HealCommand);

        public void Process(ref HealCommand context) {
            // 限定治疗值的增加为正，且增加后不超过角色的最大生命值
            context.HealAmount = math.clamp(context.HealAmount, 0, context.Target.MaxHp - context.Target.CurHp);
        }
    }
}

