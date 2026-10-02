using UnityEngine;

namespace Thaka.Platformer.CameraRig
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset = new Vector3(0f, 3.5f, -16f);
        [SerializeField, Min(0f)] float smoothTime = 0.2f;
        [Tooltip("How far ahead of the player the camera leans in the facing direction.")]
        [SerializeField, Min(0f)] float lookAhead = 2.5f;
        [SerializeField, Min(0f)] float lookAheadSmoothTime = 0.6f;

        Vector3 velocity;
        float currentLookAhead;
        float lookAheadVelocity;

        void Start()
        {
            SnapToTarget();
        }

        void LateUpdate()
        {
            currentLookAhead = Mathf.SmoothDamp(currentLookAhead, Facing() * lookAhead, ref lookAheadVelocity, lookAheadSmoothTime);
            transform.position = Vector3.SmoothDamp(transform.position, DesiredPosition(), ref velocity, smoothTime);
        }

        public void SnapToTarget()
        {
            currentLookAhead = Facing() * lookAhead;
            lookAheadVelocity = 0f;
            velocity = Vector3.zero;
            transform.position = DesiredPosition();
        }

        float Facing() => target.forward.x >= 0f ? 1f : -1f;

        Vector3 DesiredPosition() => target.position + offset + Vector3.right * currentLookAhead;
    }
}
