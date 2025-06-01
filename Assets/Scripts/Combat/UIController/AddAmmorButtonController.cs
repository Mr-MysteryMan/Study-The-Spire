using Combat;
using UnityEngine;

public class AddAmmorButtonContrroller : MonoBehaviour
{
    [SerializeField] private Combat.Character target;

    public void OnClick() {
        target.AddAmmor(10); 
        Debug.Log("Add Ammor button clicked!");
        Debug.Log("Target HP: " + target.CurHp + " Ammor: " + target.Ammor);
    }
}