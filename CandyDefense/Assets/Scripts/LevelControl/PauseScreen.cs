using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.SetParent(GameObject.Find("Canvas").transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(1f,1f,1f);
	}
}
