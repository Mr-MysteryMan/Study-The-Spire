using Combat;
using Cards.CardEffect;
using System.Collections.Generic;

class HealEffect : IEffect
{
    private int health; // 生命值
    public HealEffect(int health)
    {
        this.health = health;
    }

    public void Work(Character survivor, List<Character> enemys)
    {
        survivor.Heal(health); // 执行治疗
    }
}