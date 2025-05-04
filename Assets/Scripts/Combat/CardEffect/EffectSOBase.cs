using UnityEngine;

namespace Combat.CardEffect
{
    public abstract class EffectSOBase : ScriptableObject, IEffect
    {
        public abstract void Work(Character source, Character target);
    }
}