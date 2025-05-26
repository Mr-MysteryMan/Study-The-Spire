using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyAttack")]
    public class EnemyAttack : TypedEffectSOBase
    {
        public int Damage;

        public override EnemyEffectType EffectType => EnemyEffectType.Attack;
        public override CardEffectTarget TargetType => CardEffectTarget.AdventurerOne;
        
        public override void Work(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.Attack(target, Damage); // 执行攻击
            }
        }
    }
}