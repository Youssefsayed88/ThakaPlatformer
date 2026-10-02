using NUnit.Framework;
using Thaka.Platformer.Player;

namespace Thaka.Platformer.Tests
{
    public class StompRuleTests
    {
        const float EnemyTop = 1f;
        const float Tolerance = 0.35f;

        [Test]
        public void FallingFromAbove_IsStomp()
        {
            Assert.IsTrue(StompRule.IsStomp(-5f, 1.2f, EnemyTop, Tolerance));
        }

        [Test]
        public void FastFallThatPassedTheTopThisFrame_IsStillStomp()
        {
            Assert.IsTrue(StompRule.IsStomp(-25f, 1.1f, EnemyTop, Tolerance));
        }

        [Test]
        public void TouchingFromTheSide_IsNotStomp()
        {
            Assert.IsFalse(StompRule.IsStomp(-2f, 0f, EnemyTop, Tolerance));
        }

        [Test]
        public void JumpingUpIntoEnemy_IsNotStomp()
        {
            Assert.IsFalse(StompRule.IsStomp(5f, 0.9f, EnemyTop, Tolerance));
        }
    }
}
