using Unity.Mathematics;

public class AmmorDamageRule : IDamageProcessor
{
    public int Priority => 0;

    public void Process(ref AttackCommand context)
    {
        if (context.BaseDamage > 0) {
            int ammorDamage = math.min(context.BaseDamage, context.Target.CurAmmor);
            context.AmmorDamage = ammorDamage; // 计算护甲伤害
            context.HPDamage = context.BaseDamage - ammorDamage; // 计算生命值伤害
            context.FinalDamage = context.HPDamage; // 最终伤害等于生命值伤害
        }
    }
}