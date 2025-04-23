using UnityEngine;

public static class CardUI {
  private static Sprite BlankCardBg; // 空白卡片背景
  private static Sprite AttackCardBg;
  private static Sprite DefenseCardBg;
  private static Sprite HealCardBg;

  static CardUI() {
    BlankCardBg = Resources.Load<Sprite>("CardUI/BlankCard");
    AttackCardBg = Resources.Load<Sprite>("CardUI/AttackCard");
    DefenseCardBg = Resources.Load<Sprite>("CardUI/DefenseCard");
    HealCardBg = Resources.Load<Sprite>("CardUI/HealCard");
    Debug.Log("CardUI initialized");
  }

  public static Sprite GetCardBackground(CardType cardType) {
    switch (cardType) {
      case CardType.Attack:
        return AttackCardBg;
      case CardType.Defense:
        return DefenseCardBg;
      case CardType.Heal:
        return HealCardBg;
      default:
        return BlankCardBg;
    }
  }
}