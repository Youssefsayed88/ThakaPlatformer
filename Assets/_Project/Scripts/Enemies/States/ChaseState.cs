using Thaka.Platformer.AI;
using Thaka.Platformer.Config;

namespace Thaka.Platformer.Enemies.States
{
    public class ChaseState : IState
    {
        readonly EnemyMover mover;
        readonly PatrolPath path;
        readonly TargetSensor sensor;
        readonly EnemyConfig config;

        public ChaseState(EnemyMover mover, PatrolPath path, TargetSensor sensor, EnemyConfig config)
        {
            this.mover = mover;
            this.path = path;
            this.sensor = sensor;
            this.config = config;
        }

        public void Enter()
        {
        }

        public void Tick(float deltaTime)
        {
            var targetX = sensor.Target.Position.x;

            // Stay on the path so the chaser can't follow the player off a ledge, but keep facing them
            mover.MoveTowards(path.Clamp(targetX), config.ChaseSpeed, deltaTime);
            mover.Face(targetX - mover.X);
        }

        public void Exit()
        {
        }
    }
}
