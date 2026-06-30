namespace Combat.Command
{
    public struct HealCommand : ICommand
    {
        public Character Source { get; set; }
        public Character Target { get; set; }
        public int HealAmount;

        public HealCommand(Character source, Character target, int healAmount)
        {
            Source = source;
            Target = target;
            HealAmount = healAmount;
        }

        public void Execute()
        {
            Target._Heal(HealAmount);
        }
    }
}