using UnityEngine;
using System.Collections;
using Level;
using Store;
using UnityEngine.UI;

public class UpdateLabels : MonoBehaviour {

	public GameObject round_level;
	public LevelControl round_accessor;


	void Start () {
		round_level = GameObject.Find ("spawner");
		round_accessor = round_level.GetComponent<LevelControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		Text Coins_Text = GameObject.Find ("Coins_Text").GetComponent<Text> ();
		Text Level_Text = GameObject.Find ("Round_Text").GetComponent<Text> (); 

		Level_Text.text = round_accessor.wave.ToString();
		Coins_Text.text = StoreControl.Coins.ToString();
	}
}
