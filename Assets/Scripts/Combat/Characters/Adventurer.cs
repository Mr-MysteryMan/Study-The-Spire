using Combat;
using Combat.Events;
using Combat.EventVariable;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace Combat.Characters
{
    /// <summary>
    /// 角色类，表示游戏中的冒险者角色，继承自Character类，主要添加了法力值。
    /// 注意CardManager里也直接管理了法力值，如果不需要每个人一个法力值，下面的代码都没有什么用。
    /// </summary>
    public class Adventurer : Character
    {
        // 回合开始时的法力值
        private ReactiveIntVariable turnMana;
        private ReactiveIntVariable mana;

        [SerializeField] private int initTurnMana = 0; // 初始法力值

        public int Mana => combatSystem.CardManager.EnergyPoint; // 目前直接以CardManager的法力值作为角色的法力值。
        public int TurnMana => turnMana.Value;

        protected override void Init(CombatSystem combatSystem)
        {
            base.Init(combatSystem);
            var eventManager = combatSystem.EventManager;
            turnMana = new ReactiveIntVariable("TurnMana", initTurnMana, eventManager, this);
            mana = new ReactiveIntVariable("Mana", 0, eventManager, this);
        }

        public void SetInitMana(int mana) // 设置初始化法力值数据
        {
            initTurnMana = mana;
        }

        internal void SetMana(int mana)
        {
            this.mana.Value = mana;
        }

        internal void UseMana(int mana)
        {
            if (this.mana.Value < mana)
            {
                // 也许法力值可以透支，Debug显示一下
                Debug.LogError("法力值不足！");
            }
            this.mana.Value -= mana;
        }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            this.mana.Value = turnMana.Value; // 回合开始时恢复法力值
        }
    }
}