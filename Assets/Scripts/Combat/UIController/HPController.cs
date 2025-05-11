using Combat;
using Combat.Events;
using Combat.EventVariable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private TMP_Text curHpText;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private GameObject ammorTextObject;
    public EventManager eventManager;
    private Character character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 由Character启动
    public void launch(Character character)
    {
        this.character = character;
        eventManager.Subscribe<ValueChangedEvent<int>>(OnHpChange);
        Change();
    }

    void OnHpChange(ValueChangedEvent<int> e)
    {
        if ((object)e.Parent == character && (e.ValueName == "CurHp" || e.ValueName == "MaxHp" || e.ValueName == "Ammor"))
        {
            Change();
        }
    }

    private void Change()
    {
        slider.value = (float)character.CurHp / character.MaxHp;
        curHpText.text = character.CurHp.ToString();
        maxHpText.text = character.MaxHp.ToString();
        if (character.Ammor > 0)
        {
            ammorTextObject.SetActive(true);
            ammorTextObject.GetComponentInChildren<TMP_Text>().text = character.Ammor.ToString();
        }
        else
        {
            ammorTextObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        eventManager.Unsubscribe<ValueChangedEvent<int>>(OnHpChange);
    }
}
