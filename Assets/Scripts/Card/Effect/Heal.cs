using Combat;

class HealEffect:CardEffect {
  private int health; // 生命值
  public HealEffect(int health) {
    this.health = health;
  }
  public void work(Character survivor, Character enemy) {
    survivor.Heal(health);
  }
}