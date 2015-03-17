#pragma strict


var Bomb : GameObject;
var turret : GameObject;
var gos : GameObject[];	
var fireRate : int;
private var nextFire = 0.0;

// Use this for initialization
function Start () {

GetComponent.<Animation>().Play("Countdown");
GetComponent.<Rigidbody2D>().velocity = transform.up * 0.1;
FindClosestEnemy();	
}

// Update is called once per frame
function Update () {

		FindClosestEnemy();	
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		GetComponent.<Rigidbody2D>().velocity = transform.up * 0;

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
<<<<<<< HEAD
			if (distance < 1 && closest.transform.position.x >= -2.388) {
			transform.LookAt(Bomb.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
			if (Time.time > nextFire) {
			nextFire = (Time.time + fireRate);
=======
			if (distance < 0.2 && closest.transform.position.x >= -2.388) {
			transform.LookAt(closest.transform.position, Vector3.forward);
>>>>>>> Perry
			Explode();
			
			}
		} 	
	}
}

function Explode () {

<<<<<<< HEAD
GetComponent.<Animation>().Play("Countdown", PlayMode.StopAll);
	
=======

		
		anim.SetBool ("explode", true);
		Physics2D.IgnoreLayerCollision(10,13, false);
		Physics2D.IgnoreLayerCollision(13,11, false);
		transform.localScale += Vector3(1,1,0);
		Destroy(gameObject, 0.17);
		
>>>>>>> Perry

}