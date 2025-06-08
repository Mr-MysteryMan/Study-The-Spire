using System.Collections.Generic;
using Combat.Characters;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    // ----------------------游戏组件---------------------//
    public GameObject treasure; // 宝箱
    public GameObject panel; // 弹窗
    public Text GoldText; // 金币文本
    public GameObject CardPrefab; // 卡片预制件
    // ----------------------基本信息---------------------//
    private int goldNum; // 金币数
    private List<ICardData> cardDataList = new List<ICardData>(); // 卡片数据列表
    private static CardManager cardManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void init(List<Adventurer> infos, OnClosed onClosed)
    {
        if (cardManager == null) // 如果卡片管理器为空
            cardManager = CardManager.Instance; // 获取卡片管理器实例
        // 生成金币数
        int goldNumBase = Setting.TreasureGoldNum; // 设置金币数
        goldNum = (int)Random.Range(goldNumBase * 0.5f, goldNumBase * 1.5f); // 随机生成金币数
        GoldText.text = "+" + goldNum.ToString(); // 设置金币文本

        // 生成卡片
        int cardNumBase = Setting.TreasureCardNum; // 设置卡片数
        List<ICardData> cardDataList = CardManager.randomCardData(cardNumBase);
        for (int i = 0; i < cardNumBase; i++)
        {
            // 在panel上生成卡片
            GameObject cardObj = Instantiate(CardPrefab, panel.transform); // 实例化卡片
            // 设置卡片位置在panel上排成一排
            float panelWidth = panel.GetComponent<RectTransform>().rect.width; // 获取面板宽度
            float panelHeight = panel.GetComponent<RectTransform>().rect.height; // 获取面板高度
            float x = (i - cardNumBase / 2) * (panelWidth / cardNumBase); // 设置卡片X坐标
            float y = panelHeight / 6; // 设置卡片Y坐标
            cardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y); // 设置卡片位置


            // 设置卡片数据
            // TODO: 生成随机卡片数据
            ICardData cardData = cardDataList[i];
            cardObj.GetComponent<Card>().updateCardStatus(cardData); // 更新卡片数据
            cardDataList.Add(cardData); // 添加到卡片数据列表

            // 给卡片绑定点击事件
            Button button = cardObj.GetComponent<Button>();
            List<int> test = new List<int> { 1, 2, 3 }; // 测试,删
            int j = test[i]; // 测试,删
            button.onClick.AddListener(() =>
            {
                // 点击卡片时，应用宝物效果
                Debug.Log(goldNum + " " + j + " ");
                work(goldNum, cardData, infos);
                close(); // 关闭宝物
            });
        }

        this.onClosed += onClosed; // 绑定关闭事件
    }

    public void close()
    {
        Destroy(treasure); // 销毁
        onClosed?.Invoke(); // 触发关闭事件
    }

    public delegate void OnClosed();

    public event OnClosed onClosed;

    // 应用宝物效果
    public static void work(int GoldNum, ICardData cardData, List<Adventurer> advs)
    {
        cardManager.AddGold(GoldNum); // 设置金币
        cardManager.AddCard(cardData); // 添加卡片
        foreach (var adv in advs)
        {
            adv.SetToGlobalStatus();
        }
    }
}
