using NUnit.Framework;
using Thaka.Platformer.Session;
using UnityEngine;

namespace Thaka.Platformer.Tests
{
    public class LevelStateTests
    {
        [Test]
        public void FourHits_KillThePlayer()
        {
            var state = new LevelState(4);

            state.TakeHit();
            state.TakeHit();
            state.TakeHit();
            Assert.IsFalse(state.IsDead);

            state.TakeHit();
            Assert.IsTrue(state.IsDead);
        }

        [Test]
        public void SameCoin_CountsOnce()
        {
            var state = new LevelState(4);

            state.TryCollectCoin("coin");
            state.TryCollectCoin("coin");

            Assert.AreEqual(1, state.Coins);
        }

        [Test]
        public void Restore_BringsBackSnapshot()
        {
            var state = new LevelState(4);
            state.TryCollectCoin("coin");
            state.TryDefeatEnemy("enemy");
            state.TakeHit();
            var snapshot = state.CreateSnapshot(Vector3.zero, 0f);

            var restored = new LevelState(4);
            restored.Restore(snapshot, restoreHp: true);

            Assert.AreEqual(3, restored.Hp);
            Assert.AreEqual(1, restored.Coins);
            Assert.IsTrue(restored.IsCoinCollected("coin"));
            Assert.IsTrue(restored.IsEnemyDefeated("enemy"));
        }

        [Test]
        public void Respawn_RefillsHp()
        {
            var state = new LevelState(4);
            state.TakeHit();
            var snapshot = state.CreateSnapshot(Vector3.zero, 0f);

            state.Restore(snapshot, restoreHp: false);

            Assert.AreEqual(4, state.Hp);
        }
    }
}
