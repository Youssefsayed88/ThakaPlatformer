using Thaka.Platformer.Player;
using Thaka.Platformer.Session;
using UnityEngine;

namespace Thaka.Platformer.Audio
{
    // Listens to gameplay events so no gameplay class needs to know about audio
    [RequireComponent(typeof(AudioSource))]
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] SfxLibrary library;
        [SerializeField] GameSession session;
        [SerializeField] PlayerMotor player;

        AudioSource source;
        int lastHp;

        LevelState State => session.State;

        void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        void Start()
        {
            lastHp = State.Hp;

            State.HpChanged += OnHpChanged;
            State.CoinsChanged += OnCoinsChanged;
            State.Died += OnDied;
            session.CheckpointReached += OnCheckpointReached;
            session.LevelCompleted += OnLevelCompleted;
            player.Jumped += OnJumped;
            player.Bounced += OnBounced;
        }

        void OnDestroy()
        {
            if (session != null && State != null)
            {
                State.HpChanged -= OnHpChanged;
                State.CoinsChanged -= OnCoinsChanged;
                State.Died -= OnDied;
                session.CheckpointReached -= OnCheckpointReached;
                session.LevelCompleted -= OnLevelCompleted;
            }

            if (player != null)
            {
                player.Jumped -= OnJumped;
                player.Bounced -= OnBounced;
            }
        }

        void OnHpChanged(int hp, int maxHp)
        {
            // Death has its own sound
            if (hp < lastHp && hp > 0)
                Play(library.Hurt);
            lastHp = hp;
        }

        void OnCoinsChanged(int coins) => Play(library.Coin);
        void OnDied() => Play(library.Death);
        void OnCheckpointReached() => Play(library.Checkpoint);
        void OnLevelCompleted() => Play(library.LevelComplete);
        void OnJumped() => Play(library.Jump);
        void OnBounced() => Play(library.Stomp);

        void Play(AudioClip clip)
        {
            if (clip != null)
                source.PlayOneShot(clip);
        }
    }
}
