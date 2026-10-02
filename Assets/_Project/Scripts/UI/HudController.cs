using System.Collections.Generic;
using Thaka.Platformer.Session;
using UnityEngine;
using UnityEngine.UI;

namespace Thaka.Platformer.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] GameSession session;

        [Header("Health")]
        [SerializeField] Image hpPipTemplate;
        [SerializeField] Color hpFullColor = new Color(0.9f, 0.2f, 0.25f);
        [SerializeField] Color hpEmptyColor = new Color(1f, 1f, 1f, 0.2f);

        [Header("Coins")]
        [SerializeField] Text coinsText;

        [Header("Level Complete")]
        [SerializeField] GameObject levelCompletePanel;
        [SerializeField] Text levelCompleteText;

        readonly List<Image> hpPips = new List<Image>();

        LevelState State => session.State;

        void Start()
        {
            CreateHpPips(State.MaxHp);
            levelCompletePanel.SetActive(false);

            State.HpChanged += ShowHp;
            State.CoinsChanged += ShowCoins;
            session.LevelCompleted += ShowLevelComplete;

            ShowHp(State.Hp, State.MaxHp);
            ShowCoins(State.Coins);
        }

        void OnDestroy()
        {
            if (session == null || State == null)
                return;

            State.HpChanged -= ShowHp;
            State.CoinsChanged -= ShowCoins;
            session.LevelCompleted -= ShowLevelComplete;
        }

        void CreateHpPips(int count)
        {
            hpPipTemplate.gameObject.SetActive(false);
            for (var i = 0; i < count; i++)
            {
                var pip = Instantiate(hpPipTemplate, hpPipTemplate.transform.parent);
                pip.gameObject.SetActive(true);
                hpPips.Add(pip);
            }
        }

        void ShowHp(int hp, int maxHp)
        {
            for (var i = 0; i < hpPips.Count; i++)
                hpPips[i].color = i < hp ? hpFullColor : hpEmptyColor;
        }

        void ShowCoins(int coins)
        {
            coinsText.text = $"{coins} / {session.TotalCoins}";
        }

        void ShowLevelComplete()
        {
            levelCompleteText.text = $"Level Complete!\n<size=40>Coins  {State.Coins} / {session.TotalCoins}</size>";
            levelCompletePanel.SetActive(true);
        }
    }
}
