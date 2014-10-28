#pragma strict

function Start () {

}

function Update () {

}

function OnControllerColliderHit (hit: ControllerColliderHit)		//dino destruction!
{
	if (hit.gameObject.CompareTag("stomp"));
	{
		//Destroy(gameObject);
		Destroy(hit.gameObject);
	}
	
} 