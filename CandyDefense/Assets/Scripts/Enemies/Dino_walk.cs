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

	public GameObject other_dino;
	public GameObject HealthBar;
	
		public int healthdo;
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
			} else if (this.gameObject.name == "Salid_snake(Clone)") {
				Speed = 1;
			}
			healthdo = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
			/*if (this.gameObject.name == "Sanic_Raptor(Clone)") {
				Speed = 4;	
				Physics2D.IgnoreCollision(other_dino.collider2D, collider2D);
			}*/
			Physics2D.IgnoreLayerCollision(10,11, true);
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
					//InvokeRepeating ("TakeHealth", 1, 1);
					StartCoroutine (TakeHealth(healthdo));
				
				return;
			}
		}
		else
		{
			// Walk towards waypoint
			GetComponent<Rigidbody2D>().AddForce(new Vector2(movementNormal.x, movementNormal.y) * movementSpeed * (Speed * .5f));
		}

	}
	IEnumerator TakeHealth (int Hi) {
			for (int i = 0; i < 50; i++) {
				levelcontrol.lives = (levelcontrol.lives + -0.002f);
				yield return new WaitForSeconds(0.0001f);
			}	
			Destroy(GameObject.Find ("Enemy_slider"));
			Destroy(GameObject.Find ("Enemy_health_slider(Clone)"));
			Destroy(gameObject);

		}

}	
}