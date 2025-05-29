using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.CardEffect;
using Combat;
using UnityEngine;

namespace Cards.CardEffect
{
    class AmplifyCardEffect : IEffect, ISyncEffect
    {
        private float amplifyAmount;
        private Func<List<ICardData>, List<ICardData>> cardFilter;

        public AmplifyCardEffect(float amplifyAmount, Func<List<ICardData>, List<ICardData>> cardFilter)
        {
            this.amplifyAmount = amplifyAmount;
            this.cardFilter = cardFilter;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log($"Amplify effect triggered by {source.name} with amount {amplifyAmount}");
            WorkSync(source, targets);
            yield return new WaitForSeconds(0.2f);
        }

        public void WorkSync(Character source, List<Character> targets)
        {
            cardFilter(source.combatSystem.CardManager.HandCardData)
                .ForEach(card => card.Modify(amplifyAmount, ModifyType.All));
        }
    }

    static class AmplifyEffectFactory
    {
        public static IEffect AmplifyRandomCard(float amplifyAmount)
        {
            return new AmplifyCardEffect(amplifyAmount,
                cards =>
                {
                    var randomIndex = UnityEngine.Random.Range(0, cards.Count);
                    return new List<ICardData> { cards[randomIndex] }; // 随机选择一张卡
                }
            );
        }

        public static IEffect AmplifyAllCards(float amplifyAmount)
        {
            return new AmplifyCardEffect(amplifyAmount,
                cards => cards); // 对所有卡片生效
        }

        public static IEffect AmplifyCardsByType(float amplifyAmount, CardCategory type)
        {
            return new AmplifyCardEffect(amplifyAmount,
                cards => cards.FindAll(card => card.CardCategory == type)); // 对指定类型的卡片生效
        }
    }

}