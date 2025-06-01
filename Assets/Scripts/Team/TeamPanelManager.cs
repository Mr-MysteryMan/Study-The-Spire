using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPanelManager : MonoBehaviour
{
    [Header("角色数据")]
    public List<CharacterData> characters;
    [Header("UI引用")]
    public CharacterCarouselUI leftPanel;
    public Transform slotContainer;
    public GameObject rightPrefab;
    public Button close;
    private List<RoleSlot> roleSlots = new();
    private const int maxSlots = 3;
    private List<CharacterType> types = new();

    private void Start()
    {
        GenerateCharacterData();
        GenerateRightSlots();
        leftPanel.Init(characters, AddCharacterToSlot);
        close.onClick.AddListener(OnClose);
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
            obj.SetActive(true);
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
                types.Add(data.type);
                break;
            }
        }
    }

    public void RemoveCharacterFromSlot(int index)
    {
        if (index >= 0 && index < maxSlots)
        {
            types.Remove(roleSlots[index].GetCharacterType());
            roleSlots[index].ClearSlot();
            
        }
    }

    private void OnClose()
    {
        CardManager.Instance.SetCharacterTypes(types);
    }
}