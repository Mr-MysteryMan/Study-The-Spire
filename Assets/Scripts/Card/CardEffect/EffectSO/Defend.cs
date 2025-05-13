using System;
using UnityEngine;
using Combat;

namespace Cards.CardEffect.EffectSO
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