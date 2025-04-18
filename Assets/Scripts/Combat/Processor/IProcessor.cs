using System;
using Combat.Command;
namespace Combat.Processor {

    public interface IProcessor {
        public int Priority { get; }
        Type CommandType { get; }
    }
    
    public interface IProcessor<T> : IProcessor where T : ICommand {
        void Process(ref T command);
    }
}