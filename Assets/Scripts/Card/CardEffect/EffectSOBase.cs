using UnityEngine;
using Combat;

namespace Cards.CardEffect
{
    public abstract class EffectSOBase : ScriptableObject, IEffect
    {
        public abstract void Work(Character source, Character target);
    }
}