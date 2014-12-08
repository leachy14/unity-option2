#pragma strict

var waypoint : Transform[];            // The amount of Waypoint you want
 var patrolSpeed : float = 3;        // The walking speed between Waypoints
 var loop : boolean = true;            // Do you want to keep repeating the Waypoints
 var dampingLook = 6.0;                // How slowly to turn
 var pauseDuration : float = 0;        // How long to pause at a Waypoint
 
 private var curTime : float;
 private var currentWaypoint : int = 0;
 private var character : CharacterController;
 
 function Start(){
 
     character = GetComponent(CharacterController);
 }
 
 function Update(){
 
     if(currentWaypoint < waypoint.length){
         patrol();
         }else{    
     if(loop){
         currentWaypoint=0;
         } 
     }
 }
 
 function patrol(){
 
         var target : Vector3 = waypoint[currentWaypoint].position;
         target.y = transform.position.y; // Keep waypoint at character's height
         var moveDirection : Vector3 = target - transform.position;
 
     if(moveDirection.magnitude < 0.5){
         if (curTime == 0)
             curTime = Time.time; // Pause over the Waypoint
         if ((Time.time - curTime) >= pauseDuration){
             currentWaypoint++;
             curTime = 0;
         }
     }else{        
         var rotation = Quaternion.LookRotation(target - transform.position);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
         character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
     }    
 }
 