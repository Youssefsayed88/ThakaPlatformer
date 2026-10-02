using Thaka.Platformer.Collectibles;
using Thaka.Platformer.Config;
using Thaka.Platformer.Core;
using Thaka.Platformer.Enemies;
using Thaka.Platformer.Level;
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
        Checkpoint[] checkpoints;

        public LevelState State { get; private set; }

        void Awake()
        {
            State = new LevelState(playerConfig.MaxHp);

            // Continue and dying both restore the last checkpoint exactly, HP included
            if (GameFlow.PendingLaunch == LaunchMode.Continue && GameFlow.SaveService.TryLoad(out loadedSnapshot))
                State.Restore(loadedSnapshot);

            State.Died += GameFlow.RespawnAtLastCheckpoint;
        }

        void Start()
        {
            BindEnemies();
            BindCoins();
            BindCheckpoints();
            BindKillZones();
            BindGoals();

            if (loadedSnapshot != null)
                player.Teleport(loadedSnapshot.playerPosition, loadedSnapshot.playerYaw);
            else
                Save(player.transform.position);
        }

        void OnDestroy()
        {
            State.Died -= GameFlow.RespawnAtLastCheckpoint;
        }

        void BindEnemies()
        {
            foreach (var enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
            {
                if (State.IsEnemyDefeated(enemy.Id))
                {
                    enemy.gameObject.SetActive(false);
                    continue;
                }

                enemy.Defeated += OnEnemyDefeated;
                if (enemy is ITargetSeeker seeker)
                    seeker.SetTarget(player);
            }
        }

        void OnEnemyDefeated(EnemyController enemy)
        {
            enemy.Defeated -= OnEnemyDefeated;
            State.TryDefeatEnemy(enemy.Id);
        }

        void BindCoins()
        {
            foreach (var coin in FindObjectsByType<Coin>(FindObjectsSortMode.None))
            {
                if (State.IsCoinCollected(coin.Id))
                    coin.gameObject.SetActive(false);
                else
                    coin.Collected += OnCoinCollected;
            }
        }

        void OnCoinCollected(Coin coin)
        {
            coin.Collected -= OnCoinCollected;
            State.TryCollectCoin(coin.Id, coin.Value);
        }

        void BindCheckpoints()
        {
            checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

            var saved = System.Array.Find(checkpoints, c => c.Id == State.CheckpointId);
            if (saved != null)
                MarkReachedUpTo(saved.X);

            foreach (var checkpoint in checkpoints)
                checkpoint.Reached += OnCheckpointReached;
        }

        // The level runs left to right, so reaching a checkpoint also covers every one before it.
        // Reached checkpoints never fire again, so walking back can't overwrite later progress.
        void OnCheckpointReached(Checkpoint checkpoint)
        {
            MarkReachedUpTo(checkpoint.X);
            State.SetCheckpoint(checkpoint.Id);
            Save(checkpoint.SpawnPosition);
        }

        void MarkReachedUpTo(float x)
        {
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.X <= x)
                    checkpoint.MarkReached();
            }
        }

        void BindKillZones()
        {
            foreach (var killZone in FindObjectsByType<KillZone>(FindObjectsSortMode.None))
                killZone.PlayerEntered += State.Kill;
        }

        void BindGoals()
        {
            foreach (var goal in FindObjectsByType<LevelGoal>(FindObjectsSortMode.None))
                goal.Reached += GameFlow.CompleteLevel;
        }

        void Save(Vector3 playerPosition)
        {
            GameFlow.SaveService.Save(State.CreateSnapshot(playerPosition, player.transform.eulerAngles.y));
        }
    }
}
