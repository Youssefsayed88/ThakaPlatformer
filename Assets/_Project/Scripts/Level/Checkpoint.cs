using System;
using Thaka.Platformer.Persistence;
using UnityEngine;

namespace Thaka.Platformer.Level
{
    [RequireComponent(typeof(PersistentId))]
    public class Checkpoint : MonoBehaviour, IPlayerTrigger
    {
        const float SpawnHeightOffset = 0.05f;

        [SerializeField] Renderer flag;
        [SerializeField] Material reachedMaterial;

        PersistentId persistentId;

        public event Action<Checkpoint> Reached;

        public string Id => persistentId.Id;
        public float X => transform.position.x;
        public Vector3 SpawnPosition => transform.position + Vector3.up * SpawnHeightOffset;
        public bool IsReached { get; private set; }

        void Awake()
        {
            persistentId = GetComponent<PersistentId>();
        }

        public void OnPlayerTouch()
        {
            if (!IsReached)
                Reached?.Invoke(this);
        }

        public void MarkReached()
        {
            if (IsReached)
                return;

            IsReached = true;
            flag.sharedMaterial = reachedMaterial;
        }
    }
}
