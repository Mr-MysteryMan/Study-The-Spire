using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Buffs
{
    [RequireComponent(typeof(BuffListController))]
    public class BuffManager : MonoBehaviour
    {
        public List<IBuff> buffs;

        void Start()
        {
            buffs = new List<IBuff>();
            buffListController = GetComponent<BuffListController>();
        }

        [SerializeField] private BuffListController buffListController;

        public bool HasBuff<T>() where T : IBuff
        {
            return buffs.Exists(buff => buff is T);
        }

        public void AddBuff(IBuff buff)
        {
            buffs.Add(buff);
            buffListController.AddBuff(buff);
        }

        public void RemoveBuff(IBuff buff)
        {
            buffs.Remove(buff);
            buffListController.RemoveBuff(buff);
        }

        public void RemoveBuff<T>() where T : IBuff
        {
            foreach (var buff in buffs)
            {
                if (buff is T)
                {
                    buffListController.RemoveBuff(buff);
                }
            }

            buffs.RemoveAll(buff => buff is T);
        }

        public void ClearBuffs()
        {
            buffs.Clear();
            buffListController.ClearBuffs();
        }

        public T GetBuff<T>() where T : IBuff
        {
            return (T)buffs.Find(buff => buff is T);
        }

        public void Init(CombatSystem combatSystem)
        {
            this.buffListController.Init(combatSystem);
        }
    }
}