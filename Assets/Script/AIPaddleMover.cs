using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script
{
    public class AIPaddleMover : MonoBehaviour
    {
        [SerializeField] private Ball currentBall;
        [SerializeField] private Paddle paddle;
        [SerializeField] private float maxTargetingBias = 0.75f;

        private float _extents,_targetingBias,_speed;

        private void Update()
        { 
            _extents = paddle.Extents;
            _speed = paddle.Speed;
        }
        
        
        private void ChangeTargetingBias() =>
            _targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);


        public float Adjust(float x) //AI Control Panel
        {
            float target = currentBall.Position.x;
            target += _targetingBias * _extents;
            if (x < target)
            {
                return Mathf.Min(x + _speed * Time.deltaTime, target);
            }
            return Mathf.Max(x - _speed * Time.deltaTime,target);
        }
    }
}