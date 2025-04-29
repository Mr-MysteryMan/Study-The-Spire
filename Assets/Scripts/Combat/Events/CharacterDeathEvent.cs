namespace Combat.Events
{
    public class CharacterDeathEvent
    {
        public Character Target { get; } // 角色数据
        public Character Source { get; }

        public CharacterDeathEvent(Character target)
        {
            Target = target;
            Source = null;
        }

        public CharacterDeathEvent(Character target, Character source)
        {
            Target = target;
            Source = source;
        }
    }
}