using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardOverview : MonoBehaviour
{
    [Header("卡片预制件")]
    public GameObject CardPrefab; // 卡片预制件

    [Header("自身元素绑定")]
    public GameObject CardOverviewDialog; // 自身游戏元素
    public GameObject Canvas; // 画布元素

    public GameObject Dialog; // 卡片池

    public List<Card> myCards;

    private List<int> verticalOffsets = new List<int>{400,0,-400}; // 竖直偏移量, 设置共三行
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //生成卡片
    public void CreateCards(List<ICardData> cardDatas, int line = 1)
    {
        int cardCount = cardDatas.Count; // 卡片数量
        int verticalOffset = verticalOffsets[line]; // 竖直偏移量

        for (int i = 0; i < cardCount;i++) {
            ICardData cardData = cardDatas[i]; // 获取卡片数据
            // 在Dialog中实例化卡片
            GameObject cardObject = Instantiate(CardPrefab, Dialog.transform);
            cardObject.transform.localPosition = GetCardPosition(i, cardCount, verticalOffset); // 设置卡片位置
            cardObject.transform.localRotation = Quaternion.Euler(GetCardRotation(i, cardCount)); // 设置卡片旋转
            cardObject.GetComponent<Card>().updateCardStatus(cardData); // 更新卡片状态
        }
    }

    // 按照dialog大小,获取水平均匀排成一排时卡片位置
    private Vector3 GetCardPosition(int index, int total,float verticalOffset)
    {
        float dialogWidth = Dialog.GetComponent<RectTransform>().rect.width;
        float cardHeight = CardPrefab.GetComponent<RectTransform>().rect.height;
        double distance = dialogWidth * 0.8 / total; // 每个卡片分到位置的宽度

        double delta = index - total / 2.0 + 0.5;
        float x = (float)(distance * delta); // 卡片的x坐标
        float y = (float)(cardHeight * (delta/total) * (delta/total)); // 卡片的y坐标
        y = y > 0 ? -y : y;

        return new Vector3(x, y + verticalOffset, 0);
    }

    private Vector3 GetCardRotation(int index, int total)
    {
        float z = (float)(-index + total / 2.0 + 0.5);
        return new Vector3(0, 0, z);
    }

    public void  closeDialog() {
        Destroy(CardOverviewDialog);
    }
}
