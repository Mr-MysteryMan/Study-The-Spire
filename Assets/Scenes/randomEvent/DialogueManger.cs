using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManger : MonoBehaviour
{
    [Header("UI绑定")]
    public Button optionA;
    public Button optionB;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        optionA.onClick.AddListener(OnOptionA);
        optionB.onClick.AddListener(OnOptionB);
        exitButton.onClick.AddListener(OnExit);
    }

    void OnOptionA()
    {
        SceneManager.LoadScene("StopScene");
    }

    void OnOptionB() {

    }

    void OnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            SceneManager.UnloadScene("DialogueScene");
        #endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
