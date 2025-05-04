namespace Combat.Events.Turn
{
    public class TurnStartEvent
    {
        public Character Character;
        public int TurnNum; // 当前回合数

        public TurnStartEvent(Character character, int turnNum)
        {
            Character = character;
            TurnNum = turnNum;
        }
    }
}