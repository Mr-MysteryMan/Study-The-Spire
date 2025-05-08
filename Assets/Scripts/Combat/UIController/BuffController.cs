using Combat.EventVariable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Combat.Buffs
{
    public class BuffController : MonoBehaviour
    {
        private IBuff buff;

        [SerializeField] private TMP_Text text;

        private CombatSystem combatSystem;

        public IBuff Buff => buff;

        public void Init(IBuff buff, CombatSystem combatSystem)
        {
            this.buff = buff;
            this.combatSystem = combatSystem;
            combatSystem.EventManager.Subscribe<ValueChangedEvent<int>>(OnUpdate);
        }

        void OnDestroy()
        {
            if (combatSystem == null || combatSystem.EventManager == null) return;
            this.combatSystem.EventManager.Unsubscribe<ValueChangedEvent<int>>(OnUpdate);
        }

        public void OnApply()
        {
            // 播放添加特效
            Debug.Log("添加Buff" + buff.Name);
            text.text = buff.Count.ToString();
        }

        public void OnRemove()
        {
            // 播放移除特效
            Debug.Log("移除Buff" + buff.Name);
        }

        public void OnUpdate(ValueChangedEvent<int> e)
        {
            if (e.ValueName != BuffConstants.ReactiveVariableName || buff == null || e.Parent != buff) return;
            text.text = buff.Count.ToString();
        }

        public void SetBuff(IBuff buff)
        {
            this.buff = buff;
        }
    }
}