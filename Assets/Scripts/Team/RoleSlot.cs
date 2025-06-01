using UnityEngine;
using UnityEngine.UI;

public class RoleSlot : MonoBehaviour
{
    public Image icon;
    private CharacterData curCharacter;
    private TeamPanelManager manager;
    private int slotIndex;

    public void Init(int index, TeamPanelManager mgr)
    {
        slotIndex = index;
        manager = mgr;
        GetComponent<Button>().onClick.AddListener(() => manager.RemoveCharacterFromSlot(slotIndex));
        ClearSlot();
    }

    public void SetCharacter(CharacterData data)
    {
        curCharacter = data;
        icon.sprite = data.ricon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        curCharacter = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public bool IsEmpty() => curCharacter == null;

    public CharacterType GetCharacterType()
    {
        return curCharacter.type;
    }
}