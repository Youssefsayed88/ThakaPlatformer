using System;
using System.Collections.Generic;
using System.Linq;
using Thaka.Platformer.Persistence;
using UnityEngine;

namespace Thaka.Platformer.Session
{
    public sealed class LevelState
    {
        readonly HashSet<string> collectedCoinIds = new HashSet<string>();
        readonly HashSet<string> defeatedEnemyIds = new HashSet<string>();

        public LevelState(int maxHp)
        {
            if (maxHp <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHp), "Max HP must be positive.");

            MaxHp = maxHp;
            Hp = maxHp;
        }

        public event Action<int, int> HpChanged;

        public event Action<int> CoinsChanged;

        public event Action Died;

        public int MaxHp { get; }

        public int Hp { get; private set; }

        public int Coins { get; private set; }

        public string CheckpointId { get; private set; }

        public bool IsDead => Hp <= 0;

        public bool IsCoinCollected(string coinId) => collectedCoinIds.Contains(coinId);

        public bool IsEnemyDefeated(string enemyId) => defeatedEnemyIds.Contains(enemyId);

        public bool TryCollectCoin(string coinId, int value = 1)
        {
            if (!collectedCoinIds.Add(coinId))
                return false;

            Coins += value;
            CoinsChanged?.Invoke(Coins);
            return true;
        }

        public bool TryDefeatEnemy(string enemyId) => defeatedEnemyIds.Add(enemyId);

        public void TakeHit(int damage = 1)
        {
            if (IsDead || damage <= 0)
                return;

            Hp = Math.Max(0, Hp - damage);
            HpChanged?.Invoke(Hp, MaxHp);
            if (IsDead)
                Died?.Invoke();
        }

        public void SetCheckpoint(string checkpointId)
        {
            CheckpointId = checkpointId;
        }

        // Memento: LevelState is the originator, SaveData the memento, ISaveService the caretaker
        public SaveData CreateSnapshot(Vector3 playerPosition, float playerYaw)
        {
            return new SaveData
            {
                checkpointId = CheckpointId,
                playerPosition = playerPosition,
                playerYaw = playerYaw,
                hp = Hp,
                coins = Coins,
                collectedCoinIds = collectedCoinIds.ToList(),
                defeatedEnemyIds = defeatedEnemyIds.ToList()
            };
        }

        public void Restore(SaveData snapshot, bool restoreHp)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            CheckpointId = snapshot.checkpointId;
            Hp = restoreHp ? Mathf.Clamp(snapshot.hp, 1, MaxHp) : MaxHp;
            Coins = snapshot.coins;

            collectedCoinIds.Clear();
            collectedCoinIds.UnionWith(snapshot.collectedCoinIds);
            defeatedEnemyIds.Clear();
            defeatedEnemyIds.UnionWith(snapshot.defeatedEnemyIds);

            HpChanged?.Invoke(Hp, MaxHp);
            CoinsChanged?.Invoke(Coins);
        }
    }
}
