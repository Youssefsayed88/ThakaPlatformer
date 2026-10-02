using Thaka.Platformer.AI;
using Thaka.Platformer.Enemies.States;
using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    [RequireComponent(typeof(PatrolPath))]
    public class ChaserEnemy : EnemyController, ITargetSeeker
    {
        TargetSensor sensor;

        public void SetTarget(ITarget target)
        {
            sensor.Target = target;
        }

        protected override IState ConfigureStates(StateMachine machine)
        {
            var path = GetComponent<PatrolPath>();
            sensor = new TargetSensor(transform, Config.DetectionRadius, Config.LoseRadius);

            var patrol = new PatrolState(Mover, path, Config);
            var chase = new ChaseState(Mover, path, sensor, Config);

            machine.AddTransition(patrol, chase, () => sensor.CanDetect);
            machine.AddTransition(chase, patrol, () => sensor.HasLost);

            return patrol;
        }

        void OnDrawGizmosSelected()
        {
            if (Config == null)
                return;

            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawWireSphere(transform.position, Config.DetectionRadius);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, Config.LoseRadius);
        }
    }
}
