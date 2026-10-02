using NUnit.Framework;
using Thaka.Platformer.AI;
using Thaka.Platformer.Enemies;
using UnityEngine;

namespace Thaka.Platformer.Tests
{
    public class TargetSensorTests
    {
        class FakeTarget : ITarget
        {
            public Vector3 Position { get; set; }
        }

        GameObject owner;
        FakeTarget target;
        TargetSensor sensor;

        [SetUp]
        public void SetUp()
        {
            owner = new GameObject("Owner");
            target = new FakeTarget();
            sensor = new TargetSensor(owner.transform, detectionRadius: 5f, loseRadius: 7f) { Target = target };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(owner);
        }

        [Test]
        public void TargetInsideDetectionRadius_IsDetected()
        {
            target.Position = new Vector3(4f, 0f, 0f);

            Assert.IsTrue(sensor.CanDetect);
        }

        [Test]
        public void TargetBetweenRadii_IsNotDetectedButNotLost()
        {
            target.Position = new Vector3(6f, 0f, 0f);

            Assert.IsFalse(sensor.CanDetect);
            Assert.IsFalse(sensor.HasLost);
        }

        [Test]
        public void TargetBeyondLoseRadius_IsLost()
        {
            target.Position = new Vector3(8f, 0f, 0f);

            Assert.IsTrue(sensor.HasLost);
        }
    }
}
