using Thaka.Platformer.Config;
using UnityEngine;

namespace Thaka.Platformer.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        const float GroundedVelocity = -2f;

        [SerializeField] PlayerConfig config;
        [SerializeField] PlayerInputReader input;

        CharacterController controller;
        Vector3 velocity;
        float laneZ;
        float lastGroundedTime = float.NegativeInfinity;
        float lastJumpPressedTime = float.NegativeInfinity;
        float controlLockedUntil = float.NegativeInfinity;

        public Vector3 Velocity => velocity;
        public bool IsGrounded => controller.isGrounded;
        bool IsControlLocked => Time.time < controlLockedUntil;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            laneZ = transform.position.z;
        }

        void Update()
        {
            if (controller.isGrounded)
                lastGroundedTime = Time.time;
            if (input.JumpPressed)
                lastJumpPressedTime = Time.time;

            UpdateHorizontal();
            UpdateVertical();

            var move = velocity * Time.deltaTime;
            move.z = laneZ - transform.position.z;
            var flags = controller.Move(move);

            if ((flags & CollisionFlags.Above) != 0 && velocity.y > 0f)
                velocity.y = 0f;

            UpdateFacing();
        }

        public void Teleport(Vector3 position, float yaw)
        {
            // CharacterController overwrites transform changes while enabled
            controller.enabled = false;
            transform.SetPositionAndRotation(position, Quaternion.Euler(0f, yaw, 0f));
            controller.enabled = true;
            velocity = Vector3.zero;
        }

        public void Bounce(float height)
        {
            velocity.y = Mathf.Sqrt(2f * height * -config.Gravity);
            lastGroundedTime = float.NegativeInfinity;
            lastJumpPressedTime = float.NegativeInfinity;
        }

        public void ApplyKnockback(float directionX)
        {
            velocity.x = Mathf.Sign(directionX) * config.KnockbackSpeed;
            velocity.y = config.KnockbackUpSpeed;
            controlLockedUntil = Time.time + config.KnockbackControlLockSeconds;
            lastJumpPressedTime = float.NegativeInfinity;
        }

        void UpdateHorizontal()
        {
            if (IsControlLocked)
                return;

            var target = input.Move * config.MoveSpeed;
            var acceleration = controller.isGrounded ? config.GroundAcceleration : config.AirAcceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, target, acceleration * Time.deltaTime);
        }

        void UpdateVertical()
        {
            if (controller.isGrounded && velocity.y < 0f)
                velocity.y = GroundedVelocity;

            var canJump = Time.time - lastGroundedTime <= config.CoyoteTime;
            var wantsJump = Time.time - lastJumpPressedTime <= config.JumpBufferTime;
            if (canJump && wantsJump && !IsControlLocked)
            {
                velocity.y = Mathf.Sqrt(2f * config.JumpHeight * -config.Gravity);
                lastGroundedTime = float.NegativeInfinity;
                lastJumpPressedTime = float.NegativeInfinity;
            }

            var gravity = config.Gravity;
            if (velocity.y > 0f && !input.JumpHeld && !IsControlLocked)
                gravity *= config.JumpCutGravityMultiplier;

            velocity.y = Mathf.Max(velocity.y + gravity * Time.deltaTime, -config.MaxFallSpeed);
        }

        void UpdateFacing()
        {
            if (!IsControlLocked && Mathf.Abs(input.Move) > 0.01f)
                transform.rotation = Quaternion.LookRotation(input.Move > 0f ? Vector3.right : Vector3.left);
        }
    }
}
