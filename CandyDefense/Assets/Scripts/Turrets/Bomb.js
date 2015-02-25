#pragma strict


var Bomb : GameObject;
var turret : GameObject;
var gos : GameObject[];	
var fireRate : int;
var explodeRate : int;
private var nextFire = 0.0;
private var nextExplode = 0.0;
protected var anim : Animator;
var runStateHash : int = Animator.StringToHash("Base Layer.Run");
var iscounting : int;

// Use this for initialization
function Start () {

iscounting = 0;
anim = GetComponent("Animator");

rigidbody2D.velocity = transform.up * 0.1;
FindClosestEnemy();	
fireRate = 2;
explodeRate = 3;
}

// Update is called once per frame
function Update () {

		if(iscounting != 1){
		FindClosestEnemy();	
		}
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		rigidbody2D.velocity = transform.up * 0;
		 var stateInfo : AnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
		 if(iscounting == 1) {
		 
	 if (Time.time > nextExplode) {
		anim.SetBool ("explode", true);
		anim.SetBool ("countdown", false);
		transform.localScale += Vector3(0.5,0.5,0);
		Destroy(gameObject, 0.06);
		}
		}
		 

}
function FindClosestEnemy () {
		// Find all game objects with tag Enemy
		
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		Bomb = GameObject.FindGameObjectWithTag("Bomb");
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


nextExplode = (Time.time + explodeRate);
anim.SetBool ("countdown", true);
anim.SetBool ("explode", false);
iscounting = 1;

		if (Time.time > nextExplode) {
		anim.SetBool ("explode", true);
		anim.SetBool ("countdown", false);
		Destroy(gameObject, 0.06);
		}

}