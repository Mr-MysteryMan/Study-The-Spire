using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton,continueButton;

    public ObjectEventSO newGameEvent;
    public ObjectEventSO continueEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitButton");
        continueButton = rootElement.Q<Button>("ContinueGameButton");
        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
        continueButton.clicked += onContinueButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }

    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaiseEvent(null, this);
    }

    private void onContinueButtonClicked()
    {
        continueEvent.RaiseEvent(null, this);
    }
}