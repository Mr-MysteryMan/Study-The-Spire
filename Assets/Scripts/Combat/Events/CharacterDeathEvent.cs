namespace Combat.Events
{
    public class CharacterDeathEvent
    {
        public Character Murderer { get; } // 角色数据
        public Character Victim { get; }

        public CharacterDeathEvent(Character victim)
        {
            Victim = victim;
            Murderer = null;
        }

        public CharacterDeathEvent(Character victim, Character muderer)
        {
            Victim = victim;
            Murderer = muderer;
        }
    }
}