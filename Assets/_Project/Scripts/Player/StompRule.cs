namespace Thaka.Platformer.Player
{
    public static class StompRule
    {
        // Uses last frame's feet position so a fast fall can't tunnel past the top before contact is detected
        public static bool IsStomp(float playerVelocityY, float previousFeetY, float enemyTopY, float tolerance)
        {
            return playerVelocityY <= 0f && previousFeetY >= enemyTopY - tolerance;
        }
    }
}
