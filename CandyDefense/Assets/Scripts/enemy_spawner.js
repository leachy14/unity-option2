#pragma strict

var dino: GameObject;
var num = 5;
var spawnTime: float = 5f;
var spawnDelay: float = 3f;
var enemies : GameObject[];

function Start () {
	
	InvokeRepeating("Spawn", spawnDelay, spawnTime); 
	
}



function Update () {
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
}

function Spawn() {
	
	//var enemyIndex = Random.Range(0, dino.length);
	Instantiate(dino, transform.position, transform.rotation);
	
}