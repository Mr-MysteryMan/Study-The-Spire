using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 卡牌管理器：负责管理玩家所有卡牌数据，包括增删查与金币管理功能
public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    // 存储所有卡牌数据
    private List<CardData> allCards;

    // 玩家金币
    public int Gold { get; private set; } = 100;

    public int health { get; set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
       
        Instance = this;
        DontDestroyOnLoad(gameObject); // 防止切换场景丢失
        
        allCards =randomCardData(); 
    }

    public static List<CardData> randomCardData(int cardCount = 20) { // 测试用随机卡片数据
            List<CardData> cardData = new List<CardData>();

            for (int i = 0; i < cardCount; i++)
            {
                // 随机生成卡片数据
                int cardType = Random.Range(0, 3); // 随机卡片类型
                CardType type = (CardType)cardType; // 转换为枚举类型
                int value = Random.Range(0, 100); // 随机卡片内容
                int cost = Random.Range(1, 5); // 随机卡片费用
                cardData.Add(new CardData(type,value,cost)); // 随机生成卡片数据
            }

            return cardData;
    }

    // 设置金币数
    public void SetGold(int amount)
    {
        Gold = Mathf.Max(0, amount);
        Debug.Log($"设置金币为：{Gold}");
    }

    // 增加金币
    public void AddGold(int amount)
    {
        Gold += Mathf.Max(0, amount);
        Debug.Log($"获得金币：+{amount}，当前金币：{Gold}");
    }

    // 消耗金币，返回是否成功
    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            Debug.Log($"花费金币：-{amount}，剩余金币：{Gold}");
            return true;
        }
        else
        {
            Debug.LogWarning($"金币不足，尝试消费 {amount}，当前金币：{Gold}");
            return false;
        }
    }

    // 添加一张卡牌到背包
    public void AddCard(CardData card)
    {
        allCards.Add(card);
        Debug.Log($"添加卡牌：{card.cardType} {card.cardValue}");
    }

    // 根据卡牌 ID 移除卡牌
    public void RemoveCard(int cardId)
    {
        var card = allCards.Find(c => c.cardId == cardId);
        if (card != null)
        {
            allCards.Remove(card);
            Debug.Log($"移除卡牌 ID：{cardId}");
        }
        else
        {
            Debug.LogWarning($"未找到要移除的卡牌 ID：{cardId}");
        }
    }

    // 获取所有卡牌
    public List<CardData> GetAllCards()
    {
        return new List<CardData>(allCards);
    }

    // 根据类型获取卡牌（攻击、防御、治疗）
    public List<CardData> GetCardsByType(CardType type)
    {
        PrintAllCards();
        return allCards.Where(c => c.cardType == type).ToList();
    }

    // 清空所有卡牌（仅用于调试或重置）
    public void ClearAllCards()
    {
        allCards.Clear();
        Debug.Log("清空了所有卡牌");
    }

    // 示例方法：将所有卡牌信息输出到控制台
    public void PrintAllCards()
    {
        Debug.Log($"当前卡牌总数：{allCards.Count}");
        foreach (var card in allCards)
        {
            Debug.Log($"ID: {card.cardId} 类型: {card.cardType} 数值: {card.cardValue} 弃牌: {card.isDiscarded}");
        }
    }
}
