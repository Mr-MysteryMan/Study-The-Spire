using Combat.Events;
using Combat.Events.Turn;
using Combat.EventVariable;

namespace Combat.Buffs.TurnBuff
{
    public abstract class TurnBuffBase : IBuff
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract BuffType BuffType { get; }

        private ReactiveIntVariable _countVariable;

        public int Count { get => _countVariable.Value; private set => _countVariable.Value = value; }

        public Character Parent { get; private set; }

        protected CombatSystem _combatSystem;

        public bool IsAvaliable(int count)
        {
            return count > 0;
        }

        public bool IsAvaliable()
        {
            return Count > 0;
        }

        public virtual void OnApply(Character target, int count = 1)
        {
            this._combatSystem = target.combatSystem;
            if (_combatSystem == null) return;
            this.Parent = target;
            _countVariable = new ReactiveIntVariable(BuffConstants.ReactiveVariableName, count, _combatSystem.EventManager, this);
            _combatSystem.EventManager.Subscribe<TurnStartEvent>(OnTurnStart);
            _combatSystem.EventManager.Subscribe<TurnEndEvent>(OnTurnEnd);
            _combatSystem.EventManager.Subscribe<CombatWinningEvent>(OnCombatEnd);
        }

        public virtual void OnRemove()
        {
            if (_combatSystem == null) return;
            _combatSystem.EventManager.Unsubscribe<TurnStartEvent>(OnTurnStart);
            _combatSystem.EventManager.Unsubscribe<TurnEndEvent>(OnTurnEnd);
            _combatSystem.EventManager.Unsubscribe<CombatWinningEvent>(OnCombatEnd);
        }

        public virtual void OnUpdate(int count)
        {
            if (count == Count) return;
            Count = count;
        }

        public void OnUpdate(IBuff buff, int count)
        {
            OnUpdate(count);
        }

        protected abstract void OnTurnStart(TurnStartEvent e);
        protected abstract void OnTurnEnd(TurnEndEvent e);
        protected abstract void OnCombatEnd(CombatWinningEvent e);
    }
}