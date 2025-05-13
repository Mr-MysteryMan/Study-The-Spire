using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text AnswerText;
    public string text;

    public void SetAnswerText(string text)
    {
        AnswerText.text = text;
        this.text = text;
    }
}
