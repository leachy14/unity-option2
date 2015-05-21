using UnityEngine;
using System.Collections;
using Level;
public class PauseScreen : MonoBehaviour {

	public GameObject round_level;
	public LevelControl round_accessor;
	// Use this for initialization
	void Start () {


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
		foreach (GameObject bArs in round_accessor.Bars) {
			bArs.SetActive (true);
		}
		foreach (GameObject bOmb in round_accessor.Bombs) {
			bOmb.SetActive (true);
		}
		foreach (GameObject ProJ in round_accessor.Projectiles) {
			ProJ.SetActive (true);
		}
		foreach (GameObject fIre in round_accessor.FireShots) {
			fIre.SetActive (true);
		}
		foreach (GameObject iCes in round_accessor.Ices) {
			iCes.SetActive (true);
		}
		GameObject PauseScreen = GameObject.Find("Pause Screen(Clone)");
		Destroy(PauseScreen);
	}
	public void MainMenu () {
		Application.LoadLevel("Start Screen");
	}
}
