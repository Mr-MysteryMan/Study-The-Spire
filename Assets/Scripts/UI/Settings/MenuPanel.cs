using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton,continueButton,nodebookButton;

    public ObjectEventSO newGameEvent;
    public ObjectEventSO continueEvent;
    public GameObject nodebookPre;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitButton");
        continueButton = rootElement.Q<Button>("ContinueGameButton");
        nodebookButton = rootElement.Q<Button>("NodebookButton");
        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
        continueButton.clicked += onContinueButtonClicked;
        nodebookButton.clicked += onNodeBookonClicked;
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

    private void onNodeBookonClicked()
    {
        Instantiate(nodebookPre);
        
    }
}