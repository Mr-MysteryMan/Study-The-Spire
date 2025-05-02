using Combat.CardEffect;

namespace Combat.Characters.EnemyEffect
{
    public enum EnemyEffectType {
        None,
        Attack,
        
        Defend,
        Buff,
        Debuff,
    }

    public interface ITypedEffect : IEffect
    {
        EnemyEffectType EffectType { get; }
    }
}