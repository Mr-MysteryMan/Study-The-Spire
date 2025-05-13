using System.IO;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuestionList questionList = LoadQuestions("questions.json");
        Debug.Log("Question List Loaded: " + questionList.questions.Length + " questions found.");
    }

    private QuestionList LoadQuestions(string fileName)
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