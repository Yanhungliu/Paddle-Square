using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
   [SerializeField]
	Ball ball;

	[SerializeField]
	Paddle bottomPaddle, topPaddle;
    [SerializeField, Min(0f)]
	Vector2 arenaExtents = new Vector2(10f, 10f);

	[SerializeField, Min(2)]
	int pointsToWin = 3;


    void Awake () => StartNewGame();
	void StartNewGame ()
	{
		ball.StartNewGame();
		bottomPaddle.StartNewGame();
		topPaddle.StartNewGame();
	}
	
	void Update ()
	{
		bottomPaddle.Move(ball.Position.x, arenaExtents.x);
		topPaddle.Move(ball.Position.x, arenaExtents.x);
		ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
		ball.UpdateVisualization();
	}

    void BounceYIfNeeded ()
	{
		float yExtents = arenaExtents.y - ball.Extents;
		if (ball.Position.y < -yExtents)
		{
			BounceY(-yExtents,bottomPaddle,topPaddle);
		}
		else if (ball.Position.y > yExtents)
		{
			BounceY(yExtents,topPaddle,bottomPaddle);
		} 
	}
	void BounceY (float boundary, Paddle defender,Paddle attacker)
	{
		float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
		float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

		BounceXIfNeeded(bounceX);
		bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
		ball.BounceY(boundary);

		
		if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
		{
			ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
		}
		else if (attacker.ScorePoint(pointsToWin))
		{
			StartNewGame();
		}

	}

    void BounceXIfNeeded (float x)
	{
		float xExtents = arenaExtents.x - ball.Extents;
		if (x < -xExtents)
		{
			ball.BounceX(-xExtents);
		}
		else if (x> xExtents)
		{
			ball.BounceX(xExtents);
		}
	}
}
