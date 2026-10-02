using System;
using UnityEngine;

namespace Thaka.Platformer.Level
{
    public class LevelGoal : MonoBehaviour, IPlayerTrigger
    {
        bool reached;

        public event Action Reached;

        public void OnPlayerTouch()
        {
            if (reached)
                return;

            reached = true;
            Reached?.Invoke();
        }
    }
}
