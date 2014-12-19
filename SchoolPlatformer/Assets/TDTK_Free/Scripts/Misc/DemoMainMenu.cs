using UnityEngine;
using System.Collections;

public class DemoMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80, 100, 30), "Demo 1")){
			Application.LoadLevel("ExampleScene1");
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80+45, 100, 30), "Demo 2")){
			Application.LoadLevel("ExampleScene2");
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80+90, 100, 30), "Demo 3")){
			Application.LoadLevel("ExampleScene3");
		}
		//~ if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80+135, 100, 30), "Demo 4")){
			//~ Application.LoadLevel("ExampleScene4");
		//~ }
		//~ if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80+180, 100, 30), "Demo 1 nGUI")){
			//~ Application.LoadLevel("ExampleScene1(nGUI)");
		//~ }
		//~ if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-80+225, 100, 30), "Demo 2 nGUI")){
			//~ Application.LoadLevel("ExampleScene2(nGUI)");
		//~ }
	}
}
