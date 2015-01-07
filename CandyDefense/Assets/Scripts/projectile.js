#pragma strict
import turret_ill_shoot;


var projectile_entity : GameObject;

function Start () {

//turret_ill_shoot.FindClosestEnemy();

}


function Update () {



		projectile_entity = GameObject.FindGameObjectWithTag("Projectile");
		
		transform.LookAt(projectile_entity.gameObject.FindGameObjectWithTag("Enemy").transform.position);
		
		//projectile_entity.transform.forward;

		constantForce.relativeForce = Vector2(0,0);
}
function Shoot () {

  //  Instantiate(projectile_entity, transform.position, transform.rotation);
		




}
function HandleShotMovement () {




		float distanceToWaypoint = relative.magnitude;
		float targetAngle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 90;




}