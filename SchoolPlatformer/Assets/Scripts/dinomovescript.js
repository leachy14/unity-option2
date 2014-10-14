#pragma strict

var dinospeed: float;
var imageText: Texture;
var dinoPrefab: Transform;
var aniImage: Texture;

var index = Mathf.FloorToInt(Time.time * 12.0) % 6;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);


function Start () {
	
	//transform.Translate(dinospeed * Time.deltaTime,0,0); 
}

function Update () {

	renderer.material.mainTexture = aniImage;
	transform.Translate(-dinospeed * Time.deltaTime,0,0);
	
		var index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		var size = Vector2(0.15,1);
		var offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);
		
}