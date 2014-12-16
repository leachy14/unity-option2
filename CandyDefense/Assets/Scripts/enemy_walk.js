#pragma strict

private var speed: float;
private var direction: int;
private var move: int;
private var goingUp: boolean;

function Start () {
		
}

function Update () {
	move = 1;
	if (move == 1)
		Move1();
	
	move++;
	
	if (move == 2 && transform.position.x >= -1.739) 
		Move2();
		
	move++;
	
	if (move == 3 && transform.position.y <= -0.72)
		Move3();
	
	move++;
	
	if (move == 4 && transform.position.x >= -0.27 || goingUp == true) //looks like move4 is refusing to work
		Move4();
		
	move++;
	
	if (move == 5 && transform.position.y <= 0.7)	
		Move5();
		
	//move++;
	
	if (move == 6 && transform.position.x <= 1.17)
		Move6();
		
	//move++;
	
	if (move == 7 && transform.position.y <= -0.3)
		Move7();
		
}

function Move1() {
	
	if (transform.position.x <= -1.739){
		transform.position.x += .02;
		//move++;
	}
}

function Move2() {
	
	if (transform.position.y >= -0.72){
		transform.position.y += -.02;
		//move++;
	}
}

function Move3() {
	
	if (transform.position.x <= -0.27){
		transform.position.x += .02;
		//move=4;
		goingUp = true;
	}
}

function Move4() {
	
	if (transform.position.y <= 0.7){
		transform.position.y += .02;
		//move = 4;
	}
}

function Move5() {
	
	if (transform.position.x <= 1.17){
		transform.position.x += .02;
		//move++;
	}
}

function Move6() {
	
	if (transform.position.y >= -0.3){
		transform.position.y += -.02;
		//move++;
	}
}

function Move7() {
	
	if (transform.position.x <= 2.254){
		transform.position.x += .02;
		//move++;
	}
}