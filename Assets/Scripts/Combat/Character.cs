using System;
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

        internal void TakeHpDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            curHp -= damage;
            if (curHp < 0) {
                curHp = 0;
            }
        }

        internal void TakeAmmorDamage(int damage) {
            Assert.IsTrue(damage >= 0, "Damage cannot be negative.");
            ammor -= damage;
            if (ammor < 0) {
                ammor = 0;
            }
        }

        internal void Heal(int heal)
        {
            Assert.IsTrue(heal >= 0, "Heal cannot be negative.");
            curHp += heal;
            if (curHp > maxHp) {
                curHp = maxHp;
            }
        }

        internal void AddAmmor(int ammor)
        {
            Assert.IsTrue(ammor >= 0, "Ammor cannot be negative.");
            this.ammor += ammor;
        }

        internal void AddMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp += maxHp;
        }

        internal void MinusMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp -= maxHp;
            if (this.maxHp < 0) {
                this.maxHp = 0;
            }
            if (curHp > this.maxHp) {
                curHp = this.maxHp;
            }
        }

        internal void SetMaxHp(int maxHp) {
            Assert.IsTrue(maxHp >= 0, "MaxHp cannot be negative.");
            this.maxHp = maxHp;
            if (curHp > this.maxHp) {
                curHp = this.maxHp;
            }
        }
    }
}