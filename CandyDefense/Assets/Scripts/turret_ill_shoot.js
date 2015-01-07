#pragma strict
import enemy_spawner;
import UnityEngine.GameObject;
import projectile;

var enemy : Transform;
var turret : GameObject;
var gos : GameObject[];	
var shot : GameObject;
var time : int;

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
			transform.LookAt(turret.gameObject.FindGameObjectWithTag("Enemy").transform.position);
			yield WaitForSeconds (3);
			time++;
			if (time == 6) {
			time = 0;
			Shoot();
			yield WaitForSeconds (1);
			}
		} 	
	}
}

function Shoot () {



Instantiate(shot, transform.position, transform.rotation);






}