using System.Collections.Generic;
using UnityEngine;

public class TeamPanelManager : MonoBehaviour
{
    [Header("角色数据")]
    public List<CharacterData> characters;
    [Header("UI引用")]
    public CharacterCarouselUI leftPanel;
    public Transform slotContainer;
    public GameObject rightPrefab;
    private List<RoleSlot> roleSlots = new();
    private const int maxSlots = 3;

    private void Start()
    {
        GenerateCharacterData();
        GenerateRightSlots();
        leftPanel.Init(characters, AddCharacterToSlot);
    }

    private void GenerateCharacterData()
    {
        characters = new List<CharacterData>()
        {
            new CharacterData{type = CharacterType.Warrior, licon=Resources.Load<Sprite>("CharacterUI/warriorl"), ricon=Resources.Load<Sprite>("CharacterUI/warrior")},
            new CharacterData{type = CharacterType.Knight, licon=Resources.Load<Sprite>("CharacterUI/knightl"), ricon=Resources.Load<Sprite>("CharacterUI/knight")},
            new CharacterData{type = CharacterType.Mage, licon=Resources.Load<Sprite>("CharacterUI/magel"), ricon=Resources.Load<Sprite>("CharacterUI/mage")},
        };
    }

    private void GenerateRightSlots()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject obj = Instantiate(rightPrefab, slotContainer);
            var slot = obj.GetComponent<RoleSlot>();
            slot.Init(i, this);
            roleSlots.Add(slot);
        }
    }

    public void AddCharacterToSlot(CharacterData data)
    {
        foreach (var slot in roleSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetCharacter(data);
                break;
            }
        }
    }

    public void RemoveCharacterFromSlot(int index)
    {
        if (index >= 0 && index < maxSlots)
        {
            roleSlots[index].ClearSlot();
        }
    }
}