#pragma strict


var Bomb : GameObject;
var turret : GameObject;
var gos : GameObject[];	
var fireRate : int;
private var nextFire = 0.0;

// Use this for initialization
function Start () {

animation.Play("Countdown");
rigidbody2D.velocity = transform.up * 0.1;
FindClosestEnemy();	
}

// Update is called once per frame
function Update () {

		FindClosestEnemy();	
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		rigidbody2D.velocity = transform.up * 0;

}
function FindClosestEnemy () {
		// Find all game objects with tag Enemy
		
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		Bomb = GameObject.FindGameObjectWithTag("Projectile");
		var closest : GameObject; 
		var distance = Mathf.Infinity; 
		var position = transform.position; 
		
		// Iterate through them and find the closest one
		for (var go : GameObject in gos)  { 
			var diff = (go.transform.position - position);
			var curDistance = diff.sqrMagnitude; 
			if (curDistance < distance) { 
				closest = go; 
				distance = curDistance; 
			}
			if (distance < 1 && closest.transform.position.x >= -2.388) {
			transform.LookAt(Bomb.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
			if (Time.time > nextFire) {
			nextFire = (Time.time + fireRate);
			Explode();
			
			}
		} 	
	}
}

function Explode () {

animation.Play("Countdown", PlayMode.StopAll);
	

}