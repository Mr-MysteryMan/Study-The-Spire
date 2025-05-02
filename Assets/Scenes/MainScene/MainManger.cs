using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManger : MonoBehaviour
{
    [Header("UI绑定")]
    public Button optionA;
    public Button optionB;
    public Button optionc;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        optionA.onClick.AddListener(OnOptionA);
        optionB.onClick.AddListener(OnOptionB);
        optionB.onClick.AddListener(OnOptionC);
        exitButton.onClick.AddListener(OnExit);
    }

    void OnOptionA()
    {
        SceneManager.LoadScene("Map");
    }

    void OnOptionB()
    {

    }

    void OnOptionC()
    {

    }

    void OnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
