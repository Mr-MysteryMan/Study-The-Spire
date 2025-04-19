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
    Debug.Log("GetCardBackground called with cardType: " + cardType);
    switch (cardType) {
      case CardType.Attack:
      Debug.Log("AttackCardBg" + AttackCardBg.name);
        return AttackCardBg;
      case CardType.Defense:
      Debug.Log("DefenseCardBg" + DefenseCardBg.name);
        return DefenseCardBg;
      case CardType.Heal:
      Debug.Log("HealCardBg" + HealCardBg.name);
        return HealCardBg;
      default:
      Debug.Log("BlankCardBg" + BlankCardBg.name);
        return BlankCardBg;
    }
  }
}