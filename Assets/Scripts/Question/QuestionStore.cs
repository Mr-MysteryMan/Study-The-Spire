
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionStore : MonoBehaviour
{
  // -----------------------------变量----------------------------------
  public static readonly List<string> questionListNames = new List<string> { "生活常识", "英语缩写", "历史知识", "二次元", "数学知识" }; // 所有问题列表名称
  private static List<string> EnableQuestionListNames = new List<string>(); // 启用的问题列表名称
  public static Dictionary<string, QuestionList> questionStore = new Dictionary<string, QuestionList>(); // 可用题库
  public static bool isInit = false; // 是否初始化

  // 初始化
  public static void Init()
  {
    for (int i = 0; i < questionListNames.Count; i++)
    {
      string fileName = questionListNames[i];
      QuestionList questionList = LoadQuestions(fileName);
      if (questionList != null)
      {
        questionStore.Add(fileName, questionList);
        EnableQuestionListNames.Add(fileName);
      }
    }
    Debug.Log("QuestionStore initialized with " + questionStore.Count + " question lists.");
    isInit = true;
  }
  // 获取随机问题
  public static QuestionData GetRandomQuestion()
  {
    // 取随机可用题库
    int randomIndex = Random.Range(0, EnableQuestionListNames.Count);
    string randomQuestionListName = EnableQuestionListNames[randomIndex];
    // 取随机问题
    QuestionList questionList = questionStore[randomQuestionListName];
    int randomQuestionIndex = Random.Range(0, questionList.questions.Length);
    QuestionData question = questionList.questions[randomQuestionIndex];

    return question;
  }

  // 添加问题库
  public static void AddQuestionList(string fileName)
  {
    if (!questionStore.ContainsKey(fileName))
    {
      QuestionList questionList = LoadQuestions(fileName);
      if (questionList != null)
      {
        questionStore.Add(fileName, questionList);
        EnableQuestionListNames.Add(fileName);
      }
    }
  }

  // 删除问题库
  public static void RemoveQuestionList(string fileName)
  {
    if (questionStore.ContainsKey(fileName))
    {
      questionStore.Remove(fileName);
      EnableQuestionListNames.Remove(fileName);
    }
  }

  private static QuestionList LoadQuestions(string fileName)
  {
    string filePath = Path.Combine(Application.streamingAssetsPath, "question", fileName + ".json");
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