using Combat.Command;
namespace Combat.Observer {
    public interface IObserver<T> where T : ICommand
    {
        public int Priority { get; }

        // 命令执行前的检查，如需要发送事件，可以通过manager发送
        void PreCheck(EventManager manager, T Command);

        // 命令执行后的检查，如需要发送事件，可以通过manager发送
        void PostCheck(EventManager manager, T Command);
    }
}