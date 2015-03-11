using UnityEngine;
using System.Collections;
using Level;

namespace Walk {
public class Dino_walk : MonoBehaviour {
	
	private int _targetWaypoint = 0;
	private Transform _waypoints;
	
	public float movementSpeed = 3f;
	public int wave_number;
	public int Speed;

	public GameObject level_accessor;
	public LevelControl levelcontrol;

	// Use this for initialization
	void Start () 
	{
		_waypoints = GameObject.Find("Waypoints").transform;
		level_accessor = GameObject.Find("spawner");
		levelcontrol = level_accessor.GetComponent<LevelControl> ();
		
		//wave_number = levelcontrol.wave;
			if (this.gameObject.name == "Sanic_Raptor(Clone)") {
				Speed = 4;			
			} else if (this.gameObject.name == "Dino_enemy(Clone)") {
				Speed = 2;
			}

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
//		float targetAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;
		
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
				levelcontrol.lives--;
				return;
			}
		}
		else
		{
			// Walk towards waypoint
			GetComponent<Rigidbody2D>().AddForce(new Vector2(movementNormal.x, movementNormal.y) * movementSpeed * (Speed * .5f));
		}

	}


}
}