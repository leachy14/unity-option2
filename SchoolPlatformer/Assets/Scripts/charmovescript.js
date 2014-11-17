#pragma strict

var charSpeed: float;

var mainImage: Texture;
var aniImage: Texture;
var aniImageLeft: Texture;
var canJump: boolean;
var collider: Physics;

//used for animation
private var index: float;// = Mathf.FloorToInt(Time.time * 8.0) % 4;
var size = Vector2(0.15,1);
var offset = Vector2(index/6.0,0);

function Start () {


	canJump = true;
}

function Update () {



	if (Input.GetKey ("escape"))
		Application.Quit();
	
	
		

	if (Input.GetKey ("right"))
	{
		renderer.material.mainTexture = aniImage;
		transform.Translate(-charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 12.0) % 6;
		size = Vector2(0.15,1);
		offset = Vector2(index/6.0,0);
		renderer.material.SetTextureScale("_MainTex", size);
		renderer.material.SetTextureOffset("_MainTex", offset);

	} 
	
	else if (Input.GetKey ("left"))
	{
		renderer.material.mainTexture = aniImageLeft;
		transform.Translate(charSpeed * Time.deltaTime,0,0);
		index = Mathf.FloorToInt(Time.time * 12.0) % 6;
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
	
	 
	if (Input.GetKey("up") && canJump == true)
	{
		canJump = false;
		transform.rigidbody.velocity.y = 7.7;
			//transform.Translate(0,3.5 * charSpeed * Time.deltaTime,0);
		
	} 
	

	/*else if (Input.GetKey("up") && JumpNum == 1)
	{
		canJump = false;
	}
	*/
	
	
/*function OnControllerColliderHit (hit : ControllerColliderHit) {
		var body : Rigidbody = hit.collider.attachedRigidbody;
		// no rigidbody
	
	
	if (body == null) 
	{ 
		canJump = false;
	}
	else 
	{
	canJump = true;
	}
}
*/
	
	if (transform.position.x < -49.03)
		transform.position.x = -49.03;
		
	if (transform.position.x > 48.96)
		transform.position.x = 48.96;
}

function OnCollisionEnter(collision : Collision) {
	var body : Rigidbody = collision.collider.attachedRigidbody;

	if (collision.rigidbody == null) {
	canJump = true;
	}

	
	else {
	canJump = true;
	}

}

