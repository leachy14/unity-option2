﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Enemy;
public class Enemy_health_slider : MonoBehaviour {

	public Slider EnemyBar;

	public Camera main;
	public Transform Enemy_for_this;
	public Vector3 vec;	
	public GameObject Bar;
	public GameObject[] Dinos;
	public Object[] Bars;
	public int Hi = 0;
	public Enemy_Coll round_accessor;

	// Use this for initialization
	void Start () {

		Dinos = GameObject.FindGameObjectsWithTag("Enemy");
		transform.SetParent(GameObject.Find("Canvas").transform, false);
		Enemy_for_this = Dinos[Dinos.Length - 1].transform;
		round_accessor = Enemy_for_this.GetComponent<Enemy_Coll> ();
		//Bars = FindObjectsOfType(typeof (Slider));
		EnemyBar = FindObjectOfType(typeof (Slider)) as Slider;
		EnemyBar.maxValue = round_accessor.hlth;
	}
	
	// Update is called once per frame
	void Update () {
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;
		transform.localScale = new Vector3(1f,1f,1f);

		EnemyBar.value = Mathf.MoveTowards (EnemyBar.value, round_accessor.hlth, 1f);
		if (round_accessor.hlth < 1) 
		{
			Destroy(gameObject);
		}
		}

	}	
