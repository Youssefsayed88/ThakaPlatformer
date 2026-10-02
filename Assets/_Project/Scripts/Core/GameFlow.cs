using Thaka.Platformer.Persistence;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thaka.Platformer.Core
{
    public static class GameFlow
    {
        static ISaveService saveService;

        public static ISaveService SaveService => saveService ??= new JsonFileSaveService();

        public static LaunchMode PendingLaunch { get; private set; } = LaunchMode.NewGame;

        public static void StartNewGame()
        {
            SaveService.Delete();
            LoadLevel(LaunchMode.NewGame);
        }

        public static void ContinueGame()
        {
            if (!SaveService.HasSave)
            {
                StartNewGame();
                return;
            }

            LoadLevel(LaunchMode.Continue);
        }

        public static void RespawnAtLastCheckpoint()
        {
            LoadLevel(SaveService.HasSave ? LaunchMode.Continue : LaunchMode.NewGame);
        }

        public static void CompleteLevel()
        {
            SaveService.Delete();
            ReturnToMainMenu();
        }

        public static void ReturnToMainMenu()
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        static void LoadLevel(LaunchMode mode)
        {
            PendingLaunch = mode;
            SceneManager.LoadScene(SceneNames.Level);
        }

        // Needed when domain reload is disabled
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            saveService = null;
            PendingLaunch = LaunchMode.NewGame;
        }
    }
}
