#pragma strict
//import turret_ill_shoot;


var projectile_entity : GameObject;
var time : int;
var turret : GameObject;
var gos : GameObject[];	

function Start () {
FindClosestEnemy();
HandleShotMovement();
		projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		
		transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
}


function FixedUpdate () {

		FindClosestEnemy();
		
		HandleShotMovement();
		
		//projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		
		//transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position);
		transform.position.z = 0;
		
	transform.rotation.x = 0;
	transform.rotation.y = 0;
		//projectile_entity.transform.forward;
;
}
function Shoot () {

  //  Instantiate(projectile_entity, transform.position, transform.rotation);
		




}
function HandleShotMovement () {


rigidbody2D.velocity = transform.up * -2;
 /*if (transform.rotation.z >= 270 && transform.rotation.z <= 90) {
 
	rigidbody2D.velocity = transform.right * 2;
}
 else {
 rigidbody2D.velocity = transform.right * -2;
 }
*/


}
function FindClosestEnemy (){
		// Find all game objects with tag Enemy
		
		gos = GameObject.FindGameObjectsWithTag("Enemy"); 
		turret = GameObject.FindGameObjectWithTag("Turret");
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
 			if (distance < 2) {
			//transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position);
			yield WaitForSeconds (5);
			time++;
		}
	
	}

}
/*function OnCollisionEnter(collision : Collision) {
	var body : Rigidbody = collision.collider.attachedRigidbody;

	if (collision.rigidbody == null) {

	}

	
	else if (collision.rigidbody == {
	canJump = true;
	}

}
*/
/*function OnCollisionEnter (hit: Collision)		//dino destruction!
{
	if (hit.gameObject.name == "enemy(Clone)")
	{
		//Destroy(gameObject);
		Destroy(hit.gameObject);
	}
	
} */