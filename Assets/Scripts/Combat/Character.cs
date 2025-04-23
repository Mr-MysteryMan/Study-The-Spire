using System;
using Combat.Command;
using UnityEngine;
using UnityEngine.Assertions;

namespace Combat {

    // 角色类，表示游戏中的角色，包含生命值、护甲值等属性
    public class Character : MonoBehaviour
    {
        public GameObject HPbar;
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int curHp = 100;
        [SerializeField] private int ammor = 0;

        // 角色的最大生命值
        public int MaxHp => maxHp;

        // 角色的当前生命值
        public int CurHp => curHp;

        // 角色的护甲值
        public int Ammor => ammor;

        public CombatSystem combatSystem;

    public void SetHP(int maxHp,int curHp) // 设置当前生命值数据
    {
        this.curHp = curHp;
        this.maxHp = maxHp;
    }

    public void Start()
    {
        if (HPbar != null) // 存在血条时(非系统对象), 启动血条
        {
            HPController hPController = HPbar.GetComponent<HPController>();
            hPController.eventManager = this.combatSystem.GetComponent<EventManager>();
            hPController.launch();
        }
    }
        // 攻击target,造成damage点伤害，会触发相应事件
        public void Attack(Character target, int damage) {
            combatSystem.ProcessCommand(new AttackCommand(this, target, damage, DamageType.Normal));
        }

        // 为target加血，会触发相应事件
        public void Heal(Character target, int heal) {
            combatSystem.ProcessCommand(new HealCommand(this, target, heal));
        }

        // 为自己加血，会触发相应事件
        public void Heal(int heal) {
            combatSystem.ProcessCommand(new HealCommand(this, this, heal));
        }

        // 为target添加护甲值，会触发相应事件
        public void AddAmmor(Character character, int ammor) {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, character, ammor));
        }

        // 为自己添加护甲值，会触发相应事件
        public void AddAmmor(int ammor) {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, this, ammor));
        }

        // 当前生命值减少，只有简单的数值保证(damage>=0)，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _TakeHpDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            curHp -= damage;
            if (curHp < 0) {
                curHp = 0;
            }
        }

        // 当前护甲值减少，只有简单的数值保证，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _TakeAmmorDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            ammor -= damage;
            if (ammor < 0) {
                ammor = 0;
            }
        }

        // 当前生命值增加，只有简单的数值保证(<maxHp)，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _Heal(int heal)
        {
            Assert.IsTrue(heal >= 0, "Heal cannot be negative.");
            curHp += heal;
            if (curHp > maxHp) {
                curHp = maxHp;
            }
        }

        // 当前护甲值增加，只有简单的数值保证(>0)，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _AddAmmor(int ammor)
        {
            Assert.IsTrue(ammor >= 0, "Ammor cannot be negative.");
            this.ammor += ammor;
        }

        // 最大生命值修改，不会同时回血，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _AddMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp += maxHp;
        }

        // 最大生命值较少，修改后血量不会超过上限，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _MinusMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp -= maxHp;
            if (this.maxHp < 0) {
                this.maxHp = 0;
            }
            if (curHp > this.maxHp) {
                curHp = this.maxHp;
            }
        }

        // 最大生命值修改，修改后血量不会超过上限，不会触发事件
        // internal修饰，以防止外部调用，请只在command的execute中调用
        internal void _SetMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp = maxHp;
            if (curHp > this.maxHp) {
                curHp = this.maxHp;
            }
        }

    }
}