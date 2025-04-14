using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StopManager : MonoBehaviour
{
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(OnExit);
    }

    void OnExit()
    {
        SceneManager.LoadScene("DialogueScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
