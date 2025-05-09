using Combat;
using Cards.CardEffect;

class DefenseEffect : IEffect
{
    private int Defense; // 防御数值
    public DefenseEffect(int Defense)
    {
        this.Defense = Defense;
    }
    public void Work(Character survivor, Character enemy)
    {
        survivor.AddAmmor(Defense);
    }
}