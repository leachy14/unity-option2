using UnityEngine;
using System.Collections;

public class Enemy_health_slider : MonoBehaviour {


	public Camera main;
	public GameObject Enemy_for_this;
	// Use this for initialization
	void Start () {
		Enemy_for_this = GameObject.Find ("Dino_enemy(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		Enemy_for_this = GameObject.Find ("Dino_enemy(Clone)");
		Vector3 vec = main.ScreenToWorldPoint(Enemy_for_this.transform.position);

		transform.localPosition = new Vector3(vec.x, vec.y, -6f);
		//transform.Translate
	}	
}
