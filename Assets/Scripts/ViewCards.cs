using System.Collections.Generic;
using UnityEngine;

public class ViewCards : MonoBehaviour
{
    public GameObject CardOverViewPrefab; // 卡片预制件
    public void OpenCardOverview()
    {
        // 创建卡片预览窗口
        GameObject cardOverview = Instantiate(CardOverViewPrefab);
        CardOverview obj = cardOverview.GetComponent<CardOverview>();
        obj.CreateCards(randomCardData()); // 生成卡片
    }


    // TODO对接卡牌管理逻辑
    private List<CardData> randomCardData() { // 测试用随机卡片数据
        const int cardCount = 20; // 卡片数量
        List<CardData> cardDatas = new List<CardData>();

        for (int i = 0; i < cardCount; i++)
        {
            // 随机生成卡片数据
            string content = Random.Range(0, 100).ToString(); // 随机卡片内容
            bool isDiscarded = Random.Range(0, 2) == 0; // 随机弃牌状态
            cardDatas.Add(new CardData(content, isDiscarded));
        }

        return cardDatas;
    }
}
