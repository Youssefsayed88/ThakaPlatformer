using Thaka.Platformer.Core;
using Thaka.Platformer.Player;
using Thaka.Platformer.Session;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Thaka.Platformer.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameSession session;
        [SerializeField] PlayerInputReader playerInput;
        [SerializeField] GameObject panel;
        [SerializeField] Button resumeButton;
        [SerializeField] Button mainMenuButton;

        InputAction pauseAction;
        bool levelCompleted;

        public bool IsPaused { get; private set; }

        void Awake()
        {
            pauseAction = new InputAction("Pause", InputActionType.Button);
            pauseAction.AddBinding("<Keyboard>/escape");
            pauseAction.AddBinding("<Gamepad>/start");
        }

        void OnEnable()
        {
            pauseAction.Enable();
        }

        void OnDisable()
        {
            pauseAction.Disable();
        }

        void Start()
        {
            panel.SetActive(false);
            resumeButton.onClick.AddListener(Resume);
            mainMenuButton.onClick.AddListener(GameFlow.ReturnToMainMenu);
            session.LevelCompleted += OnLevelCompleted;
        }

        void OnDestroy()
        {
            pauseAction.Dispose();
            if (session != null)
                session.LevelCompleted -= OnLevelCompleted;
        }

        void Update()
        {
            if (!pauseAction.WasPressedThisFrame() || levelCompleted)
                return;

            if (IsPaused)
                Resume();
            else
                Pause();
        }

        void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
            playerInput.enabled = false;
            panel.SetActive(true);
            resumeButton.Select();
        }

        void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            playerInput.enabled = true;
            panel.SetActive(false);
        }

        void OnLevelCompleted()
        {
            levelCompleted = true;
            if (IsPaused)
                Resume();
        }
    }
}
