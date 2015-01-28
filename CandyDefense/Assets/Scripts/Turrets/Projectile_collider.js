#pragma strict

function Start () {

}

function Update () {

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

function OnCollisionEnter2D(coll: Collision2D)		//dino destruction!
{
	if (coll.gameObject.tag == "Enemy")
	{


	}
	
} 

