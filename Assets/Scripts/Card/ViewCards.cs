using System.Collections.Generic;
using UnityEngine;

public class ViewCards : MonoBehaviour
{
    public GameObject CardOverViewPrefab; // 卡片预制件

    public GameObject CardManager; // 卡片管理
    public void OpenCardOverview()
    {
        // 创建卡片预览窗口
        GameObject cardOverview = Instantiate(CardOverViewPrefab);
        CardOverview obj = cardOverview.GetComponent<CardOverview>();

        if (CardManager) {
            Combat.CardManager cardManager = CardManager.GetComponent<Combat.CardManager>();
            obj.CreateCards(cardManager.NewCardData, 0); // 根据卡片管理器生成卡片
            obj.CreateCards(cardManager.HandCardData, 1); // 根据卡片管理器生成卡片
            obj.CreateCards(cardManager.DiscardCardData, 2); // 根据卡片管理器生成卡片
        } else {
            obj.CreateCards(randomCardData()); // 生成随机卡片
        }
    }


    // TODO对接卡牌管理逻辑
    public static List<CardData> randomCardData(int cardCount = 20) { // 测试用随机卡片数据
        List<CardData> cardDatas = new List<CardData>();

        for (int i = 0; i < cardCount; i++)
        {
            // 随机生成卡片数据
            int cardType = Random.Range(0, 3); // 随机卡片类型
            CardType type = (CardType)cardType; // 转换为枚举类型
            int value = Random.Range(0, 100); // 随机卡片内容
            cardDatas.Add(new CardData(type,value)); // 随机生成卡片数据
        }

        return cardDatas;
    }
}
