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

        public Vector3 Velocity => velocity;
        public bool IsGrounded => controller.isGrounded;

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

        void UpdateHorizontal()
        {
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
            if (canJump && wantsJump)
            {
                velocity.y = Mathf.Sqrt(2f * config.JumpHeight * -config.Gravity);
                lastGroundedTime = float.NegativeInfinity;
                lastJumpPressedTime = float.NegativeInfinity;
            }

            var gravity = config.Gravity;
            if (velocity.y > 0f && !input.JumpHeld)
                gravity *= config.JumpCutGravityMultiplier;

            velocity.y = Mathf.Max(velocity.y + gravity * Time.deltaTime, -config.MaxFallSpeed);
        }

        void UpdateFacing()
        {
            if (Mathf.Abs(input.Move) > 0.01f)
                transform.rotation = Quaternion.LookRotation(input.Move > 0f ? Vector3.right : Vector3.left);
        }
    }
}
