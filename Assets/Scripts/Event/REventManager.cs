using System;
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
            Debug.LogError("REventController.Instance 是 null！");
            return;
        }
        butA.onClick.AddListener(optionA);
        butB.onClick.AddListener(optionB);
        butE.onClick.AddListener(optionExit);
    }

    void optionA()
    {
        //Debug.Log($"controller: {controller}");
        System.Random rand = new System.Random();
        int randMoney = rand.Next(-50, 51);
        controller.alterMoney(randMoney);
        loadMapEvent.RaiseEvent(null, this);
    }

    void optionB()
    {
        System.Random rand = new System.Random();
        int randBlood = rand.Next(-30, 31);
        controller.alterBlood(randBlood);
        loadMapEvent.RaiseEvent(null, this);
    }
    
    void optionExit()
    {
        loadMapEvent.RaiseEvent(null, this);
    }
}