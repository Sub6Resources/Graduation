using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonHandler : MonoBehaviour {

	public void OpenGame()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void OpenLeaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}
}
