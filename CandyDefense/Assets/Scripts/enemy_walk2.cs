using UnityEngine;
using System.Collections;

public class enemy_walk2 : MonoBehaviour {

	public Transform[] waypoints;
	int cur = 0;
	
	public float speed = 0.3f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// Waypoint not reached yet? then move closer
		if (transform.position != waypoints[cur].position) {
			Vector2 p = Vector2.MoveTowards(transform.position,
			                                waypoints[cur].position,
			                                speed);
			rigidbody2D.MovePosition(p);
		}
		// Waypoint reached, select next one
		else cur = (cur + 1) % waypoints.Length;

		if (transform.position == waypoints [7].position) {
			Destroy(gameObject);
		}
	}
}
