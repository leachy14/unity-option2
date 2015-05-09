using UnityEngine;
using System.Collections;
using Level;
using Store;
using UnityEngine.UI;

public class UpdateLabels : MonoBehaviour {

	public GameObject round_level;
	public LevelControl round_accessor;
	public GameObject store_accessor;
	public StoreControl storecontrol;

	void Start () {
		round_level = GameObject.Find ("spawner");
		round_accessor = round_level.GetComponent<LevelControl> ();
		store_accessor = GameObject.Find ("Store");
		storecontrol = store_accessor.GetComponent <StoreControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		Text Coins_Text = GameObject.Find ("Coins_Text").GetComponent<Text> ();
		Text Level_Text = GameObject.Find ("Round_Text").GetComponent<Text> (); 

		Level_Text.text = round_accessor.wave.ToString();
		Coins_Text.text = storecontrol.Coins.ToString();
	}
}
