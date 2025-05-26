using Combat;
using Cards.CardEffect;
using System.Collections.Generic;

class DefenseEffect : IEffect
{
    private int Defense; // 防御数值
    public DefenseEffect(int Defense)
    {
        this.Defense = Defense;
    }
    public void Work(Character survivor, List<Character> enemy)
    {
        survivor.AddAmmor(Defense);
    }
}