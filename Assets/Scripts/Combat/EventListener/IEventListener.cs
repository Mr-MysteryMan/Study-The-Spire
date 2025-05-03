namespace Combat.EventListener
{
    public interface IEventListener
    {
        void AddListen(EventManager eventManager);
        void RemoveListen(EventManager eventManager);
    }
}