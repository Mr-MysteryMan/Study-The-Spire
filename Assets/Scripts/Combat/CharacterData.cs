using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName; // 角色名称
    [SerializeField] private ReactiveIntVariable curHP;
    [SerializeField] private int maxHP; // 最大生命值

    public int CurHP {
        get => curHP.Value;
    }

    public int MaxHP => maxHP; 
    public string CharacterName => characterName;

    [SerializeField] private ReactiveIntVariable curAmmor;

    public int CurAmmor {
        get => curAmmor.Value;
    }


    private void Init() {
        curHP = new ReactiveIntVariable("CurHP", "HPChangedEvent", 0, this);
        curAmmor = new ReactiveIntVariable("CurAmmor", "AmmorChangedEvent", 0, this);
    }

    private void OnEnable() {
        Init();
    }

    public void TakeAmmorDamage(int damage) {
        this.curAmmor.Value = math.max(0, this.curAmmor.Value - damage);
    }

    public void TakeHPDamage(int damage) {
        this.curHP.Value -= damage;
    }
}
