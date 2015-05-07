using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	public int AddNum;
	public string LoadingString = "";
	public int lengthofString;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Text Loading_Text = GameObject.Find ("LoadingText").GetComponent<Text> ();



		lengthofString = LoadingString.Length;
		if (lengthofString < 880) {

		
		AddNum = Mathf.RoundToInt(Random.value);
		
		LoadingString = LoadingString + AddNum.ToString();
		
		Loading_Text.text = LoadingString;
		} else {
			Application.LoadLevel("Forest");
		}

	}
}
