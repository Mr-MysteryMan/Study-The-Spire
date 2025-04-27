class HealEffect:CardEffect {
  private int health; // 生命值
  public HealEffect(int health) {
    this.health = health;
  }
  public void work(Survivor survivor, Enemy enemy) {
    survivor.heal(health);
  }
}