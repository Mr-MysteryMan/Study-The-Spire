using Combat;

class DefenseEffect:CardEffect {
  private int Defense; // 防御数值
  public DefenseEffect(int Defense) {
    this.Defense = Defense;
  }
  public void work(Character survivor, Character enemy) {
    survivor.AddAmmor(Defense);
  }
}