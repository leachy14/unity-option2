using UnityEngine;
using System.Collections;

public class DebugShowSelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
