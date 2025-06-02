using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitButton,continueButton,nodebookButton;

    public ObjectEventSO newGameEvent;
    public ObjectEventSO continueEvent;

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
        nodebookButton.clicked += onNoteBookOnClicked;
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


    // -------------------------------渲染题目笔记本-------------------------------
    public GameObject notebookPrefab; // 笔记本预制件
    private void onNoteBookOnClicked()
    {
        Instantiate(notebookPrefab); // 实例化笔记本预制件
    }
}