using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    public abstract class TypedEffectSOBase : ScriptableObject, ITypedEffect
    {
        public abstract EnemyEffectType EffectType { get; }

        public abstract void Work(Character source, Character target);
    }
}