using UnityEngine;

public class AttackButtonController : MonoBehaviour
{
    [SerializeField] private Combat.Character adventurer;
    [SerializeField] private Combat.Character monster;

    public void OnClick() {
        adventurer.Attack(monster, 10);
        Debug.Log("Attack button clicked!");
        Debug.Log("Adventurer HP: " + adventurer.CurHp + " Ammor: " + adventurer.Ammor);
        Debug.Log("Monster HP: " + monster.CurHp + " Ammor: " + monster.Ammor);
    }
}
