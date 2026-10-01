using System;
using UnityEngine;

namespace Thaka.Platformer.Persistence
{
    [DisallowMultipleComponent]
    public sealed class PersistentId : MonoBehaviour
    {
        [SerializeField] string id;

        public string Id => id;

        void Reset()
        {
            id = Guid.NewGuid().ToString("N");
        }
    }
}
