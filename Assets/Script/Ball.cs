using UnityEngine;

namespace Script
{
	public class Ball : MonoBehaviour
	{
		[SerializeField, Min(0f)]
		float
			maxXSpeed=20f,
			maxStartXSpeed = 2f,
			startXSpeed = 8f,
			constantYSpeed = 10f,
			extents=0.5f;


		Vector2 _position, _velocity;

		public float Extents => extents;
	
		public Vector2 Position => _position;

		public Vector2 Velocity => _velocity;

    

		public void UpdateVisualization () =>
			transform.localPosition = new Vector3(_position.x, 0f, _position.y);

		public void Move () => _position += _velocity * Time.deltaTime;

		void Awake () => gameObject.SetActive(false);

		

		public void StartNewGame ()
		{
			_position = Vector2.zero;
			UpdateVisualization();
			_velocity.x = Random.Range(-maxStartXSpeed, maxStartXSpeed);
			_velocity.y = -constantYSpeed;
			gameObject.SetActive(true);
		}

		public void EndGame ()
		{
			_position.x = 0f;
			gameObject.SetActive(false);
		}

		public void SetXPositionAndSpeed (float start, float speedFactor, float deltaTime)
		{
			_velocity.x = maxXSpeed * speedFactor;
			_position.x = start + _velocity.x * deltaTime;
		}


		public void BounceX (float boundary)
		{
			_position.x = 2f * boundary - _position.x;
			_velocity.x = -_velocity.x;
		}

		public void BounceY (float boundary)
		{
			_position.y = 2f * boundary - _position.y;
			_velocity.y = -_velocity.y;
		}


		public void SetAttributes(float sizeTimes, BallInfo ballInfo)
		{
			extents = sizeTimes*extents;
			transform.localScale = Vector3.one * sizeTimes;
			
		
			
			GetComponent<Renderer>().material.color =ballInfo switch
			{
				BallInfo.Normal => Color.yellow,
				BallInfo.Color => Color.green,
				_ => Color.white
			};
		}
	}
}

