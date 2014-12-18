#pragma strict


function Start () {
	
	FindClosestEnemy();
}

function Update () {
	
	//if (enemy.gameObject 
}

function FindClosestEnemy (){
		// Find all game objects with tag Enemy
		var gos : GameObject[];
		gos = GameObject.FindGameObjectsWithTag("Enemy"); 
		var closest : GameObject; 
		var distance = Mathf.Infinity; 
		var position = transform.position; 
		// Iterate through them and find the closest one
		for (var go : GameObject in gos)  { 
			var diff = (go.transform.position - position);
			var curDistance = diff.sqrMagnitude; 
			if (curDistance < distance) { 
				closest = go; 
				distance = curDistance; 
			} 
		} 	