using Combat.Command.Buff;
using Combat.Events;
using Combat.Events.Turn;

namespace Combat.Buffs.TurnBuff
{
    public abstract class EndTurnDecreaseBuffBase<T> : TurnBuffBase where T : IBuff
    {
        public override string Description => this.EffectDescription + "回合结束时，减少一层。";

        protected abstract string EffectDescription { get; }

        override protected void OnTurnStart(TurnStartEvent e)
        {
            // Do nothing
        }

        protected override void OnTurnEnd(TurnEndEvent e)
        {
            if (e.Character != this.Parent) return;
            _combatSystem.ProcessCommand(new UpdateBuffCountCommand<T>(null, Parent, -1));
        }

        protected override void OnCombatEnd(CombatWinningEvent e)
        {
            // Do nothing
        }
    }
}