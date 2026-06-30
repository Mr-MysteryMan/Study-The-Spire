using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class REventManager : MonoBehaviour
{
    private REventController controller;

    [Header("UI绑定")]
    public Button butA;
    public Button butB;
    public Button butE;
    public ObjectEventSO loadMapEvent;
    public GameObject popupPanel;
    public float showTime = 1000f;
    public TextMeshProUGUI popupText;

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
        Debug.Log($"controller: {controller}");
        randGain();
        loadMapEvent.RaiseEvent(null, this);
    }

    void optionB()
    {
        randGain();
        loadMapEvent.RaiseEvent(null, this);
    }

    void optionExit()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    void randGain()
    {
        System.Random rand = new System.Random();
        int randType = rand.Next(0, 2);
        if (randType == 0)
        {
            int randMoney = rand.Next(-50, 51);
            controller.alterMoney(randMoney);
            ShowPopup(0, randMoney);
        }
        else
        {
            int randBlood = rand.Next(-30, 31);
            controller.alterBlood(randBlood);
            ShowPopup(1, randBlood);
        }
    }

    void ShowPopup(int type, int val)
    {
        if (type == 0)
        {
            if (val > 0) popupText.text = "恭喜你！\n获得了金钱: " + val.ToString() + "!";
            else if (val < 0) popupText.text = "你做出了错误的选择！\n失去了金钱: " + val.ToString() + "!";
            else popupText.text = "你并没有得到什么。但也没有失去，这何尝不是好事呢？";
        }
        else
        {
            if (val > 0) popupText.text = "恭喜你！\n回复了血量: " + val.ToString() + "!";
            else if (val < 0) popupText.text = "你做出了错误的选择！\n失去了血量: " + val.ToString() + "!";
            else popupText.text = "你并没有得到什么。但也没有失去，这何尝不是好事呢？";
        }
        // 启动协程来显示并延迟关闭弹窗
        StartCoroutine(ShowPopupCoroutine());
    }

    IEnumerator ShowPopupCoroutine()
    {
        popupPanel.SetActive(true); // 显示弹窗
        yield return new WaitForSeconds(showTime); // 等待指定时间
        popupPanel.SetActive(false); // 关闭弹窗
    }

}