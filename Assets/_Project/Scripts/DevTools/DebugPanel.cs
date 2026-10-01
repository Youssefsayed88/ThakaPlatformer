using System;
using Thaka.Platformer.Core;
using Thaka.Platformer.Session;
using UnityEngine;

namespace Thaka.Platformer.DevTools
{
    // Stand-in for gameplay that doesn't exist yet. Only active in the editor and development builds.
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] GameSession session;

        void Awake()
        {
            if (!Debug.isDebugBuild)
                Destroy(this);
        }

        void OnGUI()
        {
            var scale = Screen.height / 720f;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));

            GUILayout.BeginArea(new Rect(Screen.width / scale - 190, 10, 180, 200), GUI.skin.box);
            GUILayout.Label("Debug");

            if (GUILayout.Button("Take Hit"))
                session.State.TakeHit();
            if (GUILayout.Button("Collect Coin"))
                session.State.TryCollectCoin(Guid.NewGuid().ToString("N"));
            if (GUILayout.Button("Reach Checkpoint"))
                session.SaveCheckpoint("debug-checkpoint");
            if (GUILayout.Button("Complete Level"))
                session.CompleteLevel();
            if (GUILayout.Button("Back to Menu"))
                GameFlow.ReturnToMainMenu();

            GUILayout.EndArea();
        }
    }
}
