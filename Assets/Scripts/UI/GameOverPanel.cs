using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    public Button backToMenuButton;
    public ObjectEventSO loadMenuEvent;

    public GameManager gameManager;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").clicked += BackToMenu;
    }

    private void BackToMenu()
    {
        loadMenuEvent.RaiseEvent(null, this);
        this.gameObject.SetActive(false);
        gameManager.OnNewGameEvent();
    }
}