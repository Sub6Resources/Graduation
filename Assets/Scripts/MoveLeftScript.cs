using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftScript : MonoBehaviour
{

	public float MoveLeftSpeed = 5f;

	// Update is called once per frame
	void Update ()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (-MoveLeftSpeed, GetComponent<Rigidbody2D> ().velocity.y);
	}
}
