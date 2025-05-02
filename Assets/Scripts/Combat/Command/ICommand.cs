using System;

namespace Combat.Command
{
    public interface ICommand
    {
        public Character Source { get; }
        public Character Target { get; }

        void Execute();
    }
}