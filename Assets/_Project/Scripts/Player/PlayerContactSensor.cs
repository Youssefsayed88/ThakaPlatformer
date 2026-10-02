using Thaka.Platformer.Config;
using Thaka.Platformer.Level;
using UnityEngine;

namespace Thaka.Platformer.Player
{
    [RequireComponent(typeof(PlayerMotor), typeof(PlayerHealth), typeof(CharacterController))]
    public class PlayerContactSensor : MonoBehaviour
    {
        [SerializeField] PlayerConfig config;

        readonly Collider[] overlaps = new Collider[16];
        CharacterController controller;
        EnemyContactResolver enemies;
        float previousFeetY;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            enemies = new EnemyContactResolver(GetComponent<PlayerMotor>(), GetComponent<PlayerHealth>(), config);
            previousFeetY = transform.position.y;
        }

        // Polled in LateUpdate, after the player and enemies have moved, because trigger callbacks arrive a few frames late
        void LateUpdate()
        {
            Physics.SyncTransforms();

            var count = QueryOverlaps();
            for (var i = 0; i < count; i++)
            {
                var other = overlaps[i];
                if (other.TryGetComponent(out IPlayerTrigger trigger))
                    trigger.OnPlayerTouch();
                else
                    enemies.Resolve(other, previousFeetY);
            }

            previousFeetY = transform.position.y;
        }

        int QueryOverlaps()
        {
            var center = transform.TransformPoint(controller.center);
            var halfHeight = Mathf.Max(controller.height * 0.5f - controller.radius, 0f);
            var bottom = center - Vector3.up * halfHeight;
            var top = center + Vector3.up * halfHeight;
            return Physics.OverlapCapsuleNonAlloc(bottom, top, controller.radius, overlaps, ~0, QueryTriggerInteraction.Collide);
        }
    }
}
