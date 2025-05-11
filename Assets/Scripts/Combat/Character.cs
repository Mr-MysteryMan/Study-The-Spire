using System;
using System.Collections.Generic;
using Combat.Buffs;
using Combat.Command;
using Combat.Command.Buff;
using Combat.Events.Turn;
using Combat.EventVariable;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Combat
{

    // 角色类，表示游戏中的角色，包含生命值、护甲值等属性
    public class Character : MonoBehaviour
    {
        public GameObject HPbar;

        // 注意使用了ReactiveIntVariable之后，一次函数尽量只有一次赋值，避免触发多次事件。
        [SerializeField] private ReactiveIntVariable maxHp;
        [SerializeField] private ReactiveIntVariable curHp;
        [SerializeField] private ReactiveIntVariable ammor;

        [SerializeField] private int initMaxHp = 10; // 初始最大生命值
        [SerializeField] private int initCurHp = 10; // 初始当前生命值
        [SerializeField] private int initAmmor = 0; // 初始护甲值

        // 角色的最大生命值
        public int MaxHp => maxHp.Value;

        // 角色的当前生命值
        public int CurHp => curHp.Value;

        // 角色的护甲值
        public int Ammor => ammor.Value;

        public CombatSystem combatSystem;

        [SerializeField] public BuffManager buffManager;

        public void SetInitHP(int maxHp, int curHp) // 设置初始化生命值数据
        {
            initCurHp = curHp;
            initMaxHp = maxHp;
        }

        protected virtual void Init(CombatSystem combatSystem)
        {
            var eventManager = combatSystem.EventManager;
            if (buffManager == null) return;
            buffManager.Init(combatSystem);

            maxHp = new ReactiveIntVariable("MaxHp", initMaxHp, eventManager, this);
            curHp = new ReactiveIntVariable("CurHp", initCurHp, eventManager, this);
            ammor = new ReactiveIntVariable("Ammor", initAmmor, eventManager, this);
        }

        public void Start()
        {
            Init(combatSystem);
            if (HPbar != null) // 存在血条时(非系统对象), 启动血条
            {
                HPController hPController = HPbar.GetComponent<HPController>();
                hPController.eventManager = this.combatSystem.GetComponent<EventManager>();
                hPController.launch(this);
            }
        }


        // 攻击target,造成damage点伤害，会触发相应事件
        public void Attack(Character target, int damage)
        {
            combatSystem.ProcessCommand(new AttackCommand(this, target, damage, DamageType.Normal));
        }

        // 为target加血，会触发相应事件
        public void Heal(Character target, int heal)
        {
            combatSystem.ProcessCommand(new HealCommand(this, target, heal));
        }

        // 为自己加血，会触发相应事件
        public void Heal(int heal)
        {
            combatSystem.ProcessCommand(new HealCommand(this, this, heal));
        }

        // 为target添加护甲值，会触发相应事件
        public void AddAmmor(Character character, int ammor)
        {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, character, ammor));
        }

        // 为自己添加护甲值，会触发相应事件
        public void AddAmmor(int ammor)
        {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, this, ammor));
        }

        public void AddBuff(Character target, IBuff buff, int count = 1)
        {
            combatSystem.ProcessCommand(new ApplyBuffCommand(this, target, buff, count));
        }

        public void DecreaseBuff(Character target, IBuff buff, int count = 1)
        {
            combatSystem.ProcessCommand(new ApplyBuffCommand(this, target, buff, -count));
        }

        public void RemoveBuff(Character target, Type type)
        {
            combatSystem.ProcessCommand(new RemoveBuffCommand(this, target, type));
        }

        // 当前生命值减少，只有简单的数值保证(damage>=0)
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _TakeHpDamage(int damage)
        {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            curHp.Value = Math.Max(curHp.Value - damage, 0);
        }

        // 当前护甲值减少，只有简单的数值保证
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _TakeAmmorDamage(int damage)
        {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            ammor.Value = Math.Max(ammor.Value - damage, 0);
        }

        // 当前生命值增加，只有简单的数值保证(<maxHp)
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _Heal(int heal)
        {
            Assert.IsTrue(heal >= 0, "Heal cannot be negative.");
            curHp.Value = Math.Min(curHp.Value + heal, maxHp.Value);
        }

        // 当前护甲值增加，只有简单的数值保证(>0)
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _AddAmmor(int ammor)
        {
            Assert.IsTrue(ammor >= 0, "Ammor cannot be negative.");
            this.ammor.Value += ammor;
        }

        // 最大生命值修改，不会同时回血
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _AddMaxHp(int maxHp)
        {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp.Value += maxHp;
        }

        // 最大生命值较少，修改后血量不会超过上限
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _MinusMaxHp(int maxHp)
        {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp.Value = Math.Max(this.maxHp.Value - maxHp, 0);
            if (curHp.Value > this.maxHp.Value)
            {
                curHp.Value = this.maxHp.Value;
            }
        }

        // 最大生命值修改，修改后血量不会超过上限
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _SetMaxHp(int maxHp)
        {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp.Value = maxHp;
            if (curHp.Value > this.maxHp.Value)
            {
                curHp.Value = this.maxHp.Value;
            }
        }

        internal void _ApplyBuff(IBuff buff, int count)
        {
            var _buff = buffManager.GetBuff(buff.GetType());
            if (_buff != null)
            {
                buff.OnUpdate(buff, count);
                return;
            }
            buff.OnApply(this, count);
            buffManager.AddBuff(buff);
        }

        internal void _UpdateBuffCount(Type type, int count)
        {
            var _buff = buffManager.GetBuff(type);
            if (_buff != null)
            {
                _buff.OnUpdate(count);
            }
            else
            {
                Debug.LogError("找不到buff, 无法更新buff的层数");
            }
        }

        internal void _RemoveBuff(Type type)
        {
            var _buff = buffManager.GetBuff(type);
            if (_buff != null)
            {
                _buff.OnRemove();
                buffManager.RemoveBuff(_buff);
            }
        }

        public bool HasBuff<T>() where T : IBuff
        {
            return buffManager.HasBuff<T>();
        }

        public T GetBuff<T>() where T : IBuff
        {
            return buffManager.GetBuff<T>();
        }

        public IBuff GetBuff(Type type)
        {
            return buffManager.GetBuff(type);
        }

        /* 战斗内触发函数 */
        public virtual void OnCombatStart() { }
        public virtual void OnCombatEnd() { }
        public virtual void OnTurnStart() { }
        public virtual void OnTurnEnd() { }

        /// <summary>
        /// 角色死亡前要调用的方法，主要是用作亡语效果的触发。
        /// 清理工作请在OnDestroy中完成。
        /// </summary>
        public virtual void BeforeDeath() { }
    }
}