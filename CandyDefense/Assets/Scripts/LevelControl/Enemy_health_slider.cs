using UnityEngine;
using System.Collections;

public class Enemy_health_slider : MonoBehaviour {


	public Camera main;
	public Transform Enemy_for_this;
	public Vector3 vec;
	// Use this for initialization
	void Start () {
		//Enemy_for_this = GameObject.Find ("Dino_enemy(Clone)");
		Debug.Log(transform.localToWorldMatrix);
		Enemy_for_this = transform.Find ("/Dino_enemy(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		Enemy_for_this = transform.Find ("/Dino_enemy(Clone)");
		vec = Enemy_for_this.TransformVector(transform.position);
		transform.position = vec * 100.2f;


		}

	}	

