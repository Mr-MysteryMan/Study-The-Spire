using System.Collections.Generic;
using UnityEngine;

namespace Combat.Deck {
    public class CardManager : MonoBehaviour {
        public List<CardData> AllCards;

        // 抽牌堆
        public List<CardData> DrawPile;
        // 手牌
        public List<CardData> Hand;
        // 弃牌堆
        public List<CardData> DiscardPile;

        // 结算区
        public List<CardData> SettlementZone;

        // 抽一张牌
        public void DrawACardFromDrawPile() {
            if (DrawPile.Count > 0) {
                CardData card = DrawPile[0];
                DrawPile.RemoveAt(0);
                Hand.Add(card);
            } else {
                Debug.Log("抽牌堆已空，无法抽牌！");
            }
        }

        public void DrawCard(int count) {
            for (int i = 0; i < count; i++) {
                if (DrawPile.Count == 0 && DiscardPile.Count > 0) {
                    MoveDiscardToDrawPile();
                }
                if (DrawPile.Count == 0 && DiscardPile.Count == 0) {
                    Debug.Log("抽牌堆和弃牌堆已空，无法抽牌！");
                    break;
                } else {
                    DrawACardFromDrawPile();
                }
            }
        }

        public void DiscardCard(CardData card) {
            if (Hand.Contains(card)) {
                Hand.Remove(card);
                DiscardPile.Add(card);
            } else {
                Debug.Log("手牌中没有该卡片，无法弃牌！");
            }
        }

        public void DiscardAllCards() {
            foreach (CardData card in Hand) {
                DiscardPile.Add(card);
            }
            Hand.Clear();
        }

        public void ShuffleDrawPile() {
            for (int i = 0; i < DrawPile.Count; i++) {
                CardData temp = DrawPile[i];
                int randomIndex = Random.Range(i, DrawPile.Count);
                DrawPile[i] = DrawPile[randomIndex];
                DrawPile[randomIndex] = temp;
            }
        }

        public void MoveDiscardToDrawPile() {
            foreach (CardData card in DiscardPile) {
                DrawPile.Add(card);
            }
            DiscardPile.Clear();
            ShuffleDrawPile();
        }


    }
}