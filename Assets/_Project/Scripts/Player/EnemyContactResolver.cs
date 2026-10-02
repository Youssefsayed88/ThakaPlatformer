using Thaka.Platformer.Config;
using Thaka.Platformer.Enemies;
using UnityEngine;

namespace Thaka.Platformer.Player
{
    public class EnemyContactResolver
    {
        readonly PlayerMotor motor;
        readonly PlayerHealth health;
        readonly PlayerConfig config;

        public EnemyContactResolver(PlayerMotor motor, PlayerHealth health, PlayerConfig config)
        {
            this.motor = motor;
            this.health = health;
            this.config = config;
        }

        public void Resolve(Collider other, float previousFeetY)
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
