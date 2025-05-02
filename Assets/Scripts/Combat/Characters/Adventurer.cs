using Combat;
using Combat.EventVariable;
using UnityEngine;
namespace Combat.Characters
{
    public class Adventurer : Character
    {
        // 回合开始时的法力值
        private ReactiveIntVariable turnMana;
        private ReactiveIntVariable mana;

        [SerializeField] private int initTurnMana = 0; // 初始法力值

        public int Mana => mana.Value;
        public int TurnMana => turnMana.Value;

        protected override void Init(EventManager eventManager)
        {
            base.Init(eventManager);
            turnMana = new ReactiveIntVariable("TurnMana", initTurnMana, eventManager, this);
            mana = new ReactiveIntVariable("Mana", 0, eventManager, this);
        }

        public void SetInitMana(int mana) // 设置初始化法力值数据
        {
            initTurnMana = mana;
        }

        internal void SetMana(int mana) {
            this.mana.Value = mana;
        }

        internal void UseMana(int mana) {
            if (this.mana.Value < mana) {
                // 也许法力值可以透支，Debug显示一下
                Debug.LogError("法力值不足！");
            }
            this.mana.Value -= mana;
        }

        public override void OnTurnStart() {
            base.OnTurnStart();
            this.mana.Value = turnMana.Value; // 回合开始时恢复法力值
        }
    }
}