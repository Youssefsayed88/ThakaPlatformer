using UnityEngine;

namespace Thaka.Platformer.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Thaka/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Patrol")]
        [SerializeField, Min(0f)] float patrolSpeed = 2.5f;
        [SerializeField, Min(0f)] float waypointPauseSeconds = 0.4f;

        [Header("Combat")]
        [SerializeField, Min(1)] int contactDamage = 1;
        [SerializeField, Min(0f)] float defeatedVanishSeconds = 0.4f;

        public float PatrolSpeed => patrolSpeed;
        public float WaypointPauseSeconds => waypointPauseSeconds;
        public int ContactDamage => contactDamage;
        public float DefeatedVanishSeconds => defeatedVanishSeconds;
    }
}
