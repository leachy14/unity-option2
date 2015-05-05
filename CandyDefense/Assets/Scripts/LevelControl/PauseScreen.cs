using UnityEngine;
using System.Collections;
using Level;
public class PauseScreen : MonoBehaviour {

	public GameObject round_level;
	public LevelControl round_accessor;
	// Use this for initialization
	void Start () {


//			enemies = GameObject.FindGameObjectsWithTag ("Enemy");
//			Turrets = GameObject.FindGameObjectsWithTag ("Turret");

		transform.SetParent(GameObject.Find("Canvas").transform, false);
		round_level = GameObject.Find ("spawner");
		round_accessor = round_level.GetComponent<LevelControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(1f,1f,1f);

	}
	public void PauseOfF () {
		round_accessor.pause = false;
		foreach (GameObject objs in round_accessor.Turrets) {
			objs.SetActive (true);
		}
		foreach (GameObject Enem in round_accessor.enemies) {
			Enem.SetActive (true);
		}
		GameObject PauseScreen = GameObject.Find("Pause Screen(Clone)");
		Destroy(PauseScreen);
	}
	public void MainMenu () {
		Application.LoadLevel("Start Screen");
	}
}
