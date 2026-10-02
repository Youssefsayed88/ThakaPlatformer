using Thaka.Platformer.Config;
using Thaka.Platformer.Core;
using Thaka.Platformer.Enemies;
using Thaka.Platformer.Persistence;
using Thaka.Platformer.Player;
using UnityEngine;

namespace Thaka.Platformer.Session
{
    // Runs before other scripts so the state exists in their Awake and the player is placed before the camera snaps
    [DefaultExecutionOrder(-100)]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] PlayerConfig playerConfig;
        [SerializeField] PlayerMotor player;

        SaveData loadedSnapshot;

        public LevelState State { get; private set; }

        void Awake()
        {
            State = new LevelState(playerConfig.MaxHp);

            var launch = GameFlow.PendingLaunch;
            if (launch != LaunchMode.NewGame && GameFlow.SaveService.TryLoad(out loadedSnapshot))
                State.Restore(loadedSnapshot, restoreHp: launch == LaunchMode.Continue);

            State.Died += GameFlow.RespawnAtLastCheckpoint;
        }

        void Start()
        {
            BindEnemies();

            if (loadedSnapshot != null)
                player.Teleport(loadedSnapshot.playerPosition, loadedSnapshot.playerYaw);
            else
                SaveCheckpoint(null);
        }

        void OnDestroy()
        {
            State.Died -= GameFlow.RespawnAtLastCheckpoint;
        }

        public void SaveCheckpoint(string checkpointId)
        {
            State.SetCheckpoint(checkpointId);
            var playerTransform = player.transform;
            GameFlow.SaveService.Save(State.CreateSnapshot(playerTransform.position, playerTransform.eulerAngles.y));
        }

        public void CompleteLevel()
        {
            GameFlow.CompleteLevel();
        }

        void BindEnemies()
        {
            foreach (var enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
            {
                if (State.IsEnemyDefeated(enemy.Id))
                    enemy.gameObject.SetActive(false);
                else
                    enemy.Defeated += OnEnemyDefeated;
            }
        }

        void OnEnemyDefeated(EnemyController enemy)
        {
            enemy.Defeated -= OnEnemyDefeated;
            State.TryDefeatEnemy(enemy.Id);
        }
    }
}
