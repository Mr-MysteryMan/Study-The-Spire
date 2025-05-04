using Combat;
using Combat.Events;
using Combat.EventVariable;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public EventManager eventManager;
    private Character character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 由Character启动
    public void launch(Character character)
    {
        this.character = character;
        eventManager.Subscribe<ValueChangedEvent<int>>(OnHpChange);
    }

    void OnHpChange(ValueChangedEvent<int> e) {
        if ((object)e.Parent == character && e.Parent is Character target) {
            slider.value = (float)target.CurHp / target.MaxHp;
        }
    }

    void OnDestroy()
    {
        eventManager.Unsubscribe<ValueChangedEvent<int>>(OnHpChange);
    }
}
