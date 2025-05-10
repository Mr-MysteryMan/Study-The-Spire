namespace Combat.Events
{
    public class AddAmmorEvent
    {
        public int Ammor { get; } // 护甲值
        public Character Source { get; } // 护甲来源
        public Character Target { get; } // 护甲目标

        public AddAmmorEvent(int ammor, Character source, Character target)
        {
            Ammor = ammor;
            Source = source;
            Target = target;
        }
    }
}