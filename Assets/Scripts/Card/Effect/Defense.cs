class DefenseEffect:CardEffect {
  private int Defense; // 防御数值
  public DefenseEffect(int Defense) {
    this.Defense = Defense;
  }
  public void work(Survivor survivor, Enemy enemy) {
    survivor.defend(Defense);
  }
}