using TMPro;
using UnityEngine;

namespace Script
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        LivelyCamera livelyCamera;
        
        [SerializeField]
        Ball currentBall ;

        [SerializeField]
        Paddle bottomPaddle, topPaddle;

        [SerializeField, Min(0f)]
        Vector2 arenaExtents = new Vector2(10f, 10f);

        [SerializeField, Min(2)]
        int pointsToWin = 3;

        [SerializeField]
        TextMeshPro countdownText;
        
        [SerializeField, Min(1f)]
        float newGameDelay = 3f;

        float _countdownUntilNewGame;
        [SerializeField] 
        private Transform buttons;

        void Awake ()
        {
            _countdownUntilNewGame = newGameDelay;
            PauseToChoose();
            
            BallSelector.BallSelected += OnBallSelected;
        }

        void OnDestroy ()
        {
            BallSelector.BallSelected -= OnBallSelected;
        }

        void OnBallSelected(Ball selectedBall)
        {
            SetCurrentBall(selectedBall);
        }

        private void SetCurrentBall(Ball ball)
        {
            this.currentBall = ball;
        }

        void StartNewGame ()
        {
            currentBall.StartNewGame();
            bottomPaddle.StartNewGame();
            topPaddle.StartNewGame();
        }
        
        void Update ()
        {
            bottomPaddle.OnPaddleMove(currentBall.Position.x, arenaExtents.x);
            topPaddle.OnPaddleMove(currentBall.Position.x, arenaExtents.x);

            if (_countdownUntilNewGame <= 0f)
            {
                UpdateGame();
            }
            else
            {
                UpdateCountdown();
            }
        }

        void UpdateGame ()
        {
            currentBall.Move();
            BounceYIfNeeded();
            BounceXIfNeeded(currentBall.Position.x);
            currentBall.UpdateVisualization();
        }

        void UpdateCountdown ()
        {
            _countdownUntilNewGame -= Time.deltaTime;
            if (_countdownUntilNewGame <= 0f)
            {
                countdownText.gameObject.SetActive(false);
                StartNewGame();
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

        void BounceYIfNeeded ()
        {
            float yExtents = arenaExtents.y - currentBall.Extents;
            if (currentBall.Position.y < -yExtents)
            {
                BounceY(-yExtents,bottomPaddle,topPaddle);
            }
            else if (currentBall.Position.y > yExtents)
            {
                BounceY(yExtents,topPaddle,bottomPaddle);
            } 
        }

        void BounceY (float boundary, Paddle defender,Paddle attacker)
        {
            float durationAfterBounce = (currentBall.Position.y - boundary) / currentBall.Velocity.y;
            float bounceX = currentBall.Position.x - currentBall.Velocity.x * durationAfterBounce;

            BounceXIfNeeded(bounceX);
            bounceX = currentBall.Position.x - currentBall.Velocity.x * durationAfterBounce;
            livelyCamera.PushXZ(currentBall.Velocity);
            currentBall.BounceY(boundary);

            
            if (defender.HitBall(bounceX, currentBall.Extents, out float hitFactor))
            {
                currentBall.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
            }
            else 
            {
                livelyCamera.JostleY();
                if (attacker.ScorePoint(pointsToWin))
                {
                    EndGame();
                }

            }

        }

        void EndGame ()
        {
            _countdownUntilNewGame = newGameDelay;
            countdownText.SetText("GAME OVER");
            countdownText.gameObject.SetActive(true);
            currentBall.EndGame();
            PauseToChoose();
        }

        void BounceXIfNeeded (float x)
        {
            float xExtents = arenaExtents.x - currentBall.Extents;
            if (x < -xExtents)
            {
                livelyCamera.PushXZ(currentBall.Velocity);
                currentBall.BounceX(-xExtents);
            }
            else if (x> xExtents)
            {
                livelyCamera.PushXZ(currentBall.Velocity);
                currentBall.BounceX(xExtents);
            }
        }

        private void PauseToChoose()
        {
            Time.timeScale = 0;
            buttons.gameObject.SetActive(true);
        }
    }
}
