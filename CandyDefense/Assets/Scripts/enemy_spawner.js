#pragma strict

var dino: GameObject;
var num = 5;
//var spawnTime: float = 5f;
var spawnDelay: float = 3f;


function Start () {
	
	//InvokeRepeating("Spawn", spawnDelay, spawnTime); 
	this.Spawn();
}



function Update () {
		
}

function Spawn() {
	
	
	for(var i = 1; i <= num; i++){//var enemyIndex = Random.Range(0, dino.length);
	Instantiate(dino, transform.position, transform.rotation);
	yield WaitForSeconds(spawnDelay);
	}
}