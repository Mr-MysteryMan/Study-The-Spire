using Combat;
using Cards.CardEffect;
using System.Collections.Generic;
class AttackEffect : IEffect
{
    private int damage; // 攻击伤害
    public AttackEffect(int damage)
    {
        this.damage = damage;
    }
    public void Work(Character from, List<Character> to)
    {
        foreach (var target in to)
        {
            from.Attack(target, damage); // 执行攻击
        }
    }
}