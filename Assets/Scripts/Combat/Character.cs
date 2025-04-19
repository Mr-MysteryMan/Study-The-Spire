using System;
using Combat.Command;
using UnityEngine;
using UnityEngine.Assertions;

namespace Combat {
    public class Character : MonoBehaviour
    {
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int curHp = 100;
        [SerializeField] private int ammor = 0;

        public int MaxHp => maxHp;

        public int CurHp => curHp;

        public int Ammor => ammor;


        [SerializeField] private CombatSystem combatSystem;

        public void Attack(Character target, int damage) {
            combatSystem.ProcessCommand(new AttackCommand(this, target, damage, DamageType.Normal));
        }

        public void Heal(Character target, int heal) {
            combatSystem.ProcessCommand(new HealCommand(this, target, heal));
        }

        public void Heal(int heal) {
            combatSystem.ProcessCommand(new HealCommand(this, this, heal));
        }

        public void AddAmmor(Character character, int ammor) {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, character, ammor));
        }

        public void AddAmmor(int ammor) {
            combatSystem.ProcessCommand(new AddAmmorCommand(this, this, ammor));
        }


        internal void _TakeHpDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            curHp -= damage;
            if (curHp < 0) {
                curHp = 0;
            }
        }

        internal void _TakeAmmorDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            ammor -= damage;
            if (ammor < 0) {
                ammor = 0;
            }
        }

        internal void _Heal(int heal)
        {
            Assert.IsTrue(heal >= 0, "Heal cannot be negative.");
            curHp += heal;
            if (curHp > maxHp) {
                curHp = maxHp;
            }
        }

        internal void _AddAmmor(int ammor)
        {
            Assert.IsTrue(ammor >= 0, "Ammor cannot be negative.");
            this.ammor += ammor;
        }

        internal void _AddMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp += maxHp;
        }

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

        internal void _SetMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp = maxHp;
            if (curHp > this.maxHp) {
                curHp = this.maxHp;
            }
        }

    }
}