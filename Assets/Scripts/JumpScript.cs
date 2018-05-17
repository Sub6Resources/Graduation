using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
	public float JumpForce = 6.5f;
	private bool _jumping;

	private bool _doubleJumped;

	public GradeScript GradeManager;
	public SpawnStuffScript SpawnManager;

	// Update is called once per frame
	private void Update () {
		
		if (Input.GetButtonDown("Jump"))
			Jump();

		foreach (Touch touch in Input.touches)
			if (touch.fingerId == 0)
				if (Input.GetTouch(0).phase == TouchPhase.Began)
					Jump();
	}

	private void Jump() {
		if (_jumping || !SpawnManager.GameRunning) return;
		_jumping = true;
		GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, JumpForce);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.CompareTag("Ground")) {
			Console.WriteLine("Landed");
			_jumping = false;
		}

		else if (other.transform.CompareTag("Soda"))
		{
			GradeManager.AddSoda();
			Destroy(other.gameObject);
		}

		else if (other.transform.CompareTag("Book"))
		{
			GradeManager.AddBook();
			Destroy(other.gameObject);
		}

		else if (other.transform.CompareTag("BadGrade"))
		{
			GradeManager.AddBadGrade();
			Destroy(other.gameObject);
		}
	}

//	private void OnCollisionExit2D(Collision2D other) {
//		if (other.transform.CompareTag("Ground"))
//		{
//			_jumping = true;
//		}
//	}
}
