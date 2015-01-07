#pragma strict
//import turret_ill_shoot;


var projectile_entity : GameObject;
var time : int;
var turret : GameObject;
var gos : GameObject[];	

function Start () {

FindClosestEnemy();
}


function FixedUpdate () {

		FindClosestEnemy();
		
		projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		
		//transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position);
		
		//projectile_entity.transform.forward;

		//constantForce.relativeForce = Vector2(0,0);
}
function Shoot () {

  //  Instantiate(projectile_entity, transform.position, transform.rotation);
		




}
function HandleShotMovement () {

if (time == 6)  {

	constantForce.relativeForce = Vector2(1,1);

/*
		Vector3 relative = targetWaypoint.position - transform.position;;
		Vector3 movementNormal = Vector3.Normalize(relative);;
		float distanceToWaypoint = relative.magnitude;;
		float targetAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;
*/




	}
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
			transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position);
			yield WaitForSeconds (5);
			time++;
		}
	
	}

}