using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class Paddle : MonoBehaviour
    {
        [SerializeField] private bool isAI;

        [SerializeField] Ball currentBall;

        [SerializeField] private TextMeshPro scoreText;

        [SerializeField, Min(0f)] Vector2 arenaExtents = new Vector2(10f, 10f);

        private int _score;


        [SerializeField, Min(0f)] float
            minExtents = 1f,
            maxExtents = 4f,
            speed = 10f,
            maxTargetingBias = 0.75f;

        private float _extents, _targetingBias;


        private void Awake() => FindObjectOfType<Game>().SendGameModeEventHandler+= SetGameMode;


        void Update()
        {
            OnPaddleMove(currentBall.Position.x, arenaExtents.x);
        }


        private void SetGameMode(Difficulty difficulty)
        {
            Debug.Log("Paddle event");
            switch (difficulty)
            {
                case Difficulty.Easy:
                    SetExtents(4);
                    break;
                case Difficulty.Normal:
                    SetExtents(2);
                    break;
                case Difficulty.Hard:
                    SetExtents(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }

            SetScore(0);
            ChangeTargetingBias();
        }


        private void OnPaddleMove(float target, float arena)
        {
            Vector3 p = transform.localPosition;
            p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
            float limit = arena - _extents;
            p.x = Mathf.Clamp(p.x, -limit, limit);
            transform.localPosition = p;
        }


        private float AdjustByAI(float x, float target) //AI Control Panel
        {
            target += _targetingBias * _extents;
            if (x < target)
            {
                return Mathf.Min(x + speed * Time.deltaTime, target);
            }

            return Mathf.Max(x - speed * Time.deltaTime, target);
        }


        private float AdjustByPlayer(float x) // Player Control Panel
        {
            bool goRight = Input.GetKey(KeyCode.RightArrow);
            bool goLeft = Input.GetKey(KeyCode.LeftArrow);
            if (goRight && !goLeft)
            {
                return x + speed * Time.deltaTime;
            }
            else if (goLeft && !goRight)
            {
                return x - speed * Time.deltaTime;
            }

            return x;
        }


        public bool HitBall(float ballX, float ballExtents, out float hitFactor)
        {
            ChangeTargetingBias();
            hitFactor =
                (ballX - transform.localPosition.x) /
                (_extents + ballExtents);
            return hitFactor is >= -1f and <= 1f;
        }


        

        public bool ScorePoint(int pointsToWin)
        {
            SetScore(_score + 1, pointsToWin);
            return _score >= pointsToWin;
        }

        private void SetScore(int newScore, float pointsToWin = 1000f)
        {
            _score = newScore;
            scoreText.SetText("{0}", newScore);
            SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointsToWin - 1f)));
            
        }

        private void ChangeTargetingBias() =>
            _targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);


        private void SetExtents(float newExtents)
        {
            maxExtents = newExtents;
            _extents = newExtents;
            Vector3 s = transform.localScale;
            s.x = 2f * newExtents;
            transform.localScale = s;
        }
    }
}