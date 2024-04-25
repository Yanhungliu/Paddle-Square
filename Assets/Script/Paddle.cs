using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
	bool isAI;


    [SerializeField, Min(0f)]
	float
		extents = 4f,
		speed = 10f;



    public void Move (float target, float arenaExtents)
	{
		Vector3 p = transform.localPosition;
        p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
		float limit = arenaExtents - extents;
		p.x = Mathf.Clamp(p.x, -limit, limit);
		transform.localPosition = p;
	}


    float AdjustByAI (float x, float target)//AI Controll Panal
	{
		if (x < target)
		{
			return Mathf.Min(x + speed * Time.deltaTime, target);
		}
		return Mathf.Max(x - speed * Time.deltaTime, target);
	}


    float AdjustByPlayer (float x)// Player Control Panal
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

    
}
