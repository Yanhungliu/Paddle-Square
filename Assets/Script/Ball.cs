using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script
{
    public class Ball : MonoBehaviour
    {
        [SerializeField, Min(0f)] float
            maxXSpeed = 20f,
            maxStartXSpeed = 2f,
            constantYSpeed = 10f,
            extents = 0.5f;

        [SerializeField, Min(0f)] Vector2 arenaExtents = new Vector2(10f, 10f);

        [SerializeField] Paddle bottomPaddle, topPaddle;

        [SerializeField] LivelyCamera livelyCamera;
        
        public RectTransform buttons;


        Vector2 _position, _velocity;

        private float Extents => extents;

        public Vector2 Position => _position;

        private Vector2 Velocity => _velocity;
        


        private void UpdateVisualization() =>
            transform.localPosition = new Vector3(_position.x, 0f, _position.y);

        private void Move() => _position += _velocity * Time.deltaTime;


        private void Awake()
        {
            FindObjectOfType<Game>().SendGameModeEventHandler += SetGameMode;
        }

        void Start() => StartNewGame();

        private void Update()
        {
            UpdateGame();
        }


        private void SetGameMode(Difficulty difficulty)
        {
            Debug.Log("Selected game mode: " + difficulty);
            float size;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    maxXSpeed = 10f;
                    maxStartXSpeed = 1f;
                    size = 2;
                    SetAttributes(size, difficulty);

                    break;
                case Difficulty.Normal:
                    maxXSpeed = 30f;
                    maxStartXSpeed = 10f;
                    size = 1;
                    SetAttributes(size, difficulty);
                    break;
                case Difficulty.Hard:
                    maxXSpeed = 50f;
                    maxStartXSpeed = 30f;
                    size = 0.5f;
                    SetAttributes(size, difficulty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }


        private void UpdateGame()
        {
            Move();
            BounceYIfNeeded();
            BounceXIfNeeded(Position.x);
            UpdateVisualization();
        }

        private void StartNewGame()
        {
            _position = Vector2.zero;
            UpdateVisualization();
            _velocity.x = Random.Range(-maxStartXSpeed, maxStartXSpeed);
            _velocity.y = -constantYSpeed;
            gameObject.SetActive(true);
        }

        private void EndGame()
        {
            _position.x = 0f;
            gameObject.SetActive(false);
            buttons.gameObject.SetActive(true);
        }

        private void SetXPositionAndSpeed(float start, float speedFactor, float deltaTime)
        {
            _velocity.x = maxXSpeed * speedFactor;
            _position.x = start + _velocity.x * deltaTime;
        }


        private void BounceX(float boundary)
        {
            _position.x = 2f * boundary - _position.x;
            _velocity.x = -_velocity.x;
        }

        private void BounceY(float boundary)
        {
            _position.y = 2f * boundary - _position.y;
            _velocity.y = -_velocity.y;
        }

        void BounceYIfNeeded()
        {
            float yExtents = arenaExtents.y - Extents;
            if (Position.y < -yExtents)
            {
                BounceY(-yExtents, bottomPaddle, topPaddle);
            }
            else if (Position.y > yExtents)
            {
                BounceY(yExtents, topPaddle, bottomPaddle);
            }
        }

        void BounceY(float boundary, Paddle defender, Paddle attacker)
        {
            float durationAfterBounce = (Position.y - boundary) / Velocity.y;
            float bounceX = Position.x - Velocity.x * durationAfterBounce;

            BounceXIfNeeded(bounceX);
            bounceX = Position.x - Velocity.x * durationAfterBounce;
            livelyCamera.PushXZ(Velocity);
            BounceY(boundary);


            if (defender.HitBall(bounceX, Extents, out float hitFactor))
            {
                SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
            }
            else
            {
                livelyCamera.JostleY();
                if (attacker.ScorePoint(3))
                {
                    EndGame();
                }
            }
        }

        void BounceXIfNeeded(float x)
        {
            float xExtents = arenaExtents.x - Extents;
            if (x < -xExtents)
            {
                livelyCamera.PushXZ(Velocity);
                BounceX(-xExtents);
            }
            else if (x > xExtents)
            {
                livelyCamera.PushXZ(Velocity);
                BounceX(xExtents);
            }
        }


        private void SetAttributes(float sizeTimes, Difficulty difficulty)
        {
            extents = sizeTimes * extents;
            transform.localScale = Vector3.one * sizeTimes;


            GetComponent<Renderer>().material.color = difficulty switch
            {
                Difficulty.Easy => Color.yellow,
                Difficulty.Normal => Color.green,
                Difficulty.Hard => Color.red,
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
            };
        }
    }
}