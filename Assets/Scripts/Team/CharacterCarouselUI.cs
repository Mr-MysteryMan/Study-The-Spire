using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCarouselUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Button left;
    public Button right;
    public Button select;

    private List<CharacterData> characters;
    private int curIndex;
    private int length;
    private System.Action<CharacterData> onCharacterSelected;

    public void Init(List<CharacterData> characters, System.Action<CharacterData> onSelect)
    {
        this.characters = characters;
        curIndex = 0;
        length = characters.Count;
        onCharacterSelected = onSelect;

        left.onClick.AddListener(() => Switch(-1));
        right.onClick.AddListener(() => Switch(1));
        select.onClick.AddListener(OnClick);

        UpdateView();
    }

    private void Switch(int off)
    {
        curIndex = (curIndex + off + length) % length;
        UpdateView();
    }

    private static readonly Dictionary<CharacterType, string> CharacterTypeNames = new Dictionary<CharacterType, string>
    {
        { CharacterType.Warrior, "战士" },
        { CharacterType.Knight, "骑士" },
        { CharacterType.Mage, "法师" }
    };

    private void UpdateView()
    {
        var data = characters[curIndex];
        icon.sprite = data.licon;
        nameText.text = CharacterTypeNames[data.type];
    }

    private void OnClick()
    {
        var data = characters[curIndex];
        onCharacterSelected?.Invoke(data);
    }
}