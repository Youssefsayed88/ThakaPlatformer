using System;
using Thaka.Platformer.AI;
using Thaka.Platformer.Config;
using Thaka.Platformer.Enemies.States;
using Thaka.Platformer.Persistence;
using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    [RequireComponent(typeof(PersistentId), typeof(Collider), typeof(Rigidbody))]
    public abstract class EnemyController : MonoBehaviour, IStompable, IContactDamage
    {
        [SerializeField] EnemyConfig config;

        StateMachine stateMachine;
        PersistentId persistentId;

        public event Action<EnemyController> Defeated;

        public string Id => persistentId.Id;
        public bool IsAlive { get; private set; } = true;
        public int ContactDamage => config.ContactDamage;
        public string CurrentStateName => stateMachine.Current?.GetType().Name;

        protected EnemyConfig Config => config;
        protected EnemyMover Mover { get; private set; }

        void Awake()
        {
            persistentId = GetComponent<PersistentId>();
            Mover = new EnemyMover(transform);
            stateMachine = new StateMachine();

            var initialState = ConfigureStates(stateMachine);
            var defeated = new DefeatedState(gameObject, GetComponent<Collider>(), config.DefeatedVanishSeconds);
            stateMachine.AddAnyTransition(defeated, () => !IsAlive);
            stateMachine.SetState(initialState);
        }

        void Update()
        {
            stateMachine.Tick(Time.deltaTime);
        }

        // Each enemy type registers its own states and transitions and returns the starting state
        protected abstract IState ConfigureStates(StateMachine machine);

        public void Stomp()
        {
            if (!IsAlive)
                return;

            IsAlive = false;
            Defeated?.Invoke(this);
        }
    }
}
