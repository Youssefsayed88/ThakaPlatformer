using Thaka.Platformer.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Thaka.Platformer.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] Button newGameButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button quitButton;

        void Start()
        {
            continueButton.interactable = GameFlow.SaveService.HasSave;

            newGameButton.onClick.AddListener(GameFlow.StartNewGame);
            continueButton.onClick.AddListener(GameFlow.ContinueGame);
            quitButton.onClick.AddListener(GameFlow.Quit);
        }
    }
}
