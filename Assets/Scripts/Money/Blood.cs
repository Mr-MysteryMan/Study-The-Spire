using UnityEngine;
using TMPro; // 使用 TextMeshPro
using UnityEngine.UI;

public class BloodPlay : MonoBehaviour
{
    public TextMeshProUGUI bloodText; // 拖拽 UI 文本组件到 Inspector
    void Update()
    {
        bloodText.text = "Blood:" + CardManager.Instance.Health;
    }
}