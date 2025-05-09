using Cards.CardEffect;
using Cards.CardEffect.Buffs;
using Combat.Buffs.PermanentBuff;
using Combat.Buffs.TurnBuff;

namespace Cards.CardDatas
{
    public class GainStrength : CardData
    {
        public GainStrength(int cardValue, int CardCost) : base(CardType.Heal, cardValue, CardCost, new AddBuffEffect<Strength>(new Strength(), cardValue)) { }
    }

    public class ApplyWeak : CardData
    {
        public ApplyWeak(int cardValue, int CardCost) : base(CardType.Attack, cardValue, CardCost, new AddBuffEffect<Weak>(new Weak(), cardValue)) { }
    }

    public class ApplyVulnerable : CardData
    {
        public ApplyVulnerable(int cardValue, int CardCost) : base(CardType.Attack, cardValue, CardCost, new AddBuffEffect<Vulnerable>(new Vulnerable(), cardValue)) { }
    }

    public class ApplyFragil : CardData
    {
        public ApplyFragil(int cardValue, int CardCost) : base(CardType.Attack, cardValue, CardCost, new AddBuffEffect<Fragil>(new Fragil(), cardValue)) { }
    }

    public class ApplyPoison : CardData
    {
        public ApplyPoison(int cardValue, int CardCost) : base(CardType.Attack, cardValue, CardCost, new AddBuffEffect<Poison>(new Poison(), cardValue)) { }
    }
}