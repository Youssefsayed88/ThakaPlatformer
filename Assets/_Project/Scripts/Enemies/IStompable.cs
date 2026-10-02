namespace Thaka.Platformer.Enemies
{
    public interface IStompable
    {
        bool IsAlive { get; }

        void Stomp();
    }
}
