using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
	public float JumpForce = 6.5f;
	private bool _jumping = false;

	private bool _doubleJumped;

	public GradeScript GradeManager;

	// Update is called once per frame
	private void Update () {

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if(Input.GetButtonDown("Jump")) {
			Jump ();
		}
		#endif
		
		foreach (Touch touch in Input.touches) {

			if (touch.fingerId == 0) {
				if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
					Jump();
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					Debug.Log("First finger left.");
				}
			}
		}

	}

	private void Jump() {
		if (!_jumping)
		{
			_jumping = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, JumpForce);
		}
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
