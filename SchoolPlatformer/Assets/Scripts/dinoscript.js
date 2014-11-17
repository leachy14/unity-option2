#pragma strict

var dino: GameObject;
var rngx: int;
var imageText: Texture;
var dinoPrefab: Transform;
var startTime: float;
var rndt: int;
var number = 0;

function Start () 
{
	
	
}
function Update () 
{
		rngx = Random.Range(-2,10);
		
		var index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		var size = Vector2(0.15,1);
		var offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);
		
		
		if (startTime == 0) 
    {
        startTime = Time.time; 
        rndt = Random.Range(5, 10);
    }
    else
    {
        if ((Time.time - startTime) >= rndt && number < 1) 
        {
        	number++;
            startTime = 0; 
            Instantiate (dinoPrefab, Vector3(rngx, -2.5, -4.3), Quaternion.identity);
		}
	}
	
}