using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.CardEffect;
using UnityEngine;
using Cards.Modifier;
using CardStackType = Combat.CardManager.CardStackType;
using Combat;
using Cards.CardEffect.Auxiliary;

namespace Cards.CardEffect
{
    namespace Auxiliary
    {
        public static class CardSourceExtensions
        {
            public static List<ICardData> Apply(this List<ICardData> cards, Func<List<ICardData>, List<ICardData>> filter)
            {
                return filter(cards);
            }
        }
    }
    class AmplifyCardEffect : IEffect, ISyncEffect
    {
        private float amplifyAmount;
        private CardStackType cardSource;
        private Func<List<ICardData>, List<ICardData>> cardFilter;

        public AmplifyCardEffect(float amplifyAmount, CardStackType source, Func<List<ICardData>, List<ICardData>> cardFilter)
        {
            this.amplifyAmount = amplifyAmount;
            this.cardFilter = cardFilter;
            this.cardSource = source;
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
            List<ICardData> cards =
                source.combatSystem.CardManager.GetCardStack(cardSource)
                    .Where(card => card.IsModifiable()) // 只选择可修改的卡片
                    .ToList()
                    .Apply(cardFilter); // 应用过滤器
            foreach (var card in cards)
            {
                source.ModifyCard(card, amplifyAmount);
            }
        }
    }

    static class AmplifyEffectFactory
    {
        public static IEffect AmplifyRandomHandCard(float amplifyAmount)
        {
            return new AmplifyCardEffect(amplifyAmount, CardStackType.Hand,
                cards =>
                {
                    var randomIndex = UnityEngine.Random.Range(0, cards.Count);
                    return new List<ICardData> { cards[randomIndex] }; // 随机选择一张卡
                }
            );
        }

        public static IEffect AmplifyAllHandCards(float amplifyAmount)
        {
            return new AmplifyCardEffect(amplifyAmount, CardStackType.Hand,
                cards => cards); // 对所有卡片生效
        }

        public static IEffect AmplifyAllCardsByType(float amplifyAmount, CardCategory type)
        {
            return new AmplifyCardEffect(amplifyAmount, CardStackType.All,
                cards => cards.FindAll(card => card.CardCategory == type)); // 对指定类型的卡片生效
        }
    }

}