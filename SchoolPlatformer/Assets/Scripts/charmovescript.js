#pragma strict

var charSpeed: float;

//used for animation
var index = Mathf.FloorToInt(Time.time * 8.0) % 4;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);

function Start () {
//	renderer.material.SetTexture("idle_placeholder",offset);
	//SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
}

function Update () {

	

	if (Input.GetKey ("right"))
	{
		transform.Translate(-charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 8.0) % 4;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

	}
	
	if (Input.GetKey ("left"))
	{
		transform.Translate(charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 8.0) % 4;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

	}
	
	//else {
		//renderer.material.SetTexture(idle_placeholder);
	//}
	
	/*var index = Mathf.FloorToInt(Time.time * 8.0) % 4;
	var size = Vector2(0.15,1);
	var offset = Vector2(index/6.0,0);
	renderer.material.SetTextureScale("_MainTex", size);
	renderer.material.SetTextureOffset("_MainTex", offset);*/

}