using UnityEngine;

namespace Thaka.Platformer.Visuals
{
    public static class CharacterAnimatorParameters
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Grounded = Animator.StringToHash("Grounded");
    }
}
