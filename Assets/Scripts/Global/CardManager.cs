using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cards.CardDatas;
using Cards;

// 卡牌管理器：负责管理玩家所有卡牌数据，包括增删查与金币管理功能
public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    // 存储所有卡牌数据
    private List<ICardData> allCards;

    // 玩家金币
    public int gold { get; set; } = Setting.TreasureGoldNum;

    // 玩家血量
    public int health { get; set; } = Setting.PlayerHp;

    public int maxHealth { get; set; } = Setting.PlayerHp;

    // 选择
    public List<CharacterType> characterTypes { get; private set; } = new List<CharacterType>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 防止切换场景丢失

        allCards = LocalCards.GetCards();
    }

    // 设置金币数
    public void SetGold(int amount)
    {
        gold = Mathf.Max(0, amount);
        Debug.Log($"设置金币为：{gold}");
    }

    // 增加金币
    public void AddGold(int amount)
    {
        gold += Mathf.Max(0, amount);
        Debug.Log($"获得金币：+{amount}，当前金币：{gold}");
    }

    // 消耗金币，返回是否成功
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"花费金币：-{amount}，剩余金币：{gold}");
            return true;
        }
        else
        {
            Debug.LogWarning($"金币不足，尝试消费 {amount}，当前金币：{gold}");
            return false;
        }
    }

    public static List<ICardData> randomCardData(int cardCount = 20)
    { // 测试用随机卡片数据
        List<ICardData> cardData = new List<ICardData>();

        for (int i = 0; i < cardCount; i++)
        {
            // 随机生成卡片数据
            int cardType = Random.Range(0, 3); // 随机卡片类型
            CardType type = (CardType)cardType; // 转换为枚举类型
            int cost = Random.Range(1, 5); // 随机卡片费用
            int value = Random.Range((cost - 1) * 8, cost * 10);
            cardData.Add(new TypedCardData(type, value, cost)); // 随机生成卡片数据
        }

        return cardData;
    }

    // 添加一张卡牌到背包
    public void AddCard(ICardData card)
    {
        allCards.Add(card);
        Debug.Log($"添加卡牌：{card.GetShortInfo()}");
    }

    // 根据卡牌 ID 移除卡牌
    public void RemoveCard(ICardData cardData)
    {
        var card = allCards.Find(c => c == cardData);
        if (card != null)
        {
            allCards.Remove(card);
            Debug.Log($"移除卡牌 ID：{cardData.CardId}");
        }
        else
        {
            Debug.LogWarning($"未找到要移除的卡牌 ID：{cardData.CardId}");
        }
    }

    // 获取所有卡牌
    public List<ICardData> GetAllCards()
    {
        return new List<ICardData>(allCards);
    }

    // 根据类型获取卡牌（攻击、防御、治疗）
    public IEnumerable<ICardData> GetCardsByCardCategory(CardCategory type)
    {
        PrintAllCards();
        return allCards.Where(c => c.CardCategory == type);
    }

    public IEnumerable<ICardData> GetCards(System.Func<ICardData, bool> predicate)
    {
        return allCards.Where(predicate);
    }

    // 清空所有卡牌（仅用于调试或重置）
    public void ClearAllCards()
    {
        allCards.Clear();
        Debug.Log("清空了所有卡牌");
    }

    // 输出所有卡牌信息
    public void PrintAllCards()
    {
        Debug.Log($"当前卡牌总数：{allCards.Count}");
        foreach (var card in allCards)
        {
            Debug.Log(card.GetDebugInfo());
        }
    }
    
    public void SetCharacterTypes(List<CharacterType> types)
    {
        characterTypes = types;
    }

}
