// using TMPro;
// using UnityEngine;
//
// namespace Script
// {
//     public class Game : MonoBehaviour
//     {
//         [SerializeField, Min(2)] int pointsToWin = 3;
//
//         [SerializeField] TextMeshPro countdownText;
//
//         [SerializeField, Min(1f)] float newGameDelay = 3f;
//
//         float _countdownUntilNewGame;
//         [SerializeField] private Transform buttons;
//
//         void Awake()
//         {
//             _countdownUntilNewGame = newGameDelay;
//         }
//
//
//         void Update()
//         {
//             if (_countdownUntilNewGame <= 0f)
//             {
//                 // UpdateGame();
//             }
//             else
//             {
//                 UpdateCountdown();
//             }
//         }
//
//
//         void UpdateCountdown()
//         {
//             _countdownUntilNewGame -= Time.deltaTime;
//             if (_countdownUntilNewGame <= 0f)
//             {
//                 countdownText.gameObject.SetActive(false);
//             }
//             else
//             {
//                 float displayValue = Mathf.Ceil(_countdownUntilNewGame);
//                 if (displayValue < newGameDelay)
//                 {
//                     countdownText.SetText("{0}", displayValue);
//                 }
//             }
//         }
//
//
//         void EndGame()
//         {
//             _countdownUntilNewGame = newGameDelay;
//             countdownText.SetText("GAME OVER");
//             countdownText.gameObject.SetActive(true);
//         }
//     }
// }