using Combat;
using Cards.CardEffect;
class AttackEffect : IEffect
{
    private int damage; // 攻击伤害
    public AttackEffect(int damage)
    {
        this.damage = damage;
    }
    public void Work(Character from, Character to)
    {
        from.Attack(to, damage); // 调用角色的攻击方法
    }
}