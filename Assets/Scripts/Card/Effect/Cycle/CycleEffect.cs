using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Cards.CardEffect.Cycle
{
    public class DrawCardEffect : IEffect
    {
        private int cardAmount;

        public DrawCardEffect(int cardAmount)
        {
            this.cardAmount = cardAmount;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            // 执行抽牌逻辑
            source.combatSystem.ProcessCommand(new Combat.Command.Cards.DrawCardCommand(source, source, cardAmount));
            yield return new WaitForSeconds(0.2f);
        }
    }

    public class DiscardRandomCardEffect : IEffect
    {
        private int cardAmount;

        public DiscardRandomCardEffect(int cardAmount)
        {
            this.cardAmount = cardAmount;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            // 执行随机弃牌逻辑
            List<ICardData> cards = source.combatSystem.CardManager.GetCardStack(Combat.CardManager.CardStackType.Hand);
            if (cards.Count < cardAmount)
            {
                cardAmount = cards.Count; // 如果手牌不足，调整弃牌数量
            }
            if (cardAmount <= 0)
            {
                cardAmount = 0;
            }
            List<ICardData> randomCards = new List<ICardData>();
            List<ICardData> tempCards = new List<ICardData>(cards);
            for (int i = 0; i < cardAmount; i++)
            {
                if (tempCards.Count == 0) break;
                int idx = Random.Range(0, tempCards.Count);
                randomCards.Add(tempCards[idx]);
                tempCards.RemoveAt(idx);
            }
            source.combatSystem.ProcessCommand(new Combat.Command.Cards.DiscardCardCommand(source, source, randomCards));
            yield return new WaitForSeconds(0.2f);
        }
    }

    public class AddEnergyEffect : IEffect
    {
        private int energyAmount;

        public AddEnergyEffect(int energyAmount)
        {
            this.energyAmount = energyAmount;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            // 执行增加能量逻辑
            source.combatSystem.CardManager.AddEnergy(energyAmount);
            yield return new WaitForSeconds(0.2f);
        }
    }
}