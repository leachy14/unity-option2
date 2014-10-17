#pragma strict

var dinospeed: float;
var imageText: Texture;
//var dinoPrefab: Transform;
var aniImage: Texture;
var hello: int;

var index = Mathf.FloorToInt(Time.time * 8.0) % 4;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);


function Start () {
	hello = 1;
	//transform.Translate(dinospeed * Time.deltaTime,0,0); 
}

function Update () {

	if (hello == 1) {
	
	
		renderer.material.mainTexture = aniImage;
		transform.Translate(-dinospeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);
		
		}
		else 
	{
		
		renderer.material.mainTexture = imageText;
		renderer.material.SetTextureOffset("_MainTex", Vector2(0,0));
		renderer.material.mainTextureScale = new Vector2(1,1);	
	}	
	
}
