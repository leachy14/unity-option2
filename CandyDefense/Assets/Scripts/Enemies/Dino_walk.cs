using UnityEngine;
using System.Collections;

namespace Walk {
public class Dino_walk : MonoBehaviour {
	
	private int _targetWaypoint = 0;
	private Transform _waypoints;
	
	public float movementSpeed = 3f;
	public int wave_number;
	
	// Use this for initialization
	void Start () 
	{
		_waypoints = GameObject.Find("Waypoints").transform;
		
		GameObject level_accessor = GameObject.Find("spawner");
		LevelControl levelcontrol = level_accessor.GetComponent<LevelControl> ();
		wave_number = levelcontrol.wave;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	// Fixed update
	void FixedUpdate()
	{
		handleWalkWaypoints();

	}
	
	// Handle walking the waypoints
	private void handleWalkWaypoints()
	{
		Transform targetWaypoint = _waypoints.GetChild(_targetWaypoint);
		Vector3 relative = targetWaypoint.position - transform.position;
		Vector3 movementNormal = Vector3.Normalize(relative);
		float distanceToWaypoint = relative.magnitude;
		float targetAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;
		
		if (distanceToWaypoint < 0.1)
		{
			if (_targetWaypoint + 1 < _waypoints.childCount)
			{
				// Set new waypoint as target
				_targetWaypoint++;
			}
			else
			{
				// Inform level script that a unit has reached the last waypoint
				Destroy(gameObject);
				return;
			}
		}
		else
		{
			// Walk towards waypoint
			rigidbody2D.AddForce(new Vector2(movementNormal.x, movementNormal.y) * movementSpeed * (wave_number * .5f));
		}

	}


}
}