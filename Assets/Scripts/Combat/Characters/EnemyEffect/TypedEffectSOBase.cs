using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    public abstract class TypedEffectSOBase : ScriptableObject, ITypedEffect
    {
        public abstract EnemyEffectType EffectType { get; }
        public abstract CardEffectTarget TargetType { get; }

        public abstract IEnumerator Work(Character source, List<Character> targets);
    }
}