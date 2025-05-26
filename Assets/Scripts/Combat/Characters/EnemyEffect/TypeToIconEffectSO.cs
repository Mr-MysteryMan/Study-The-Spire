using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "TypeToIconEffectSO", menuName = "ScriptableObjects/Combat/EnemyEffect/TypeToIconEffectSO")]
    public class TypeToIconEffectSO : ScriptableObject, IEnemyEffect
    {
        public TypedEffectSOBase Effect;

        public TypeToIconLib iconLib;

        public Sprite Icon => iconLib.GetIcon(Effect.EffectType);

        public EnemyEffectType EffectType => Effect.EffectType;
        public CardEffectTarget TargetType => Effect.TargetType;

        public void Work(Character source, List<Character> targets)
        {
            Effect.Work(source, targets);
        }
    }
}