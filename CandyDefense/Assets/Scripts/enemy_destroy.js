#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter2D(coll: Collision2D)		//dino destruction!
{
	if (coll.gameObject.name == "enemy(Clone)")
	{
	
	Destroy(coll.gameObject);
	}
	
} 
function OnCollisionExit2D(coll: Collision2D) {

	if (coll.gameObject.tag == "Enemy")
	{
	Destroy(gameObject);

	}

}