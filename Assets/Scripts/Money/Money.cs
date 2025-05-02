using UnityEngine;
using TMPro; // 使用 TextMeshPro
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinText; // 拖拽 UI 文本组件到 Inspector
    void Update()
    {
        coinText.text = "Money:" + GameManager.money;
    }
}