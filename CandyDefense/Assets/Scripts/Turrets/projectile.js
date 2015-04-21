#pragma strict

var projectile_entity : GameObject;
var time : int;
var turret : GameObject;


function Start () {
		
		transform.Translate(0, 0, 0.2);
		HandleShotMovement();
		projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		//transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position, Vector3.forward);
		if(this.gameObject.name == "shotFlame(Clone)") {
			Destroy(gameObject, 0.3);
		}

}


function FixedUpdate () {

	
		HandleShotMovement();
		transform.position.z = 0;
		transform.rotation.x = 0;
		transform.rotation.y = 0;
		Physics2D.IgnoreLayerCollision(12,12, true);
		if(this.gameObject.name == "shotFlame(Clone)") {
		transform.localScale += Vector3(0.1,0.1,0);
		
	}
}

function HandleShotMovement () {

if(this.gameObject.name == "Sniper_Shot_entity(Clone)"){
	GetComponent.<Rigidbody2D>().velocity = transform.up * -6;
} else {
	GetComponent.<Rigidbody2D>().velocity = transform.up * -6;
}

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
