using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class Score :MonoBehaviour
    {
        private int _score;
        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private Paddle paddle;
        
        
        
        public bool ScorePoint(int pointsToWin)
        {
            SetScore(_score + 1, pointsToWin);
            return _score >= pointsToWin;
        }

        public void SetScore(int newScore, float pointsToWin = 1000f)
        {
            _score = newScore;
            scoreText.SetText("{0}", newScore);
            paddle.SetExtents(Mathf.Lerp( paddle.MaxExtents, paddle.MinExtents, newScore / (pointsToWin - 1f)));
        }
        
        
       public void EndGame()
       {
           Time.timeScale = 0;
       }
        
    }
}