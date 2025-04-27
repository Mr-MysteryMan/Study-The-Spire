using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyAttack")]
    public class EnemyAttack : TypedEffectSOBase
    {
        public int Damage;

        public override EnemyEffectType EffectType => EnemyEffectType.Attack;
        public override void Work(Character source, Character target)
        {
            source.Attack(target, Damage);
        }
    }
}