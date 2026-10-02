using System;
using System.Collections.Generic;

namespace Thaka.Platformer.AI
{
    // States stay unaware of each other; all transitions are declared here by the owner.
    public class StateMachine
    {
        readonly Dictionary<IState, List<Transition>> transitions = new Dictionary<IState, List<Transition>>();
        readonly List<Transition> anyTransitions = new List<Transition>();

        public IState Current { get; private set; }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!transitions.TryGetValue(from, out var list))
            {
                list = new List<Transition>();
                transitions[from] = list;
            }

            list.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            anyTransitions.Add(new Transition(to, condition));
        }

        public void SetState(IState state)
        {
            if (state == Current)
                return;

            Current?.Exit();
            Current = state;
            Current.Enter();
        }

        public void Tick(float deltaTime)
        {
            var next = FindNextState();
            if (next != null)
                SetState(next);

            Current?.Tick(deltaTime);
        }

        IState FindNextState()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.To != Current && transition.Condition())
                    return transition.To;
            }

            if (Current != null && transitions.TryGetValue(Current, out var list))
            {
                foreach (var transition in list)
                {
                    if (transition.Condition())
                        return transition.To;
                }
            }

            return null;
        }

        readonly struct Transition
        {
            public readonly IState To;
            public readonly Func<bool> Condition;

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
    }
}
