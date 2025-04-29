using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "TypeToIconLib", menuName = "ScriptableObjects/Combat/EnemyEffect/TypeToIconLib", order = 1)]
    public class TypeToIconLib : ScriptableObject {
        public Sprite NoneIcon;
        public Sprite AttackIcon;
        public Sprite DefendIcon;
        public Sprite BuffIcon;
        public Sprite DebuffIcon;

        public Sprite GetIcon(EnemyEffectType effectType) {
            return effectType switch {
                EnemyEffectType.None => NoneIcon,
                EnemyEffectType.Attack => AttackIcon,
                EnemyEffectType.Defend => DefendIcon,
                EnemyEffectType.Buff => BuffIcon,
                EnemyEffectType.Debuff => DebuffIcon,
                _ => NoneIcon
            };
        }
    }
}