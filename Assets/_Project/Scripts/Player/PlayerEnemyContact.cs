using Thaka.Platformer.Config;
using Thaka.Platformer.Enemies;
using UnityEngine;

namespace Thaka.Platformer.Player
{
    [RequireComponent(typeof(PlayerMotor), typeof(PlayerHealth), typeof(CharacterController))]
    public class PlayerEnemyContact : MonoBehaviour
    {
        [SerializeField] PlayerConfig config;

        readonly Collider[] overlaps = new Collider[8];
        PlayerMotor motor;
        PlayerHealth health;
        CharacterController controller;
        float previousFeetY;

        void Awake()
        {
            motor = GetComponent<PlayerMotor>();
            health = GetComponent<PlayerHealth>();
            controller = GetComponent<CharacterController>();
            previousFeetY = transform.position.y;
        }

        // Polled in LateUpdate, after the player and enemies have moved, instead of waiting for trigger callbacks
        void LateUpdate()
        {
            Physics.SyncTransforms();

            var center = transform.TransformPoint(controller.center);
            var halfHeight = Mathf.Max(controller.height * 0.5f - controller.radius, 0f);
            var bottom = center - Vector3.up * halfHeight;
            var top = center + Vector3.up * halfHeight;
            var count = Physics.OverlapCapsuleNonAlloc(bottom, top, controller.radius, overlaps, ~0, QueryTriggerInteraction.Collide);

            for (var i = 0; i < count; i++)
                Resolve(overlaps[i]);

            previousFeetY = transform.position.y;
        }

        void Resolve(Collider other)
        {
            if (other.TryGetComponent(out IStompable stompable))
            {
                if (!stompable.IsAlive)
                    return;

                if (StompRule.IsStomp(motor.Velocity.y, previousFeetY, other.bounds.max.y, config.StompTolerance))
                {
                    stompable.Stomp();
                    motor.Bounce(config.StompBounceHeight);
                    return;
                }
            }

            if (other.TryGetComponent(out IContactDamage damage))
                health.TryTakeHit(damage.ContactDamage, other.transform.position);
        }
    }
}
