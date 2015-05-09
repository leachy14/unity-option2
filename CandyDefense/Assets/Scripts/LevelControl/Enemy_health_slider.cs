using UnityEngine;
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
		Enemy_for_this = Dinos[Dinos.Length - 1].transform;
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;

		round_accessor = Enemy_for_this.GetComponent<Enemy_Coll> ();
		//Bars = FindObjectsOfType(typeof (Slider));
		EnemyBar = FindObjectOfType(typeof (Slider)) as Slider;
		EnemyBar.maxValue = round_accessor.hlth;
	

	}
	
	// Update is called once per frame
	void Update () {

		transform.localScale = new Vector3(1f,1f,1f);
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;

		EnemyBar.value = Mathf.MoveTowards (EnemyBar.value, round_accessor.hlth, 1f);
		if (round_accessor.hlth <= 0) 
		{
			Destroy(gameObject);
		}
		}
	void LateUpdate () {
		vec = Enemy_for_this.InverseTransformDirection(Enemy_for_this.position.x,Enemy_for_this.position.y + -.25f, -6 );
		transform.position = vec;
		transform.SetParent(GameObject.Find("Canvas").transform, false);
		transform.localScale = new Vector3(1f,1f,1f);
	}

	}	

