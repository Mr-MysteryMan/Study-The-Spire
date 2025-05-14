using UnityEngine;
using UnityEngine.UI;
using System;

public class CardSearchBar : MonoBehaviour
{
    private InputField inputField;

    // 公开的委托事件，监听搜索词变化
    public Action<string> OnSearchKeywordChanged;

    private void Awake()
    {
        inputField = GetComponentInChildren<InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string keyword)
    {
        OnSearchKeywordChanged?.Invoke(keyword.Trim().ToLower());
    }

    public void SetKeyword(string keyword)
    {
        inputField.text = keyword;
    }

    public void Clear()
    {
        inputField.text = "";
    }
}
