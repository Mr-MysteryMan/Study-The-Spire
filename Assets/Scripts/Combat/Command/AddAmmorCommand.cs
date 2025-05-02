namespace Combat.Command
{
    public struct AddAmmorCommand : ICommand {
        public Character Source { get; set; }
        public Character Target { get; set; }

        public int AmmorAmount { get; set; }

        public AddAmmorCommand(Character source, Character target, int ammorAmount) {
            Source = source;
            Target = target;
            AmmorAmount = ammorAmount;
        }

        public void Execute() {
            Target._AddAmmor(AmmorAmount);
        }
    }
}