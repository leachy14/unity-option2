using UnityEngine;
using System.Collections;

public class enemy_path : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("enemy_path"), "time", 8));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
