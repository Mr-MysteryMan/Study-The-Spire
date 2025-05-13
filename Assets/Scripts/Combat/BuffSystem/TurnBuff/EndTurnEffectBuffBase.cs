using Combat.Events.Turn;
using UnityEngine;

namespace Combat.Buffs.TurnBuff
{
    public abstract class EndTrunEffectBuffBase<T> : EndTurnDecreaseBuffBase<T> where T : IBuff
    {
        public override string Description => EffectDescription + "，并减少一层。";

        protected override string EffectDescription => "回合结束时，" + EndTurnEffectDescription;

        protected abstract string EndTurnEffectDescription { get; }

        protected abstract void Effect(TurnEndEvent e);

        protected override void OnTurnEnd(TurnEndEvent e)
        {
            if (e.Character != this.Parent) return;
            Effect(e);
            base.OnTurnEnd(e);
        }
    }
}