#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter (hit: Collision)		//dino destruction!
{
	if (hit.gameObject.name == "dinoprefab(Clone)")
	{
		//Destroy(gameObject);
		Destroy(hit.gameObject);
	}
	
} 