#pragma strict
import UnityEngine.GameObject;
import projectile;

var enemy : Transform;
var turret : GameObject;
var gos : GameObject[];	
var shotnormal : GameObject;
var closest : GameObject; 
var shotFlame : GameObject;

var distance : float;
var fireRate : int;
var iceRate : int;
private var nextIce = 0.0;
private var nextFire = 0.0;


function Start () {

	FindClosestEnemy();
	transform.position.z = 0;
	if(this.gameObject.name == "illuminaty_turret(Clone)") {
	fireRate = 2;
	}
	if (this.gameObject.name == "green_turret(Clone)") {
	fireRate = 1;
	}
	if (this.gameObject.name == "FlameThrower(Clone)") {
	fireRate = 0.1;
	}
	if (this.gameObject.name == "Ice_turret(Clone)") {
	fireRate = 4;
	iceRate = 0.2;
	}
}

function Update () {
	
	FindClosestEnemy();
	transform.rotation.x = 0;
	transform.rotation.y = 0;
Physics2D.IgnoreLayerCollision(10,13, true);
Physics2D.IgnoreLayerCollision(13,11, true);
Physics2D.IgnoreLayerCollision(13,13, true);
Physics2D.IgnoreLayerCollision(13,14, true);
Physics2D.IgnoreLayerCollision(11,14, true);
Physics2D.IgnoreLayerCollision(10,14, true);
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
			if (distance < 1.5 && closest.transform.position.x >= -2.388) {
			transform.LookAt(turret.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
			if (Time.time > nextFire) {
			nextFire = (Time.time + fireRate);
			Shoot();
			
			}
		} 	
	}// && this.gameObject.name != "Flame thrower(Clone)"
}

function Shoot () {

	//if(this.gameObject.name != "FlameThrower(Clone)"){
	//	Instantiate(shotnormal, transform.position, transform.rotation);
	//}
	if(this.gameObject.name == "FlameThrower(Clone)"){
		Instantiate(shotFlame, transform.position, transform.rotation);
	}else if(this.gameObject.name == "Ice_turret(Clone)"){
	
			for (var i = 0; i < 3; i++) {
			//if (Time.time > nextIce) {
			//nextIce = (Time.time + iceRate);
			Instantiate(shotFlame, transform.position, transform.rotation);
			//}
			}
			
	} else {
	Instantiate(shotnormal, transform.position, transform.rotation);
	}
}
