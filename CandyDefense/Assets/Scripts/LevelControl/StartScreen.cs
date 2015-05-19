using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {


	//Textures


	//Button pos
	public int HealthDo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	public void StartGame() {
		StartCoroutine (LoadGame(HealthDo));
	}
	IEnumerator LoadGame (int hi) {
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel("LoadingScreen");
	}
	public void ExitGame() {
		Application.Quit();
	}
	public void NewGame() {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt ("Money", 300);
		PlayerPrefs.SetFloat ("Health", 20);
		PlayerPrefs.SetInt ("Round", 0);
		PlayerPrefs.SetInt ("MaxEnemies", 1);
		PlayerPrefs.SetInt ("currentEnemies", 1);
		StartCoroutine (LoadGame(HealthDo));
	}
}