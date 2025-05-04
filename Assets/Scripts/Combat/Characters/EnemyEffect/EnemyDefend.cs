using UnityEngine;

namespace Combat.Characters.EnemyEffect
{
    [CreateAssetMenu(fileName = "EnemyDefend", menuName = "ScriptableObjects/Combat/EnemyEffect/EnemyDefend")]
    public class EnemyDefend : TypedEffectSOBase
    {
        public int AmmorAmount;

        public override EnemyEffectType EffectType => EnemyEffectType.Defend;

        public override void Work(Character source, Character target)
        {
            source.AddAmmor(target, AmmorAmount);
        }
    }
}