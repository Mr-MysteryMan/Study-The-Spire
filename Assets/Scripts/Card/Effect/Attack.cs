class AttackEffect:CardEffect {
  private int damage; // 攻击伤害
  private Character target; // 目标角色
  public AttackEffect(int damage) {
    this.damage = damage;
  }
  public void work(Survivor survivor, Enemy enemy) {
    survivor.attack(target,damage);
    enemy.getAttacked(damage);
  }
}