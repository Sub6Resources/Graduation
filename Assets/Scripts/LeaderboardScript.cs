using System.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{

	private DatabaseReference _reference;

	public Text LeaderboardText;
	
	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://graduation-ryanberger.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("Graduation-2a14a91f999d.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("graduation-ryanberger@appspot.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
		
		_reference = FirebaseDatabase.DefaultInstance.RootReference;
		
		_reference.OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted)
			{
				LeaderboardText.text = "Error loading leaderboard. Try again later.";
			}
			else if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				LeaderboardText.text = "LEADERBOARD\n";
				for (int i = (int) snapshot.ChildrenCount - 1; i >= 0; i--)
				{
					var leaderboardEntry = snapshot.Children.ToArray()[i];
					LeaderboardText.text += leaderboardEntry.Child("name").Value.ToString() + ": " + leaderboardEntry.Child("score").Value.ToString() + "\n";
				}
			}
		});
	}

	public void CloseLeaderboard()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
