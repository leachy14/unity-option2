#pragma strict
import UnityEngine.GameObject;
import projectile;

var enemy : Transform;
var turret : GameObject;
var gos : GameObject[];	
var shot : GameObject;
var closest : GameObject; 
var distance : float;
var fireRate = 2;
private var nextFire = 0.0;


function Start () {

	FindClosestEnemy();
	transform.position.z = 0;
}

function Update () {
	
	FindClosestEnemy();
	transform.rotation.x = 0;
	transform.rotation.y = 0;


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
			if (distance < 1.5) {
			transform.LookAt(turret.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
			if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Shoot();
			
			}
		} 	
	}
}

function Shoot () {

	Instantiate(shot, transform.position, transform.rotation);

}