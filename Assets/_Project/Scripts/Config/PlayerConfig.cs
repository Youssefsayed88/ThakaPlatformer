using UnityEngine;

namespace Thaka.Platformer.Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Thaka/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Health")]
        [SerializeField, Min(1)] int maxHp = 4;
        [SerializeField, Min(0f)] float invulnerabilitySeconds = 1f;
        [SerializeField, Min(0.01f)] float blinkInterval = 0.1f;

        [Header("Knockback")]
        [SerializeField, Min(0f)] float knockbackSpeed = 8f;
        [SerializeField, Min(0f)] float knockbackUpSpeed = 6f;
        [Tooltip("How long movement input is ignored after being hit.")]
        [SerializeField, Min(0f)] float knockbackControlLockSeconds = 0.25f;

        [Header("Movement")]
        [SerializeField, Min(0f)] float moveSpeed = 7f;
        [SerializeField, Min(0f)] float groundAcceleration = 70f;
        [SerializeField, Min(0f)] float airAcceleration = 40f;

        [Header("Jumping")]
        [SerializeField, Min(0f)] float jumpHeight = 2.5f;
        [SerializeField] float gravity = -35f;
        [Tooltip("Extra gravity while rising with the jump button released, for short hops.")]
        [SerializeField, Min(1f)] float jumpCutGravityMultiplier = 2.5f;
        [SerializeField, Min(0f)] float maxFallSpeed = 25f;
        [SerializeField, Min(0f)] float coyoteTime = 0.1f;
        [SerializeField, Min(0f)] float jumpBufferTime = 0.12f;

        public int MaxHp => maxHp;
        public float InvulnerabilitySeconds => invulnerabilitySeconds;
        public float BlinkInterval => blinkInterval;
        public float KnockbackSpeed => knockbackSpeed;
        public float KnockbackUpSpeed => knockbackUpSpeed;
        public float KnockbackControlLockSeconds => knockbackControlLockSeconds;
        public float MoveSpeed => moveSpeed;
        public float GroundAcceleration => groundAcceleration;
        public float AirAcceleration => airAcceleration;
        public float JumpHeight => jumpHeight;
        public float Gravity => gravity;
        public float JumpCutGravityMultiplier => jumpCutGravityMultiplier;
        public float MaxFallSpeed => maxFallSpeed;
        public float CoyoteTime => coyoteTime;
        public float JumpBufferTime => jumpBufferTime;
    }
}
