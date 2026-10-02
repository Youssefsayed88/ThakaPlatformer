using UnityEngine;

namespace Thaka.Platformer.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Thaka/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Patrol")]
        [SerializeField, Min(0f)] float patrolSpeed = 2.5f;
        [SerializeField, Min(0f)] float waypointPauseSeconds = 0.4f;

        [Header("Chase (Chaser only)")]
        [SerializeField, Min(0f)] float chaseSpeed = 4.5f;
        [Tooltip("Starts chasing when the player is closer than this.")]
        [SerializeField, Min(0f)] float detectionRadius = 5f;
        [Tooltip("Gives up when the player is further than this. Keep it larger than the detection radius.")]
        [SerializeField, Min(0f)] float loseRadius = 7f;

        [Header("Combat")]
        [SerializeField, Min(1)] int contactDamage = 1;
        [SerializeField, Min(0f)] float defeatedVanishSeconds = 0.4f;

        public float PatrolSpeed => patrolSpeed;
        public float WaypointPauseSeconds => waypointPauseSeconds;
        public float ChaseSpeed => chaseSpeed;
        public float DetectionRadius => detectionRadius;
        public float LoseRadius => loseRadius;
        public int ContactDamage => contactDamage;
        public float DefeatedVanishSeconds => defeatedVanishSeconds;
    }
}
