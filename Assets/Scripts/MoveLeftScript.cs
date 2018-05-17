using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftScript : MonoBehaviour
{

	public float MoveLeftSpeed = 5f;
	public SpawnStuffScript SpawnManager;

	// Update is called once per frame
	void Update ()
	{
		GetComponent<Rigidbody2D>().velocity = SpawnManager.GameRunning ? new Vector2(-MoveLeftSpeed, GetComponent<Rigidbody2D>().velocity.y) : Vector2.zero;
	}
}
