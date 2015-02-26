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
var path_collide : GameObject;


// Use this for initialization
function Start () {
transform.Translate(0, 0, 0.2);
iscounting = 0;
anim = GetComponent("Animator");
rigidbody2D.velocity = transform.up * 0.1;
FindClosestEnemy();	
fireRate = 2;
explodeRate = 3;

path_collide = gameObject.Find("Path Spawn Blocker");
Physics2D.IgnoreLayerCollision(10,13, true);
Physics2D.IgnoreLayerCollision(13,11, true);
Physics2D.IgnoreLayerCollision(13,13, true);
}

// Update is called once per frame
function Update () {

		
		FindClosestEnemy();	
		
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		
		
		if (path_collide.collider2D.OverlapPoint (transform.position) == false) {
		rigidbody2D.velocity = transform.up * -0.5;
		}
		if (path_collide.collider2D.OverlapPoint (transform.position) == true) {
		rigidbody2D.velocity = transform.up * 0;
		}
		 
		  
		 var stateInfo : AnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
		
		 
	 

		


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
			if (distance < 0.2 && closest.transform.position.x >= -2.388) {
			transform.LookAt(Bomb.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
			Explode();
			
			}
		 	
	}
}

function Explode () {


		
		anim.SetBool ("explode", true);
Physics2D.IgnoreLayerCollision(10,13, false);
Physics2D.IgnoreLayerCollision(13,11, false);
		transform.localScale += Vector3(1,1,0);
		Destroy(gameObject, 0.17);
		

}