using UnityEngine;
using System.Collections;
using DG.Tweening;
public class dotween : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*transform.DOPath (Vector3[] waypoints, 5, PathType pathType = Linear,
		                  PathMode pathMode = 2D, 5, null);*/

		//transform.DOMoveX (-2, 1);
		//transform.DOMoveY (-2, -1).From(true);
		//transform.DOMoveX (2, 1).From(true);
	}
	
	// Update is called once per frame
	void Update () {
		transform.DOMoveX (-2, 1);
		transform.DOMoveY (-2, -1).From(true);
	}
}
