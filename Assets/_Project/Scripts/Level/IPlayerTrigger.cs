namespace Thaka.Platformer.Level
{
    // Called every frame the player overlaps the object, so implementations must be safe to call repeatedly
    public interface IPlayerTrigger
    {
        void OnPlayerTouch();
    }
}
