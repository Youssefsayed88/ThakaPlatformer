using Thaka.Platformer.AI;
using UnityEngine;

namespace Thaka.Platformer.Enemies
{
    // Detect and lose use different radii so the chaser doesn't flicker between states at the edge
    public class TargetSensor
    {
        readonly Transform owner;
        readonly float detectionRadius;
        readonly float loseRadius;

        public TargetSensor(Transform owner, float detectionRadius, float loseRadius)
        {
            this.owner = owner;
            this.detectionRadius = detectionRadius;
            this.loseRadius = Mathf.Max(loseRadius, detectionRadius);
        }

        public ITarget Target { get; set; }

        public bool CanDetect => Target != null && Distance() <= detectionRadius;
        public bool HasLost => Target == null || Distance() > loseRadius;

        float Distance() => Vector3.Distance(owner.position, Target.Position);
    }
}
