using Combat;
using Combat.Events;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public EventManager eventManager;
    [SerializeField] private Combat.Character character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 由Character启动
    public void launch()
    {
        eventManager.Subscribe<DamageDealtEvent>(OnHpChange);
    }

    void OnHpChange(DamageDealtEvent e) {
        if (e.Target == character) {
            slider.value = (float)e.Target.CurHp / e.Target.MaxHp;
        }
    }

    void OnDestroy()
    {
        eventManager.Unsubscribe<DamageDealtEvent>(OnHpChange);
    }
}
