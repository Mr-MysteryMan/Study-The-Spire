using System;
using UnityEngine;

namespace Combat.CardEffect.EffectSO
{
    [CreateAssetMenu(fileName = "Defend", menuName = "Combat/Effect/Defend")]
    public class Defend : EffectSOBase
    {
        public int ammorAmount;

        public override void Work(Character source, Character target)
        {
            // 执行治疗效果
            target.AddAmmor(target, ammorAmount);
        }
    }
}