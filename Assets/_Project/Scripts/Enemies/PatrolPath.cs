using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    // Horizontal path measured from where the enemy is placed, so it can be tuned without extra waypoint objects
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField, Min(0f)] float leftExtent = 3f;
        [SerializeField, Min(0f)] float rightExtent = 3f;

        Vector3 origin;

        public float LeftX => Origin.x - leftExtent;
        public float RightX => Origin.x + rightExtent;

        Vector3 Origin => Application.isPlaying ? origin : transform.position;

        void Awake()
        {
            origin = transform.position;
        }

        public float Clamp(float x) => Mathf.Clamp(x, LeftX, RightX);

        void OnDrawGizmos()
        {
            var y = Origin.y + 0.1f;
            var left = new Vector3(LeftX, y, Origin.z);
            var right = new Vector3(RightX, y, Origin.z);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(left, right);
            Gizmos.DrawWireSphere(left, 0.15f);
            Gizmos.DrawWireSphere(right, 0.15f);
        }
    }
}
