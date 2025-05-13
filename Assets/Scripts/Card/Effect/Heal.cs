using Combat;
using Cards.CardEffect;

class HealEffect : IEffect
{
    private int health; // 生命值
    public HealEffect(int health)
    {
        this.health = health;
    }

    public void Work(Character survivor, Character enemy)
    {
        survivor.Heal(health);
    }
}