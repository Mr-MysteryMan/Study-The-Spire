
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionStore : MonoBehaviour
{
  // -----------------------------题库选择----------------------------------
  public GameObject QuestionStoreUI; // 题库选择UI
  public GameObject Selections; // 题库选择UI
  public GameObject TextBox; // 题库选择文本框
  public GameObject ConfirmButton; // 确认按钮

  void Start()
  {
    if (isInit == false)
    {
      Init(); // 初始化题库
    }

    // -----------------------------题库选择----------------------------------
    int line = Mathf.CeilToInt((float)questionListNames.Count / 4); // 向上取整
    Selections.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250 * (line + 1)); // 设置长度
    // 创建题库选项TextBox, 每行四个, 均匀分布
    for (int i = 0; i < questionListNames.Count; i++)
    {
      GameObject textBox = Instantiate(TextBox, Selections.transform);
      textBox.GetComponent<TextBox>().SetAnswerText(questionListNames[i]); // 设置文本
      // 未启用的设为灰色
      if (!EnableQuestionListNames.Contains(questionListNames[i]))
      {
        textBox.GetComponent<TextBox>().shift();
      }
      // 设置位置
      int row = i / 4; // 行数
      int column = i % 4; // 列数
      RectTransform rectTransform = textBox.GetComponent<RectTransform>();
      rectTransform.anchorMin = new Vector2(0.05f + column * 0.225f, 1);
      rectTransform.anchorMax = new Vector2(0.25f + column * 0.225f, 1);
      rectTransform.anchoredPosition = new Vector2(0, -200 - row * 250); // 设置位置
      rectTransform.sizeDelta = new Vector2(0, 200); // 设置大小

      // 绑定点击事件
      textBox.GetComponent<Button>().onClick.AddListener(() =>
      {
        TextBox tb = textBox.GetComponent<TextBox>();
        if (EnableQuestionListNames.Count <= 3 && tb.mark == true) // 至少保留3个题库
        {
          return;
        }
        tb.shift(); // 切换使能状态
        shiftQuestionList(tb.text); // 更新题库
      });
    }

    // -----------------------------确认退出----------------------------------
    ConfirmButton.GetComponent<Button>().onClick.AddListener(() =>
    {
      SaveQuestionListConfig(); // 保存题库配置文件
      Destroy(QuestionStoreUI); // 销毁题库选择UI
    });
  }

  // -----------------------------仓库控制----------------------------------
  public static readonly List<string> questionListNames = new List<string> { "生活常识", "英语缩写", "历史知识", "二次元", "数学知识", "古典诗词", "计算机", "乐理", "游戏", "地理" }; // 所有问题列表名称
  public static List<string> EnableQuestionListNames = new List<string>(); // 启用的问题列表名称
  public static Dictionary<string, QuestionList> questionStore = new Dictionary<string, QuestionList>(); // 可用题库
  public static bool isInit = false; // 是否初始化

  // 初始化, 全局只调用一次
  public static void Init()
  {
    if (isInit) return; // 如果已经初始化则不再执行
    LoadQuestionListConfig(); // 从配置文件加载题库
    if (EnableQuestionListNames.Count < 3) // 如果启用的题库少于3个，则默认出现问题, 重新加载所有
    {
      EnableQuestionListNames = new List<string>(questionListNames);
    }
    for (int i = 0; i < questionListNames.Count; i++)
    {
      string fileName = questionListNames[i];
      if (EnableQuestionListNames.Contains(fileName))
      {
        questionStore.Add(fileName, LoadQuestions(fileName)); // 添加题库
      }
    }
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

  public static void shiftQuestionList(string fileName)
  {
    if (EnableQuestionListNames.Contains(fileName))
    {
      RemoveQuestionList(fileName); // 删除题库
    }
    else
    {
      AddQuestionList(fileName); // 添加题库
    }
    Debug.Log("EnableQuestionListNames: " + EnableQuestionListNames.Count); // 打印启用的题库数量
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

  // 更新题库配置文件
  public static void SaveQuestionListConfig()
  {
    string filePath = Path.Combine(Application.streamingAssetsPath, "question", "config.json");
    QuestionListConfig config = new QuestionListConfig(EnableQuestionListNames);
    string json = JsonUtility.ToJson(config, true);
    try
    {
      File.WriteAllText(filePath, json);
      Debug.Log("Saved question list config to: " + filePath);
    }
    catch (System.Exception e)
    {
      Debug.LogError("Failed to save question list config: " + e.Message);
    }
  }

  // 从配置文件加载题库
  public static void LoadQuestionListConfig()
  {
    string filePath = Path.Combine(Application.streamingAssetsPath, "question", "config.json");
    if (File.Exists(filePath))
    {
      string json = File.ReadAllText(filePath);
      QuestionListConfig config = JsonUtility.FromJson<QuestionListConfig>(json);
      EnableQuestionListNames = config.questionListNames;
      Debug.Log("Loaded question list config from: " + filePath);
    }
    else
    {
      Debug.LogWarning("Question list config file not found, using default question lists.");
      EnableQuestionListNames = new List<string>(questionListNames); // 使用默认题库
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
public class QuestionData
{
  public string question;
  public string[] selection;
  public int answer;

  public QuestionData(string question, string[] selection, int answer)
  {
    this.question = question;
    this.selection = selection;
    this.answer = answer;
  }
}
[System.Serializable]
public class QuestionListConfig
{
  public List<string> questionListNames;

  public QuestionListConfig(List<string> questionListNames)
  {
    this.questionListNames = questionListNames;
  }
}