using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text AnswerText;
    public string text;
    public bool mark = true; // 使能

    public void SetAnswerText(string text)
    {
        AnswerText.text = text;
        this.text = text;
    }

    public void shift()
    {
        mark = !mark; // 使能
        AnswerText.color = mark ? Color.white : Color.gray; // 恢复颜色
    }
}
