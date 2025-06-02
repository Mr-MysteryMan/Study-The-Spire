using UnityEngine;

public enum CharacterType
{
    Warrior,
    Knight,
    Mage
}

[System.Serializable]
public class CharacterData
{
    public CharacterType type;
    public Sprite licon;
    public Sprite ricon;
}