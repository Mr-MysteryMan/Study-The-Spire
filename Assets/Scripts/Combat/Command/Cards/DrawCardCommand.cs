using System.Collections.Generic;

namespace Combat.Command.Cards
{
    public struct DrawCardCommand : ICommand
    {
        public Character Source { get; set; }
        public Character Target { get; set; }

        public int CardAmount;

        public DrawCardCommand(Character source, Character target, int cardAmount)
        {
            Source = source;
            Target = target;
            CardAmount = cardAmount;
        }

        public void Execute()
        {
            Source.combatSystem.CardManager.drewCard(CardAmount);
        }
    }

    public struct DiscardCardCommand : ICommand
    {
        public Character Source { get; set; }
        public Character Target { get; set; }

        public List<ICardData> cardDatas;

        public DiscardCardCommand(Character source, Character target, List<ICardData> cardDatas)
        {
            Source = source;
            Target = target;
            this.cardDatas = cardDatas;
        }

        public void Execute()
        {
            Source.combatSystem.CardManager.DiscardCards(cardDatas);
        }
    }
}