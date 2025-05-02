using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyHeal", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyHeal")]
    public class EnemyHeal : TypedEffectSOBase
    {
        public int HealAmount;
        public override EnemyEffectType EffectType => EnemyEffectType.Buff;
        public override void Work(Character source, Character target)
        {
            source.Heal(target, HealAmount);
        }
    }
}