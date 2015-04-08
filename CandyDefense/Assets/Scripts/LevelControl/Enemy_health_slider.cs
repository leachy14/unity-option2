using UnityEngine;
using System.Collections;

public class Enemy_health_slider : MonoBehaviour {


	public Camera main;
	public Transform Enemy_for_this;
	public Vector3 vec;	
	public GameObject Bar;
	public GameObject[] Dinos;
	public GameObject[] Bars;
	public int Hi = 0;
	// Use this for initialization
	void Start () {

		Debug.Log(transform.localToWorldMatrix);
		Dinos = GameObject.FindGameObjectsWithTag("Enemy");
		//Bars = GameObject.FindGameObjectsWithTag("Bar");

		//Enemy_for_this = transform.Find ("/Dino_enemy(Clone)");
		Enemy_for_this = Dinos[Dinos.Length].transform;
		Bar = GameObject.Find ("HealthCoinGroup");
		//Bar.transform.parent = transform.Find("/Canvas/HealthCoinGroup");
		transform.SetParent(transform.Find("/Canvas"));
	}
	
	// Update is called once per frame
	void Update () {
		if (Hi < 1) {
			//Dinos = GameObject.FindGameObjectsWithTag("Enemy");
			Enemy_for_this = Dinos[Dinos.Length].transform;
			Hi = Hi + 1;
		}
		Hi = Hi + 1;
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;
		}

	}	

