#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter2D(coll: Collision2D)		//dino destruction!
{
	if (coll.gameObject.tag == "Enemy")
	{
	
	Destroy(coll.gameObject);
	Destroy(gameObject);
	}
	
} 

