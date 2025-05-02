using UnityEngine;

namespace Combat.CardEffect.EffectSO
{
    [CreateAssetMenu(fileName = "Heal", menuName = "Combat/Effect/Heal")]
    public class Heal : EffectSOBase
    {
        public int healAmount;

        public override void Work(Character source, Character target)
        {
            // 执行治疗效果
            target.Heal(target, healAmount);
        }
    }
}