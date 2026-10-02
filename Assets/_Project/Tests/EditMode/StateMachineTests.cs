using NUnit.Framework;
using Thaka.Platformer.AI;

namespace Thaka.Platformer.Tests
{
    public class StateMachineTests
    {
        class FakeState : IState
        {
            public int Entered;
            public int Exited;
            public int Ticks;

            public void Enter() => Entered++;
            public void Tick(float deltaTime) => Ticks++;
            public void Exit() => Exited++;
        }

        [Test]
        public void SetState_EntersNewState()
        {
            var machine = new StateMachine();
            var idle = new FakeState();

            machine.SetState(idle);

            Assert.AreEqual(idle, machine.Current);
            Assert.AreEqual(1, idle.Entered);
        }

        [Test]
        public void Transition_HappensWhenConditionIsTrue()
        {
            var machine = new StateMachine();
            var idle = new FakeState();
            var chase = new FakeState();
            var playerNear = false;
            machine.AddTransition(idle, chase, () => playerNear);
            machine.SetState(idle);

            machine.Tick(0f);
            Assert.AreEqual(idle, machine.Current);

            playerNear = true;
            machine.Tick(0f);
            Assert.AreEqual(chase, machine.Current);
            Assert.AreEqual(1, idle.Exited);
        }

        [Test]
        public void AnyTransition_WorksFromEveryState()
        {
            var machine = new StateMachine();
            var idle = new FakeState();
            var dead = new FakeState();
            var isDead = false;
            machine.AddAnyTransition(dead, () => isDead);
            machine.SetState(idle);

            isDead = true;
            machine.Tick(0f);

            Assert.AreEqual(dead, machine.Current);
        }
    }
}
