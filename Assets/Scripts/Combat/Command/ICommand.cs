using System;
using System.Collections;

namespace Combat.Command
{
    public interface ICommand
    {
        public Character Source { get; }
        public Character Target { get; }

        void Execute();
    }

    // 异步命令接口
    public interface IAsyncCommand : ICommand
    {
        IEnumerator ExecuteAsync();
    }
}