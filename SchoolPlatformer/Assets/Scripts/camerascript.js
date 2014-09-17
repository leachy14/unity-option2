#pragma strict

var scrollSpeed: float;

function Start () {

}

function Update () {
	renderer.material.SetTextureOffset(
	 "_MainTex",
	 Vector2(scrollSpeed * Time.time,0)
	 );

}