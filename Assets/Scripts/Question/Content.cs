using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Text questionText; // 问题文本
    public Text answerText; // 答案文本
    public void updateStatus(QuestionData questionData)
    {
        // 更新问题文本
        questionText.text = questionData.question;
        // 更新答案文本
        answerText.text = questionData.selection[questionData.answer];
    }
}
