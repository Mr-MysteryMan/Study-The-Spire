using Type = System.Type;
using Combat.Command;
using Unity.Mathematics;
namespace Combat.Processor.Rules { 
    public class AmmorBasicRule : IAddAmmorProcessor {
        public static int MaxAmmor = 999;

        // 优先级极低，用于保证最后执行。
        public int Priority => 99999;
        public int TimeStamp => 0;

        public Type CommandType => typeof(AddAmmorCommand);

        public void Process(ref AddAmmorCommand context) {

            // 限定护甲值的增加为正，且增加后不超过最大护甲值
            context.AmmorAmount = math.clamp(context.AmmorAmount, 0, MaxAmmor - context.Target.Ammor);
        }
    }
}

