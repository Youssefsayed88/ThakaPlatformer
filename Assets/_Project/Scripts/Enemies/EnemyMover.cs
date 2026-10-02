using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    public class EnemyMover
    {
        readonly Transform transform;

        public EnemyMover(Transform transform)
        {
            this.transform = transform;
        }

        public bool IsFacingRight => transform.forward.x >= 0f;

        // Returns true once the target has been reached
        public bool MoveTowards(float targetX, float speed, float deltaTime)
        {
            var position = transform.position;
            Face(targetX - position.x);
            position.x = Mathf.MoveTowards(position.x, targetX, speed * deltaTime);
            transform.position = position;
            return Mathf.Approximately(position.x, targetX);
        }

        public void Face(float directionX)
        {
            if (Mathf.Abs(directionX) > 0.001f)
                transform.rotation = Quaternion.LookRotation(directionX > 0f ? Vector3.right : Vector3.left);
        }
    }
}
