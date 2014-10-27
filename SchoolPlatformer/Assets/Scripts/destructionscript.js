#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter (collision: Collision)		//dino destruction!
{
	if (other.tag == "stomp")
	{
		Destroy(gameObject);
		Destroy(collision.gameObject);
	}
	
} 