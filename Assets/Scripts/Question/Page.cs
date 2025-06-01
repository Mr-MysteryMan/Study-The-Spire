using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public Text title; // 标题
    public GameObject Content; // 滚动框
    public GameObject ContentPrefab; // 问题预制件
    private List<GameObject> questionObjects = new List<GameObject>(); // 问题对象列表

    public void updateStatus(string title, QuestionData[] questions)
    {
        // 更新标题
        this.title.text = title;
        // 清空现有问题
        foreach (GameObject questionObject in questionObjects)
        {
            Destroy(questionObject);
        }
        // 创建新的问题对象
        questionObjects = new List<GameObject>();
        float baseY = 0; // 基础位置偏移量
        for (int i = 0; i < questions.Length; i++)
        {
            QuestionData question = questions[i];
            GameObject questionObject = Instantiate(ContentPrefab, Content.transform);
            // ---------------------设置问题数据--------------------------
            questionObject.GetComponent<Content>().updateStatus(question);
            // ---------------------设置大小位置--------------------------
            RectTransform rectTransform = questionObject.GetComponent<RectTransform>();
            // 设置位置,y为baseY
            rectTransform.anchoredPosition = new Vector2(0, baseY);
            baseY -= rectTransform.sizeDelta.y; // 更新基础位置偏移量
            // ---------------------存入--------------------------
            questionObjects.Add(questionObject);
        }
        // 更新Content的高度
        RectTransform contentRectTransform = Content.GetComponent<RectTransform>();
        // 设置Content的高度为所有问题高度之和
        contentRectTransform.sizeDelta = new Vector2(0, -baseY);
    }
}
