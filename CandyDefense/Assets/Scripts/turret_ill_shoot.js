#pragma strict
import enemy_spawner;
import UnityEngine.GameObject;
import projectile;

var enemy : Transform;
var turret : GameObject;
var gos : GameObject[];	
var shot : GameObject;
var time : int;
var closest : GameObject; 
var distance : float;
var fireRate = 0.5;
private var nextFire = 0.0;


function Start () {
	time = 0;
	FindClosestEnemy();
	
}

function Update () {
	
	FindClosestEnemy();
	
	
	transform.rotation.x = 0;
	transform.rotation.y = 0;
	//if (enemy.gameObject 

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
			 //transform.LookAt(transform.position + new Vector3(0, 0, 1), go.position);
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