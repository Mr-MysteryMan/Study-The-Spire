using Combat;
using Combat.Events;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Combat.Character character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
