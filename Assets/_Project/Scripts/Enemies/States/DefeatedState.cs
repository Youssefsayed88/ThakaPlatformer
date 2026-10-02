using Thaka.Platformer.AI;
using UnityEngine;

namespace Thaka.Platformer.Enemies.States
{
    public class DefeatedState : IState
    {
        const float SquashedHeight = 0.25f;

        readonly GameObject gameObject;
        readonly Collider collider;
        readonly float vanishSeconds;

        float timer;

        public DefeatedState(GameObject gameObject, Collider collider, float vanishSeconds)
        {
            this.gameObject = gameObject;
            this.collider = collider;
            this.vanishSeconds = vanishSeconds;
        }

        public void Enter()
        {
            collider.enabled = false;
            var scale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(scale.x * 1.2f, scale.y * SquashedHeight, scale.z);
            timer = vanishSeconds;
        }

        public void Tick(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0f)
                gameObject.SetActive(false);
        }

        public void Exit()
        {
        }
    }
}
