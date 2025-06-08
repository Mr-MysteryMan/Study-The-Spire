using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnEndButtonController : MonoBehaviour
{
    public Button turnEndButton; // 回合结束按钮
    public TurnSystem turnSystem; // 回合系统

    private bool IsButtonEnabled => turnSystem.WhoseTurn == WhoseTurn.Player;

    private string playerButtonText = "结束回合"; // 玩家回合按钮文本
    private string enemyButtonText = "敌人回合"; // 敌人回合按钮文本

    private string GetButtonText() => IsButtonEnabled ? playerButtonText : enemyButtonText;

    private void UpdateButtonText()
    {
        turnEndButton.GetComponentInChildren<TMP_Text>().text = GetButtonText();
    }


    public void OnClicked()
    {
        if (IsButtonEnabled)
        {
            turnSystem.EndPlayerTurn();
            turnEndButton.interactable = false; // 禁用按钮
            UpdateButtonText(); // 更新按钮文本
        }
    }

    public void UpdateButtonState()
    {
        turnEndButton.interactable = IsButtonEnabled; // 根据当前回合状态更新按钮状态
        UpdateButtonText(); // 更新按钮文本
    }
}