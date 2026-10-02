using Thaka.Platformer.AI;
using Thaka.Platformer.Config;

namespace Thaka.Platformer.Enemies.States
{
    public class PatrolState : IState
    {
        readonly EnemyMover mover;
        readonly PatrolPath path;
        readonly EnemyConfig config;

        bool movingRight;
        float pauseTimer;

        public PatrolState(EnemyMover mover, PatrolPath path, EnemyConfig config)
        {
            this.mover = mover;
            this.path = path;
            this.config = config;
        }

        public void Enter()
        {
            movingRight = mover.IsFacingRight;
            pauseTimer = 0f;
        }

        public void Tick(float deltaTime)
        {
            if (pauseTimer > 0f)
            {
                pauseTimer -= deltaTime;
                return;
            }

            var targetX = movingRight ? path.RightX : path.LeftX;
            if (mover.MoveTowards(targetX, config.PatrolSpeed, deltaTime))
            {
                movingRight = !movingRight;
                pauseTimer = config.WaypointPauseSeconds;
            }
        }

        public void Exit()
        {
        }
    }
}
