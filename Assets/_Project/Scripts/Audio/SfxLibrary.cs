using UnityEngine;

namespace Thaka.Platformer.Audio
{
    [CreateAssetMenu(fileName = "SfxLibrary", menuName = "Thaka/Sfx Library")]
    public class SfxLibrary : ScriptableObject
    {
        [SerializeField] AudioClip jump;
        [SerializeField] AudioClip coin;
        [SerializeField] AudioClip stomp;
        [SerializeField] AudioClip hurt;
        [SerializeField] AudioClip death;
        [SerializeField] AudioClip checkpoint;
        [SerializeField] AudioClip levelComplete;

        public AudioClip Jump => jump;
        public AudioClip Coin => coin;
        public AudioClip Stomp => stomp;
        public AudioClip Hurt => hurt;
        public AudioClip Death => death;
        public AudioClip Checkpoint => checkpoint;
        public AudioClip LevelComplete => levelComplete;
    }
}
