using System.Security.Cryptography;
using Combat.Command;
using Combat.Events.Turn;

namespace Combat.Buffs.TurnBuff
{
    public class Poison : EndTrunEffectBuffBase<Poison>
    {
        public override string Name => "Poision";

        public override BuffType BuffType => BuffType.Debuff;

        protected override string EndTurnEffectDescription => $"扣除{Count}点血量";

        protected override void Effect(TurnEndEvent e)
        {
            if (e.Character != this.Parent) return;
            var damage = Count;
            e.Character.combatSystem.ProcessCommand(new AttackCommand(null, e.Character, damage, DamageType.Poison));
        }
    }
}