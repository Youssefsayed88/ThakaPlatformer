using Thaka.Platformer.Config;
using Thaka.Platformer.Session;
using UnityEngine;

namespace Thaka.Platformer.Player
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] PlayerConfig config;
        [SerializeField] GameSession session;

        PlayerMotor motor;
        Renderer[] renderers;
        float invulnerableUntil = float.NegativeInfinity;

        public bool IsInvulnerable => Time.time < invulnerableUntil;

        void Awake()
        {
            motor = GetComponent<PlayerMotor>();
            renderers = GetComponentsInChildren<Renderer>();
        }

        void Update()
        {
            var visible = !IsInvulnerable || Mathf.Repeat(Time.time, config.BlinkInterval * 2f) < config.BlinkInterval;
            foreach (var r in renderers)
                r.enabled = visible;
        }

        public bool TryTakeHit(int damage, Vector3 sourcePosition)
        {
            var state = session.State;
            if (IsInvulnerable || state.IsDead)
                return false;

            state.TakeHit(damage);
            if (state.IsDead)
                return true;

            invulnerableUntil = Time.time + config.InvulnerabilitySeconds;
            motor.ApplyKnockback(transform.position.x - sourcePosition.x);
            return true;
        }
    }
}
