#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter2D(coll: Collision2D)		//dino destruction!
{
	if (coll.gameObject.name == "Projectile_entity(Clone)")
	{
		//GameObject.FindGameObjectWithTag("Enemy");
		//coll.gameObject.SendMessage("ApplyDamage", 10);
	Destroy(coll.gameObject);
	Destroy(gameObject);
	}
	
} 
