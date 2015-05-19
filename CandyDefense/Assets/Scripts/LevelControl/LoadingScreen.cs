using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	public int AddNum;
	public string LoadingString = "";
	public int lengthofString;
	public int HealthDo;

	// Use this for initialization
	void Start () {
		//Text Loading_Text = GameObject.Find ("LoadingText").GetComponent<Text> ();
		StartCoroutine (LoadGame(HealthDo));

	}
	
	// Update is called once per frame
	void Update () {
	
		//Text Loading_Text = GameObject.Find ("LoadingText").GetComponent<Text> ();



		//lengthofString = LoadingString.Length;

		/*for (lengthofString = 0; lengthofString < 880; lengthofString++){
			AddNum = Mathf.RoundToInt(Random.value);
			LoadingString = LoadingString + AddNum.ToString();
			Loading_Text.text = LoadingString;


		}
		Application.LoadLevel("Forest");
		/*if (lengthofString < 880) {

		

		
		Loading_Text.text = LoadingString;
		} else {

		}*/

	}
	IEnumerator LoadGame (int hi) {
		Text Loading_Text = GameObject.Find ("LoadingText").GetComponent<Text> ();

		for (lengthofString = 0; lengthofString < 223; lengthofString++){
			AddNum = Mathf.RoundToInt(Random.value);
			LoadingString = LoadingString + AddNum.ToString();
			Loading_Text.text = LoadingString;
			yield return new WaitForSeconds  (0.00000f);

		}
		Application.LoadLevel("Forest");

	}
}
