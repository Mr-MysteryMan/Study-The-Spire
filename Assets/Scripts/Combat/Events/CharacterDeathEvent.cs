public class CharacterDeathEvent
{
    public CharacterData Target { get; } // 角色数据
    public CharacterData Source { get; }

    public CharacterDeathEvent(CharacterData target)
    {
        Target = target;
        Source = null;
    }

    public CharacterDeathEvent(CharacterData target, CharacterData source)
    {
        Target = target;
        Source = source;
    }
}