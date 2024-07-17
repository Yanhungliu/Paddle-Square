using System;
using UnityEngine;

namespace Script
{
    public class PlayerPaddleMover:MonoBehaviour
    {
        [SerializeField] private Paddle paddle;
        private float _speed;

        private void Start()
        {
            _speed = paddle.Speed;
        }


        public float Adjust(float x) // Player Control Panel
        {
            bool goRight = Input.GetKey(KeyCode.RightArrow);
            bool goLeft = Input.GetKey(KeyCode.LeftArrow);
            if (goRight && !goLeft)
            {
                return x +_speed * Time.deltaTime;
            }
            else if (goLeft && !goRight)
            {
                return x - _speed * Time.deltaTime;
            }

            return x;
        }

        
    }
}