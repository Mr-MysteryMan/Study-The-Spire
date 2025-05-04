using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "TypeToIconEffectSO", menuName = "ScriptableObjects/Combat/EnemyEffect/TypeToIconEffectSO")]
    public class TypeToIconEffectSO : ScriptableObject, IEnemyEffect {
        public TypedEffectSOBase Effect;

        public TypeToIconLib iconLib;

        public Sprite Icon => iconLib.GetIcon(Effect.EffectType);

        public EnemyEffectType EffectType => Effect.EffectType;

        public void Work(Character source, Character target)
        {
            Effect.Work(source, target);
        }
    }
}