using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class REventManager : MonoBehaviour
{
    private REventController controller;

    [Header("UI绑定")]
    public Button butA;
    public Button butB;
    public Button butE;
    public ObjectEventSO loadMapEvent;

    void Awake()
    {
        controller = REventController.Instance;
        Debug.Log($"controller: {controller}");
        Debug.Log($"loadMapEvent: {loadMapEvent}");
        if (controller == null)
        {
            Debug.LogError("REventController.Instance 是 null！请确认场景中有挂载 REventController 的 GameObject。");
            return;
        }
        butA.onClick.AddListener(optionA);
        butB.onClick.AddListener(optionB);
        butE.onClick.AddListener(optionExit);
    }

    void optionA()
    {
        Debug.Log($"controller: {controller}");
        controller.alterMoney(20);
        loadMapEvent.RaiseEvent(null, this);
    }

    void optionB()
    {
        //controller.alterBlood(-10);
    }
    
    void optionExit()
    {
        //controller.alterMoney(20);
    }
}