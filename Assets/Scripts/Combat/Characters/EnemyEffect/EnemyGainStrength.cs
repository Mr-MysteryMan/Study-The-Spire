using System.Collections.Generic;
using Combat.Buffs.PermanentBuff;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyGainStrength", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyGainStrength")]
    public class EnemyGainStrength : TypedEffectSOBase
    {
        public int StrengthAmount;
        public override EnemyEffectType EffectType => EnemyEffectType.Buff;
        public override CardEffectTarget TargetType => CardEffectTarget.EnemyOne;

        public override void Work(Character source, List<Character> targets)
        {
            foreach (var target in targets)
            {
                source.AddBuff(target, new Strength(), StrengthAmount);
            }
        }
    }
}