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
}