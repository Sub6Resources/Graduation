using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GradeScript : MonoBehaviour {

	public int MaxGrade = 100;
	public Text GradeText;
	public GameObject GameOverMenu;
	
	private int _sodaCollected;
	private int _booksCollected;
	private int _badGradesCollected;
	private int _timeOffset;

	public SpawnStuffScript SpawnManager;
	
	// Use this for initialization

	// Update is called once per frame
	void Update ()
	{
		//100 + soda + calc
		float value = MaxGrade + SodaScore() + CalcScore();
		if (value >= 50)
		{
			GradeText.text = "A";
		}
		if (value < 50)
		{
			GradeText.text = "A-";
		}

		if (value < 45)
		{
			GradeText.text = "B+";
		}

		if (value < 40)
		{
			GradeText.text = "B";
		}

		if (value < 30)
		{
			GradeText.text = "B-";
		}

		if (value < 15)
		{
			GradeText.text = "C+";
		}

		if (value < 10)
		{
			GradeText.text = "C";
		}

		if (value < 5)
		{
			GradeText.text = "C-";
		}
		
		if (value <= 0)
		{
			GradeText.text = "";
			SpawnManager.GameRunning = false;
//			gameObject.SetActive(false);
			GameOverMenu.SetActive(true);
		}
		else
		{
			SpawnManager.GameRunning = true;
//			gameObject.SetActive(true);
			GameOverMenu.SetActive(false);
		}
		gameObject.GetComponent<Slider>().value = value;
	}

	public void StartOver()
	{
		MaxGrade = 100;
		_sodaCollected = 0;
		_badGradesCollected = 0;
		_booksCollected = 0;
		_timeOffset = Time.frameCount;
	}

	private float SodaScore()
	{
		//soda = sodas * 5 + ticks * -.01
		return _sodaCollected * 5 + (Time.frameCount - _timeOffset) * -.01f;
	}

	private float CalcScore()
	{
		//calc = calc * 5 - 8 * badGrades + -.05 * ticks
		return _booksCollected * 5 - _badGradesCollected * 8 + (Time.frameCount - _timeOffset) * -.05f;
	}

	public void AddSoda()
	{
		_sodaCollected++;
	}

	public void AddBook()
	{
		_booksCollected++;
	}

	public void AddBadGrade()
	{
		_badGradesCollected++;
	}
}
