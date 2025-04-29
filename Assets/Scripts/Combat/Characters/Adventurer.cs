using Combat;
using UnityEngine;
namespace Combat.Characters
{
    public class Adventurer : Character
    {
        // 回合开始时的法力值
        [SerializeField] private int turnMana = 3;
        [SerializeField] private int mana = 3;

        public int Mana => mana;
        public int TurnMana => turnMana;

        internal void SetMana(int mana) {
            this.mana = mana;
        }

        internal void UseMana(int mana) {
            if (this.mana < mana) {
                // 也许法力值可以透支，Debug显示一下
                Debug.LogError("法力值不足！");
            }
            this.mana -= mana;
        }

        public override void OnTurnStart() {
            base.OnTurnStart();
            this.mana = turnMana; // 回合开始时恢复法力值
        }
    }
}