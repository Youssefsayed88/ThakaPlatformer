using Thaka.Platformer.AI;
using Thaka.Platformer.Enemies.States;
using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    [RequireComponent(typeof(PatrolPath))]
    public class PatrollerEnemy : EnemyController
    {
        protected override IState ConfigureStates(StateMachine machine)
        {
            return new PatrolState(Mover, GetComponent<PatrolPath>(), Config);
        }
    }
}
