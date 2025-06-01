namespace Combat
{
    // 伤害类型，用于区分不同类型的伤害（在processor中判断）
    // 目前仅有普通伤害，后续可以扩展为多种类型的伤害
    public enum DamageType
    {
        Normal,
        Actual, // 真实伤害，直接扣除生命值，不受护甲影响
        Poison, // 中毒伤害，直接扣除生命值
    }
}