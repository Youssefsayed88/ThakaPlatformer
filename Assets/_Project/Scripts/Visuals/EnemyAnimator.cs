using UnityEngine;

namespace Thaka.Platformer.Visuals
{
    // Derives speed from how far the enemy moved, so it works for any movement state without knowing about the FSM
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;

        Vector3 lastPosition;

        void Start()
        {
            lastPosition = transform.position;
            animator.SetBool(CharacterAnimatorParameters.Grounded, true);
        }

        void LateUpdate()
        {
            if (Time.deltaTime <= 0f)
                return;

            var speed = Mathf.Abs(transform.position.x - lastPosition.x) / Time.deltaTime;
            lastPosition = transform.position;
            animator.SetFloat(CharacterAnimatorParameters.Speed, speed);
        }
    }
}
