using Combat;
using UnityEngine;

public class HealButtonController : MonoBehaviour
{
    [SerializeField] private Combat.Character target;

    public void OnClick() {
        target.Heal(10); 
        Debug.Log("Heal button clicked!");
        Debug.Log("Target HP: " + target.CurHp + " Ammor: " + target.Ammor);
    }
}
