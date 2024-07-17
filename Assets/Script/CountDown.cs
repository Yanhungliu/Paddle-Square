using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class CountDown : MonoBehaviour
    {
        public delegate void GameModeEventHandler(Difficulty difficulty);

        public event GameModeEventHandler SendGameModeEventHandler;


        [SerializeField] TextMeshPro countdownText;

        [SerializeField, Min(1f)] float newGameDelay = 3f;

        float _countdownUntilNewGame;

        void Awake()
        {
            FindObjectOfType<GameSetupManager>().GameModeSelected += SendGameMode;
            _countdownUntilNewGame = newGameDelay;
        }

        private void SendGameMode(Difficulty difficulty)
        {
            SendGameModeEventHandler?.Invoke(difficulty);
            StartNewGame();
        }


        void Update()
        {
            if (_countdownUntilNewGame <= 0f)
            {
            }
            else
            {
                UpdateCountdown();
            }
        }


        void UpdateCountdown()
        {
            _countdownUntilNewGame -= Time.deltaTime;
            if (_countdownUntilNewGame <= 0f)
            {
                countdownText.gameObject.SetActive(false);
                _countdownUntilNewGame = newGameDelay;
            }
            else
            {
                float displayValue = Mathf.Ceil(_countdownUntilNewGame);
                if (displayValue < newGameDelay)
                {
                    countdownText.SetText("{0}", displayValue);
                }
            }
        }


        void StartNewGame()
        {
            countdownText.gameObject.SetActive(true);
        }
    }
}