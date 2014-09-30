#pragma strict

var dino: GameObject;
var rngx: int;

function Start () {

	rngx = (Math.floor(Math.random() * 11);
	var di: GameObject;
	
	di = Instantiate(
		dino,
		Vector3(rngx, -2.5,1),
		

function Update () {

		var index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		var size = Vector2(0.15,1);
		var offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

}