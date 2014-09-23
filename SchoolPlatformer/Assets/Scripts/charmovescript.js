#pragma strict

var charSpeed: float;

var mainImage: Texture;
var aniImage: Texture;

//used for animation
var index = Mathf.FloorToInt(Time.time * 8.0) % 4;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);

function Start () {	
}

function Update () {

	if (Input.GetKey ("right"))
	{
		renderer.material.mainTexture = aniImage;
		transform.Translate(-charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 8.0) % 4;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

	} 
	
	else if (Input.GetKey ("left"))
	{
		renderer.material.mainTexture = aniImage;
		transform.Translate(charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 8.0) % 4;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

	}
	
	else 
	{
		
		renderer.material.mainTexture = mainImage;
		renderer.material.SetTextureOffset("_MainTex", Vector2(0,0));
		renderer.material.mainTextureScale = new Vector2(1,1);	
	}	
	
	 
	if (Input.GetKey("up"))
	{
	
		if (transform.position.y < 2.3)
		{
			transform.Translate(0,5 * charSpeed * Time.deltaTime,0);
		}
	} 
	
	
	
}