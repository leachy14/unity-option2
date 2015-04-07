using UnityEngine;
using System.Collections;

public class Enemy_health_slider : MonoBehaviour {


	public Camera main;
	public Transform Enemy_for_this;
	public Vector3 vec;	
	public GameObject Bar;

	// Use this for initialization
	void Start () {
		//Enemy_for_this = GameObject.Find ("Dino_enemy(Clone)");
		Debug.Log(transform.localToWorldMatrix);
		Enemy_for_this = transform.Find ("/Dino_enemy(Clone)");
		//Enemy_for_this = transform.Find ("/look for");

		Bar.transform.parent = (GameObject.Find("HealthCoinGroup")).transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Enemy_for_this = transform.Find ("/Dino_enemy(Clone)");
		//Enemy_for_this = transform.Find ("/look for");
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;
				Debug.Log(vec);

		}

	}	

