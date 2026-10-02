using System;
using Thaka.Platformer.Level;
using Thaka.Platformer.Persistence;
using UnityEngine;

namespace Thaka.Platformer.Collectibles
{
    [RequireComponent(typeof(PersistentId))]
    public class Coin : MonoBehaviour, IPlayerTrigger
    {
        [SerializeField, Min(1)] int value = 1;
        [SerializeField] float spinDegreesPerSecond = 180f;

        PersistentId persistentId;
        bool collected;

        public event Action<Coin> Collected;

        public string Id => persistentId.Id;
        public int Value => value;

        void Awake()
        {
            persistentId = GetComponent<PersistentId>();
        }

        void Update()
        {
            transform.Rotate(0f, spinDegreesPerSecond * Time.deltaTime, 0f, Space.World);
        }

        public void OnPlayerTouch()
        {
            if (collected)
                return;

            collected = true;
            Collected?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
