using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {


	//Textures
	public Texture StartGame;

	//Button pos
	public int StartButX;
	public int StartButY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		StartButX = (Screen.width / 2 - 100);
		StartButY = (Screen.height / 2 + 100);

	}
	void OnGUI() {
		if (GUI.Button(new Rect(StartButX, StartButY, 200, 50), "Start Game(This is a placeholder)")) {
			Application.LoadLevel("Forest");

		}
	}

}