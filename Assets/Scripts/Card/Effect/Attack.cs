using Combat;
using Cards.CardEffect;
using System.Collections.Generic;
using System.Collections;

namespace Cards.CardEffect
{
    class AttackEffect : IEffect, ISyncEffect
    {
        private int damage; // 攻击伤害
        public AttackEffect(int damage)
        {
            this.damage = damage;
        }
        public IEnumerator Work(Character from, List<Character> to)
        {
            yield return from.vfxManager.PlayAttackForward();
            WorkSync(from, to); // 执行攻击
            yield return from.vfxManager.PlayAttackBack();
        }

        public void WorkSync(Character from, List<Character> to)
        {
            foreach (var target in to)
            {
                from.Attack(target, damage); // 执行攻击
            }
        }
    }
}