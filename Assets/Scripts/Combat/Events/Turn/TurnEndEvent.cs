namespace Combat.Events.Turn
{
    public class TurnEndEvent
    {
        public Character Character;
        public int TurnNum; // 当前回合数

        public TurnEndEvent(Character character, int turnNum)
        {
            Character = character;
            TurnNum = turnNum;
        }
    }
}