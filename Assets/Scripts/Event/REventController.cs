using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cards.CardDatas;
using Cards;

public class REventController : MonoBehaviour
{
    public static REventController Instance { get; private set; }
    private CardManager cardManager;
    //private List<ICardData> allCards;
    //public int gold { get; private set; } = manager.gold;
    //public int health { get; set; } = manager.health;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 防止切换场景丢失
        cardManager = CardManager.Instance;
    }

    public void alterMoney(int amount)
    {
        if (cardManager.gold + amount >= 0)
        {
            cardManager.gold += amount;
            Debug.Log($"变化金币：{amount}，当前金币：{cardManager.gold}");
        }
        else
        {
            cardManager.gold = 0;
            Debug.Log($"失去全部金币");
        }
    }

    public void alterBlood(int amount)
    {
        if (cardManager.health + amount >= 0)
        {
            cardManager.health = Mathf.Min(100, cardManager.health + amount);
            Debug.Log($"变化血量：{amount}，当前血量：{cardManager.health}");
        }
        else
        {
            cardManager.health = 0;
            Debug.Log($"失去全部血量");
            //ObjectEventSO loadMenuEvent;
            //loadMenuEvent.RaiseEvent(null, this);
        }
    }
}