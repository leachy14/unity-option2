using UnityEngine;
using System.Collections;

public class Enemy_health_slider : MonoBehaviour {


	public Camera main;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vec = main.ScreenToWorldPoint(GameObject.Find("Dino_enemy(Clone)").transform.position);
		transform.position = vec;
		transform.localPosition = new Vector2 (0, -0.1f);
	}
}
