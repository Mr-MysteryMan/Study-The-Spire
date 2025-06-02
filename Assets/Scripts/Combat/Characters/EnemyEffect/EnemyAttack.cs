using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyAttack")]
    public class EnemyAttack : TypedEffectSOBase
    {
        public int Damage;

        public override EnemyEffectType EffectType => EnemyEffectType.Attack;
        public override CardEffectTarget TargetType => CardEffectTarget.AllyOne;

        public override IEnumerator Work(Character source, List<Character> targets)
        {
            yield return source.vfxManager.PlayAttackForward(); // 播放攻击前特效
            foreach (var target in targets)
            {
                source.Attack(target, Damage); // 执行攻击
            }
            yield return source.vfxManager.PlayAttackBack(); // 播放攻击后特效
        }
    }
}