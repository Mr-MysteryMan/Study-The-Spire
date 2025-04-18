namespace Combat.Processor.Damage
{
    public interface IDamageProcessor
    {
        int Priority { get; } // 优先级

        void Process(ref AttackCommand context); // 处理伤害数据
    }
}