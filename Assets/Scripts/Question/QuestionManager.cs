using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    // -----------------------------游戏组件------------------------------
    public GameObject Question; // 问题整体
    public GameObject QuestionBox; // 问题框
    public GameObject[] AnswerBox; // 选项框

    public Image image; // 背景图
    // -----------------------------变量----------------------------------
    public QuestionData question; // 问题数据
    public QuestionStore questionStore = new QuestionStore(); // 题库
    // -----------------------------回调方法-------------------------------
    public callBackFunction onCorrect = () => { Debug.Log("Correct answer!"); }; // 回答正确回调
    public callBackFunction onWrong = () => { Debug.Log("Wrong answer!"); }; // 回答错误回调

    void Start()
    {
        if (QuestionStore.isInit == false)
        {
            QuestionStore.Init(); // 初始化题库
        }
        // 选取随机问题
        this.question = QuestionStore.GetRandomQuestion();
        // 初始化问题组件UI
        upDateUI();
        // 选项点击事件
        for (int i = 0; i < AnswerBox.Length; i++)
        {
            string ans = AnswerBox[i].GetComponent<TextBox>().text; // 获取选项文本
            AnswerBox[i].GetComponent<Button>().onClick.AddListener(() => OnAnswerSelected(ans));
        }
    }

    // 传入回调函数
    public void init(callBackFunction onCorrect, callBackFunction onWrong)
    {
        // 保存回调函数
        this.onCorrect = onCorrect;
        this.onWrong = onWrong;
    }

    public void close()
    {
        Destroy(Question);
    }

    private void OnAnswerSelected(string answer)
    {
        // 检查答案是否正确
        if (answer == question.selection[question.answer])
        {
            image.sprite = Resources.Load<Sprite>("QuestionUI/Correct"); // 加载正确答案的图片
            onCorrect(); // 调用正确答案的回调函数
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("QuestionUI/Wrong"); // 加载错误答案的图片
            onWrong(); // 调用错误答案的回调函数
        }
    }

    private void upDateUI()
    {
        // 更新问题框
        QuestionBox.GetComponent<TextBox>().SetAnswerText(question.question);
        // 更新选项框, 四个选项随机出现
        List<int> randomIndex = new List<int>();
        for (int i = 0; i < question.selection.Length; i++)
        {
            randomIndex.Add(i);
        }
        for (int i = 0; i < question.selection.Length; i++)
        {
            int random = Random.Range(0, randomIndex.Count);
            AnswerBox[i].GetComponent<TextBox>().SetAnswerText(question.selection[randomIndex[random]]);
            randomIndex.RemoveAt(random);
        }
    }


}

public delegate void callBackFunction(); // 回调函数类型

