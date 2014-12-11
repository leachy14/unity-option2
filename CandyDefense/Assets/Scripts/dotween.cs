using UnityEngine;
using System.Collections;
using DG.Tweening;

public class dotween : MonoBehaviour {

	//public GameObject enemy_ph;
	// Use this for initialization
	void Start () {


		transform.DOMoveX (-2, 2, false);



		//transform.DOMove (new Vector2 (2, 2), 3, false);
	}
	
	// Update is called once per frame
	void Update () {

		//for(int i = 1; i == 7; i++)


		if (transform.position.x == -2 && transform.position.y == 0.75) {
					transform.DOMoveY (-1.04f, 3, false);

		}

		if (transform.position.y == -1.04 && transform.position.x == -2) {
						transform.DOMoveX (-1.069f, 2, false);
					
			}
			
	}
}
