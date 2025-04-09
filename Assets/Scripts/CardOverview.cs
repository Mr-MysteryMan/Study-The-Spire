using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardOverview : MonoBehaviour
{
    public GameObject CardPrefab; // 卡片预制件
    public GameObject Background;

    public GameObject Dialog; // 卡片池

    public List<Card> myCards;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    
    //生成卡片
    private void CreateCards()
    {
        const int cardCount = 10; // 创建的卡片数量
        List<string> cardStrings = new List<string> {"A","2","3","4","5","6","7","8","9","10","J","Q","K"}; // 示例卡片文本

        for (int i = 0; i < cardCount;i++) {
            // 从cardStrings随机选择一个字符串
            int randomIndex = Random.Range(0, cardStrings.Count);
            string cardString = cardStrings[randomIndex];

            // 在Dialog中实例化卡片
            GameObject cardObject = Instantiate(CardPrefab, Dialog.transform);
            cardObject.transform.localPosition = GetCardPosition(i, cardCount); // 设置卡片位置
            cardObject.transform.localRotation = Quaternion.Euler(GetCardRotation(i, cardCount)); // 设置卡片旋转
            cardObject.GetComponent<Card>().SetCardText(cardString); // 设置卡片文本
            // 随机设置卡片是否弃牌
            if (Random.Range(0, 2) == 0)
            {
                cardObject.GetComponent<Card>().setDiscard(); // 设置为弃牌
            }
        }
    }

    // 按照dialog大小,获取水平均匀排成一排时卡片位置
    private Vector3 GetCardPosition(int index, int total)
    {
        float dialogWidth = Dialog.GetComponent<RectTransform>().rect.width;
        float cardHeight = CardPrefab.GetComponent<RectTransform>().rect.height;
        double distance = dialogWidth * 0.8 / total; // 每个卡片分到位置的宽度

        double delta = index - total / 2.0 + 0.5;
        float x = (float)(distance * delta); // 卡片的x坐标
        float y = (float)(cardHeight * (delta/total) * (delta/total)); // 卡片的y坐标
        y = y > 0 ? -y : y;

        return new Vector3(x, y, 0);
    }

    private Vector3 GetCardRotation(int index, int total)
    {
        float z = (float)(-index + total / 2.0 + 0.5);
        return new Vector3(0, 0, z);
    }
}
