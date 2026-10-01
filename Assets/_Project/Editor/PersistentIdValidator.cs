using System;
using System.Collections.Generic;
using Thaka.Platformer.Persistence;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thaka.Platformer.Editor
{
    [InitializeOnLoad]
    static class PersistentIdValidator
    {
        static PersistentIdValidator()
        {
            EditorSceneManager.sceneSaving += (scene, _) => Repair(scene);
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                    RepairOpenScenes();
            };
        }

        [MenuItem("Thaka/Validate Persistent IDs")]
        static void ValidateOpenScenes()
        {
            Debug.Log($"Persistent ID validation complete. Repaired {RepairOpenScenes()} id(s).");
        }

        static int RepairOpenScenes()
        {
            var repaired = 0;
            for (var i = 0; i < SceneManager.sceneCount; i++)
                repaired += Repair(SceneManager.GetSceneAt(i));

            return repaired;
        }

        static int Repair(Scene scene)
        {
            var seen = new HashSet<string>();
            var repaired = 0;

            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var persistentId in root.GetComponentsInChildren<PersistentId>(true))
                {
                    if (!string.IsNullOrEmpty(persistentId.Id) && seen.Add(persistentId.Id))
                        continue;

                    var serialized = new SerializedObject(persistentId);
                    var newId = Guid.NewGuid().ToString("N");
                    serialized.FindProperty("id").stringValue = newId;
                    serialized.ApplyModifiedProperties();
                    seen.Add(newId);
                    repaired++;
                }
            }

            return repaired;
        }
    }
}
