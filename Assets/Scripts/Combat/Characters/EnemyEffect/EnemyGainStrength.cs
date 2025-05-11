using Combat.Buffs.PermanentBuff;
using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyGainStrength", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyGainStrength")]
    public class EnemyGainStrength : TypedEffectSOBase
    {
        public int StrengthAmount;
        public override EnemyEffectType EffectType => EnemyEffectType.Buff;
        public override void Work(Character source, Character target)
        {
            source.AddBuff(target, new Strength(), StrengthAmount);
        }
    }
}