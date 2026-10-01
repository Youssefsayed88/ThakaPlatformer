using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thaka.Platformer.Persistence
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;
        public string checkpointId;
        public Vector3 playerPosition;
        public float playerYaw;
        public int hp;
        public int coins;
        public List<string> collectedCoinIds = new List<string>();
        public List<string> defeatedEnemyIds = new List<string>();
    }
}
