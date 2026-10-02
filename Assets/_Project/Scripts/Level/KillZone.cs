using System;
using UnityEngine;

namespace Thaka.Platformer.Level
{
    [RequireComponent(typeof(BoxCollider))]
    public class KillZone : MonoBehaviour, IPlayerTrigger
    {
        public event Action PlayerEntered;

        public void OnPlayerTouch()
        {
            PlayerEntered?.Invoke();
        }

        void OnDrawGizmos()
        {
            var box = GetComponent<BoxCollider>();
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
        }
    }
}
