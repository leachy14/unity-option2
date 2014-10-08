#pragma strict

var dinospeed: float;

function Start () {
	
	transform.Translate(dinospeed * Time.deltaTime,0,0); 
}

function Update () {

	if (transform.position.x)
	transform.Translate(dinospeed * Time.deltaTime,0,0);
}