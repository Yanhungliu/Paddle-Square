using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
	[SerializeField]
	LivelyCamera livelyCamera;
	
	[SerializeField]
	Ball _currentBall ;

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

	
	
	public void SetCurrentBall(Ball currentBall)
	{
		_currentBall = currentBall;
	}

    void Awake ()
    {
	    _countdownUntilNewGame = newGameDelay;
	    PauseToChoose();
    }

    void StartNewGame ()
	{
		_currentBall.StartNewGame();
		bottomPaddle.StartNewGame();
		topPaddle.StartNewGame();
	}
	
	void Update ()
	{
		bottomPaddle.Move(_currentBall.Position.x, arenaExtents.x);
		topPaddle.Move(_currentBall.Position.x, arenaExtents.x);

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
		_currentBall.Move();
		BounceYIfNeeded();
		BounceXIfNeeded(_currentBall.Position.x);
		_currentBall.UpdateVisualization();
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
		float yExtents = arenaExtents.y - _currentBall.Extents;
		if (_currentBall.Position.y < -yExtents)
		{
			BounceY(-yExtents,bottomPaddle,topPaddle);
		}
		else if (_currentBall.Position.y > yExtents)
		{
			BounceY(yExtents,topPaddle,bottomPaddle);
		} 
	}
	void BounceY (float boundary, Paddle defender,Paddle attacker)
	{
		float durationAfterBounce = (_currentBall.Position.y - boundary) / _currentBall.Velocity.y;
		float bounceX = _currentBall.Position.x - _currentBall.Velocity.x * durationAfterBounce;

		BounceXIfNeeded(bounceX);
		bounceX = _currentBall.Position.x - _currentBall.Velocity.x * durationAfterBounce;
		livelyCamera.PushXZ(_currentBall.Velocity);
		_currentBall.BounceY(boundary);

		
		if (defender.HitBall(bounceX, _currentBall.Extents, out float hitFactor))
		{
			_currentBall.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
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
		_currentBall.EndGame();
		PauseToChoose();
	}

    void BounceXIfNeeded (float x)
	{
		float xExtents = arenaExtents.x - _currentBall.Extents;
		if (x < -xExtents)
		{
			livelyCamera.PushXZ(_currentBall.Velocity);
			_currentBall.BounceX(-xExtents);
		}
		else if (x> xExtents)
		{
			livelyCamera.PushXZ(_currentBall.Velocity);
			_currentBall.BounceX(xExtents);
		}
	}

	private void PauseToChoose()
	{
		Time.timeScale = 0;
		buttons.gameObject.SetActive(true);
	}
}
