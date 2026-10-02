using Thaka.Platformer.Player;
using UnityEngine;

namespace Thaka.Platformer.Visuals
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        PlayerMotor motor;

        void Awake()
        {
            motor = GetComponent<PlayerMotor>();
        }

        void LateUpdate()
        {
            animator.SetFloat(CharacterAnimatorParameters.Speed, Mathf.Abs(motor.Velocity.x));
            animator.SetBool(CharacterAnimatorParameters.Grounded, motor.IsGrounded);
        }
    }
}
