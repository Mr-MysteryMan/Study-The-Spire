using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoleSlot : MonoBehaviour
{
    public Image icon;
    public Text nameText;
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

    private static readonly Dictionary<CharacterType, string> CharacterTypeNames = new Dictionary<CharacterType, string>
    {
        { CharacterType.Warrior, "战士" },
        { CharacterType.Knight, "骑士" },
        { CharacterType.Mage, "法师" }
    };

    public void SetCharacter(CharacterData data)
    {
        curCharacter = data;
        icon.sprite = data.ricon;
        icon.enabled = true;
        nameText.text = CharacterTypeNames[data.type];
    }

    public void ClearSlot()
    {
        curCharacter = null;
        icon.sprite = null;
        icon.enabled = false;
        nameText.text = "";
    }

    public bool IsEmpty() => curCharacter == null;

    public CharacterType GetCharacterType()
    {
        return curCharacter.type;
    }
}