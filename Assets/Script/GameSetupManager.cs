using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class GameSetupManager : MonoBehaviour
    {
        public delegate void GameModeEventHandler(Difficulty difficulty);
        public event GameModeEventHandler GameModeSelected;

        public Button easyButton;
        public Button normalButton;
        public Button hardButton;
        public RectTransform buttons;

        private void Awake()
        {
            PauseToChoose();
        }

        void Start()
        {
            easyButton.onClick.AddListener(() => SetGame(Difficulty.Easy));
            normalButton.onClick.AddListener(() => SetGame(Difficulty.Normal));
            hardButton.onClick.AddListener(() => SetGame(Difficulty.Hard));
        }


        private void SetGame(Difficulty gameMode)
        {
            GameModeSelected?.Invoke(gameMode);
            ResumeGame();
        }


        private void ResumeGame()
        {
            buttons.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        private void PauseToChoose()
        {
            Time.timeScale = 0;
            buttons.gameObject.SetActive(true);
        }
    }
    
    
    
}