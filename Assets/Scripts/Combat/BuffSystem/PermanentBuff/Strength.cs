using System;
using Combat.Command;
using Combat.EventVariable;
using Combat.Processor;

namespace Combat.Buffs.PermanentBuff
{
    public class Strength : IBuff
    {
        public string Name => "Strength";
        public string Description => GetDescription();

        public int Count { get => _countVariable.Value; private set => _countVariable.Value = value; }

        private ReactiveIntVariable _countVariable;

        public BuffType BuffType => IsPositive ? BuffType.Buff : BuffType.Debuff;

        public Character Parent => parent;

        private Character parent;

        protected CombatSystem _combatSystem;

        public bool IsAvaliable(int count)
        {
            return count != 0;
        }

        private bool IsPositive => Count > 0;

        public void OnApply(Character character, int count = 1)
        {
            this._combatSystem = character.combatSystem;
            this._countVariable = new ReactiveIntVariable(BuffConstants.ReactiveVariableName, count, _combatSystem.EventManager, this);
        }

        public void OnRemove()
        {
        }

        public void OnUpdate(int count)
        {
            if (count == Count) return;
            this.Count = count;
        }


        private string GetDescription()
        {
            string desc = "攻击造成的伤害" + (IsPositive ? "提升" : "降低") + Math.Abs(Count).ToString() + "点。";
            return desc;
        }

        public void OnUpdate(IBuff buff, int count)
        {
            this.OnUpdate(count);
        }

        public bool IsAvaliable()
        {
            return Count != 0;
        }
    }

    public class StrengthProcessor : IProcessor<AttackCommand>
    {
        public int Priority => 5;

        public int TimeStamp => 0;

        public Type CommandType => typeof(AttackCommand);

        public ProcessorEffectSideType EffectSide => ProcessorEffectSideType.Source;

        public void Process(ref AttackCommand command)
        {
            if (command.Source == null || !command.Source.HasBuff<Strength>()) return;
            var buff = command.Source.GetBuff<Strength>();
            if (buff == null) return;
            int count = buff.Count;
            command.BaseDamage += count;
        }
    }
}