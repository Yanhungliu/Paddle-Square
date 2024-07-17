using System;
using UnityEngine;


namespace Script
{
    public class Paddle : MonoBehaviour
    {
        [SerializeField] private bool isAI;
        [SerializeField] private AIPaddleMover aiPaddleMover = new();
        [SerializeField] private PlayerPaddleMover playerPaddleMover = new();

        [SerializeField, Min(0f)] private float
            minExtents = 1f,
            maxExtents = 4f,
            speed = 10f;

        private float _extents;


        private void Awake()
        {
            FindObjectOfType<CountDown>().SendGameModeEventHandler += SetGameMode;
        }


        void Update()
        {
            OnPaddleMove(Arena.ArenaExtent.x);
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
        }


        private void OnPaddleMove(float arena)
        {
            Vector3 p = transform.localPosition;
            p.x = isAI ? aiPaddleMover.Adjust(p.x) : playerPaddleMover.Adjust(p.x);
            float limit = arena - _extents;
            p.x = Mathf.Clamp(p.x, -limit, limit);
            transform.localPosition = p;
        }


        public bool HasHitBall(float ballX, float ballExtents, out float hitFactor)
        {
            hitFactor =
                (ballX - transform.localPosition.x) /
                (_extents + ballExtents);
            return hitFactor is >= -1f and <= 1f;
        }
        


        public void SetExtents(float newExtents)
        {
            maxExtents = newExtents;
            _extents = newExtents;
            Vector3 s = transform.localScale;
            s.x = 2f * newExtents;
            transform.localScale = s;
        }

        public float Extents => _extents;
        public float Speed => speed;

        public float MaxExtents => maxExtents;
        public float MinExtents => minExtents;
    }
}