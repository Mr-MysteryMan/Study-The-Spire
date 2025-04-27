using System.Collections.Generic;
using Combat;
using Combat.Characters;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject CardPrefab; // 卡片预制件
    public GameObject CardDeck; // 手牌区

    private List<GameObject> cards = new List<GameObject>(); // 卡片列表

    private Character player; // 玩家角色
    private List<Enemy> enemy; // 敌人角色

    public List<CardData> NewCardData = new List<CardData>(); // 新卡片数据列表
    public List<CardData> HandCardData = new List<CardData>(); // 手牌数据列表
    public List<CardData> DiscardCardData = new List<CardData>(); // 弃牌数据列表

    public void init()
    {
        // TODO: 接入背包系统, 暂用随机生成的卡片
        this.NewCardData = ViewCards.randomCardData(30);
        // 所有卡片置为非弃置
        foreach (var cardData in NewCardData)
        {
            cardData.Reset();
        }
    }
    // 抽卡
    public void drewCard(int num = -1) {
        if (num <= 0) {
            num = Setting.RoundGetCardNum; // 使用默认数量
        }
        for (int i = 0; i < num; i++)
        {
            if (NewCardData.Count == 0) // 如果没有新卡片了
            {
                if (DiscardCardData.Count > 0) // 如果有弃牌
                {
                    NewCardData.AddRange(DiscardCardData); // 将弃牌添加到新卡片列表
                    DiscardCardData.Clear(); // 清空弃牌列表
                } else {
                    break; // 退出循环
                }
            }
            // 从新卡片数据列表中随机抽取一张卡片
            var index = Random.Range(0, NewCardData.Count);
            CardData cardData = NewCardData[index];
            NewCardData.RemoveAt(index);

            CreateCard(cardData); // 创建卡片
            HandCardData.Add(cardData); // 添加到手牌数据列表
        }
        // 更新卡片位置
        updateCardPosition();
    }

    public void discardCard(Card card) {
        // 从手牌中找到对应的卡片
        int index = HandCardData.FindIndex(c => c.cardId == card.cardId);
        if (index != -1)
        {
            HandCardData[index].Discard(); // 将卡片标记为弃置
            DiscardCardData.Add(HandCardData[index]); // 将卡片添加到弃牌数据列表
            HandCardData.RemoveAt(index); // 从手牌数据列表中移除已弃掉的卡片
        }
        // 销毁卡片对象
        DestroyImmediate(card.cardObj); // 销毁卡片对象
    }

    public void discardAll() {
        // 将所有手牌添加到弃牌数据列表
        DiscardCardData.AddRange(HandCardData);
        HandCardData.Clear(); // 清空手牌数据列表
        // 清空卡片列表
        foreach (var card in cards)
        {
            Destroy(card); // 销毁卡片对象
        }
        updateCardPosition(); // 更新卡片位置
    }

    public void addCharacter(Character player, List<Enemy> enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

    public Character getUser(Card card) {
        return this.player;
    }
    public Character getTarget(Card card) {
        return this.enemy[0];
    }

    public void reportUse(Card card) {
        discardCard(card); // 弃掉使用的卡片
        updateCardPosition(); // 更新卡片位置
    }

    private void CreateCard(CardData cardData) {
        GameObject CardObj = Instantiate(CardPrefab, CardDeck.transform);
        Card card = CardObj.GetComponent<Card>();
        card.updateCardStatus(cardData); // 更新卡片状态
        card.addManager(this); // 添加卡片管理器
        cards.Add(CardObj); // 添加卡片到列表
    }

    public void updateCardPosition() {
        // 移除cards中的null
        cards.RemoveAll(card => card == null);
        // 将所有卡片在手牌区排成一排
        // 每张牌偏移手牌区中心距离为: (手牌区宽度 - 卡片宽度) / 2 / ((卡片数量 - 1) / 2)
        if (cards.Count <= 1) {
            if (cards.Count <= 0) return; // 如果没有卡片, 则不更新位置
            cards[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // 如果只有一张牌, 则放在中间
            return;
        }
        float cardWidth = CardPrefab.GetComponent<RectTransform>().rect.width;
        float cardDeckWidth = CardDeck.GetComponent<RectTransform>().rect.width;
        float offset = (cardDeckWidth - cardWidth) / 2 / ((cards.Count - 1f) / 2);
        for (int i = 0; i < cards.Count; i++)
        {
            float x = (i - (cards.Count - 1f) / 2) * offset;
            cards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
        }
    }
}
