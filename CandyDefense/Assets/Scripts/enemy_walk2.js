#pragma strict

public var waypoints : Transform[];
public var moveSpeed : float = 2.0;
public var rotSpeed : float = 5.0;
private var currentWaypoint : int = 0;

function Update(){
   
    if(currentWaypoint < waypoints.length){
       
        //RotateTowards(waypoints[currentWaypoint].position, rotSpeed);
        MoveForward(moveSpeed);
 
        var waypointDistance = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
       
        if(waypointDistance <= 0.5){
            currentWaypoint++;
        }
       }
    }
   
 
function MoveForward(moveSpeed : float){
    transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed);
}
 