using Combat;

class AttackEffect:CardEffect {
  private int damage; // 攻击伤害
  private Character target; // 目标角色
  public AttackEffect(int damage) {
    this.damage = damage;
  }
  public void work(Character from, Character to) {
    from.Attack(to, damage); // 调用角色的攻击方法
  }
}