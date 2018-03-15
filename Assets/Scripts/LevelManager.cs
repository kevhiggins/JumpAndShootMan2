using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private int activeSceneIndex;
	public float autoLoadNextSceneTime;

	void Start ()
	{
		Scene activeScene = SceneManager.GetActiveScene ();
		activeSceneIndex = activeScene.buildIndex;

		if (autoLoadNextSceneTime > 0) {
			Invoke ("LoadNextLevel", autoLoadNextSceneTime);
		}
	}
		

	public void LoadLevel (string name){
		Debug.Log ("Load level requested for : " + name);
		SceneManager.LoadScene(name);
	}

	public void LoadNextLevel ()
	{
		Debug.Log ("Loading next level in index");
		SceneManager.LoadSceneAsync(activeSceneIndex+1);
	}

	public void QuitRequest(){
		Debug.Log("Quit request detected");
		Application.Quit();
	}

}
