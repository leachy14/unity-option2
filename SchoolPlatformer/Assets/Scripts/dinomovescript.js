#pragma strict

var dinospeed: float;
var imageText: Texture;
//var dinoPrefab: Transform;
var aniImageLeft: Texture;
var aniImageRight: Texture;
var hello: int; //what does this variable do? change variable name to something more significant
var dinoObject: GameObject; 

var index = Mathf.FloorToInt(Time.time * 8.0) % 4;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);

//Variable to access x-var of dino
var dinoX: int;
var direction: int;

function Start () {
	hello = 1;
	
	dinoX = transform.position.x;
	
	//transform.Translate(dinospeed * Time.deltaTime,0,0); 
}

function Update () {

	if (hello == 1) 
	{
		
		index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);
		
		if (transform.position.x >= (dinoX + 2) || transform.position.x == dinoX)
		{
			renderer.material.mainTexture = aniImageLeft;
			direction = -1;
		}
		
		else if (transform.position.x < (dinoX - 2))
		{
			renderer.material.mainTexture = aniImageRight;
			direction = 1;
		}
		
		transform.Translate(direction * dinospeed * Time.deltaTime, 0, 0);
	}
		
	else 
	{
		renderer.material.mainTexture = imageText;
		renderer.material.SetTextureOffset("_MainTex", Vector2(0,0));
		renderer.material.mainTextureScale = new Vector2(1,1);	
	}	
	
	transform.position.z = -4.3;
	
	if (transform.position.x > 60 || transform.position.x < -60)
	{
		Destroy(this);
		Destroy(dinoObject);
	}
}	
