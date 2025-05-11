namespace Combat.Command
{
    public struct RemoveAmmorCommand : ICommand {
        public Character Source { get; set; }
        public Character Target { get; set; }
        
        public RemoveAmmorCommand(Character source, Character target) {
            Source = source;
            Target = target;
        }

        public void Execute() {
            Target._TakeAmmorDamage(Target.Ammor);
        }
    }
}