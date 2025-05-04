using UnityEngine;

namespace Combat.CardEffect.EffectSO
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Combat/Effect/Attack")]
    public class Attact : EffectSOBase
    {
        public int damage;
        public override void Work(Character source, Character target)
        {
            // 执行治疗效果
            target.Attack(target, damage);
        }
    }
}