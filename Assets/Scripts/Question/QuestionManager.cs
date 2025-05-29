using System.Collections;
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
    public static QuestionList questionList; // 问题列表
    QuestionData question;
    // -----------------------------回调方法-------------------------------
    public callBackFunction onCorrect = () => {Debug.Log("Correct answer!");}; // 回答正确回调
    public callBackFunction onWrong = () => {Debug.Log("Wrong answer!");}; // 回答错误回调

    private bool isAnswered = false;

    public void MarkAnswered() => isAnswered = true;

    public IEnumerator WaitUntilAnswered()
    {
        yield return new WaitUntil(() => isAnswered);
    }

    private void OnAnswerSelected(int index)
    {
        if (index == question.answer) {
            image.sprite = Resources.Load<Sprite>("QuestionUI/Correct");
            onCorrect();
        }
        else {
            image.sprite = Resources.Load<Sprite>("QuestionUI/Wrong");
            onWrong();
        }

        MarkAnswered(); // 标记为已回答
    }

    void Start()
    {
        if (questionList == null) {
            Init();
        }
        // 选取随机问题
        int randomIndex = Random.Range(0, questionList.questions.Length);
        question = questionList.questions[randomIndex];
        // 初始化问题组件UI
        upDateUI();
        // 选项点击事件
        for (int i = 0; i < AnswerBox.Length; i++)
        {
            if (i < question.selection.Length)
            {
                int index = i; // 捕获变量
                AnswerBox[i].GetComponent<Button>().onClick.AddListener(() => OnAnswerSelected(index));
            }
        }
    }

    // 传入回调函数
    public void init (callBackFunction onCorrect, callBackFunction onWrong) {
        // 保存回调函数
        this.onCorrect = onCorrect;
        this.onWrong = onWrong;
    }

    public void close() {
        Destroy(Question);
    }

    private void upDateUI()
    {
        // 更新问题框
        QuestionBox.GetComponent<TextBox>().SetAnswerText(question.question);
        // 更新选项框
        for (int i = 0; i < AnswerBox.Length; i++)
        {
            if (i < question.selection.Length)
            {
                AnswerBox[i].GetComponent<TextBox>().SetAnswerText(question.selection[i]);
            }
        }
    }

    // 初始化静态变量
    public static void Init()
    {
        questionList = LoadQuestions("questions.json");
    }

    private static QuestionList LoadQuestions(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            QuestionList questionList = JsonUtility.FromJson<QuestionList>(json);
            return questionList;
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}

public delegate void callBackFunction(); // 回调函数类型

[System.Serializable]
public class QuestionList {
    public QuestionData[] questions;
    public QuestionList(QuestionData[] questions) {
        this.questions = questions;
    }
}
[System.Serializable]
public class QuestionData {
    public string question;
    public string[] selection;
    public int answer;

    public QuestionData(string question, string[] selection, int answer) {
        this.question = question;
        this.selection = selection;
        this.answer = answer;
    }
}