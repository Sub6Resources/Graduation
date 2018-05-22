using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class GradeScript : MonoBehaviour {

	public int MaxGrade = 100;
	public Text GradeText;
	public GameObject GameOverMenu;
	
	private int _sodaCollected;
	private int _booksCollected;
	private int _badGradesCollected;
	private int _timeOffset;
	private int _latestTimeSnapshot;

	public SpawnStuffScript SpawnManager;

	private DatabaseReference reference;
	private Firebase.Auth.FirebaseAuth auth;

	public InputField NameInput;
	public GameObject LeaderboardDialog;
	private int _currentScore;
	
	// Use this for initialization
	void Start ()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://graduation-ryanberger.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("Graduation-2a14a91f999d.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("graduation-ryanberger@appspot.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		if(auth.CurrentUser != null) {}
		else
		{
			auth.SignInAnonymouslyAsync().ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("SignInAnonymouslyAsync was canceled.");
					return;
				}

				if (task.IsFaulted)
				{
					Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("Signed In!");
			});
		}

		StartOver();
	}

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
		
		if (value <= 0 && SpawnManager.GameRunning)
		{
			GradeText.text = "";
			SpawnManager.GameRunning = false;
			GameOverMenu.SetActive(true);
			
			_latestTimeSnapshot = Time.frameCount - _timeOffset;
//			_currentScore = (_sodaCollected + _booksCollected + _latestTimeSnapshot) * _latestTimeSnapshot /
//			               ((_badGradesCollected + 1) * 5);
			_currentScore = _latestTimeSnapshot;
			
			reference.OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					// Do something with snapshot...
					foreach (var leaderboardEntry in snapshot.Children)
					{
						if (Int32.Parse(leaderboardEntry.Child("score").Value.ToString()) < _currentScore)
						{
							LeaderboardDialog.SetActive(true);
						}
					}
				}
			});
		}
		else if(value > 0)
		{
			SpawnManager.GameRunning = true;
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

	public void MainMenu()
	{
		StartOver();
		SceneManager.LoadScene("MainMenu");
	}

	public void SubmitLeaderboard()
	{
		if (SubmitLeaderboardLogic())
		{
			LeaderboardDialog.SetActive(false);
		}
	}

	private bool SubmitLeaderboardLogic()
	{
		var text = NameInput.text;
		if (text.Length < 2) return false;
		if (auth.CurrentUser != null)
		{
			newLeaderboardEntry(reference.Push(), text, _currentScore);
		}
		else
		{
			return false;
		}

		return true;

	}
	
	private class LeaderboardEntry {
		public string name;
		public int score;

		public LeaderboardEntry() {
		}

		public LeaderboardEntry(string name, int score) {
			this.name = name;
			this.score = score;
		}
	}
	
	private void newLeaderboardEntry(DatabaseReference id, string playerName, int score)
	{
		LeaderboardEntry user = new LeaderboardEntry(playerName, score);
		string json = JsonUtility.ToJson(user);

		id.SetRawJsonValueAsync(json);
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
