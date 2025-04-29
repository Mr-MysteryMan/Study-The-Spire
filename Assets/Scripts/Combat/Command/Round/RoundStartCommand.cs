namespace Combat.Command.Turn {
    public struct TurnStartCommand : ICommand
    {
        public Character Source { get; set; }
        public Character Target { get; set; }

        private TurnSystem roundSystem;

        public TurnStartCommand(Character source, Character target, TurnSystem roundSystem) {
            Source = source;
            Target = target;
            this.roundSystem = roundSystem;
        }

        public void Execute()
        {
        }
    }
}