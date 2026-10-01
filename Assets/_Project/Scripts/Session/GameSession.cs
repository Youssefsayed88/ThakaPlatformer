using Thaka.Platformer.Core;
using UnityEngine;

namespace Thaka.Platformer.Session
{
    [DefaultExecutionOrder(-100)]
    public class GameSession : MonoBehaviour
    {
        [SerializeField, Min(1)] int maxHp = 4;

        public LevelState State { get; private set; }

        void Awake()
        {
            State = new LevelState(maxHp);

            var launch = GameFlow.PendingLaunch;
            if (launch != LaunchMode.NewGame && GameFlow.SaveService.TryLoad(out var snapshot))
                State.Restore(snapshot, restoreHp: launch == LaunchMode.Continue);
            else
                SaveCheckpoint(null);

            State.Died += GameFlow.RespawnAtLastCheckpoint;
        }

        void OnDestroy()
        {
            State.Died -= GameFlow.RespawnAtLastCheckpoint;
        }

        public void SaveCheckpoint(string checkpointId)
        {
            State.SetCheckpoint(checkpointId);
            // TODO: pass the player's position once the player exists
            GameFlow.SaveService.Save(State.CreateSnapshot(Vector3.zero, 0f));
        }

        public void CompleteLevel()
        {
            GameFlow.CompleteLevel();
        }
    }
}
