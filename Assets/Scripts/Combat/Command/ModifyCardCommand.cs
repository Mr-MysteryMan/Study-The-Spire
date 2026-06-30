using System;
using System.Collections.Generic;
using static Combat.CardManager;

namespace Combat.Command
{
    public struct ModifyCardCommand : ICommand
    {
        public Character Source { get; set; }
        public Character Target => null;

        public ICardData CardData => cardData;
        private ICardData cardData;
        public float ModifyFactor;

        public ModifyType Type;
        public ModifySubType SubType;

        public ModifyCardCommand(Character source, ICardData cardData, float factor, ModifyType type, CardManager.ModifySubType subType)
        {
            Source = source;
            this.cardData = cardData;
            ModifyFactor = factor;
            Type = type;
            SubType = subType;
        }

        public void Execute()
        {
            Source.combatSystem.CardManager.ModifyCard(cardData, ModifyFactor, Type, SubType);
        }
    }
}