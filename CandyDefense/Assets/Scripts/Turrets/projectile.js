#pragma strict

var projectile_entity : GameObject;
var time : int;
var turret : GameObject;


function Start () {
		
		HandleShotMovement();
		projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
}


function FixedUpdate () {

	
		HandleShotMovement();
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		
	
}

function HandleShotMovement () {


	rigidbody2D.velocity = transform.up * -2;

if (transform.position.x < -3.5) {
	Destroy(gameObject);
	} else if (transform.position.x > 3.5) {
	Destroy(gameObject);
	} else if (transform.position.y > 2) {
	Destroy(gameObject);
	} else if (transform.position.y < -2) {
	Destroy(gameObject);
	}


}
