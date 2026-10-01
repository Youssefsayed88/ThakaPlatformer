using Thaka.Platformer.Session;
using UnityEngine;
using UnityEngine.UI;

namespace Thaka.Platformer.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] GameSession session;
        [SerializeField] Text hpText;
        [SerializeField] Text coinsText;

        void Start()
        {
            session.State.HpChanged += ShowHp;
            session.State.CoinsChanged += ShowCoins;

            ShowHp(session.State.Hp, session.State.MaxHp);
            ShowCoins(session.State.Coins);
        }

        void OnDestroy()
        {
            if (session == null || session.State == null)
                return;

            session.State.HpChanged -= ShowHp;
            session.State.CoinsChanged -= ShowCoins;
        }

        void ShowHp(int hp, int maxHp)
        {
            hpText.text = $"HP {hp}/{maxHp}";
        }

        void ShowCoins(int coins)
        {
            coinsText.text = $"Coins {coins}";
        }
    }
}
