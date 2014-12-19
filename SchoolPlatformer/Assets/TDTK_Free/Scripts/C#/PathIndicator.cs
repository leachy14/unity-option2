using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathIndicator : MonoBehaviour {

	private ParticleSystem indicator;
	public Transform indicatorT;
	
	public float stepDist=1;
	public float updateRate=0.25f;
	
	private int wpCounter=0;
	
	
	private PathTD path;
	
	// Use this for initialization
	void Start () {
		indicatorT=(Transform)Instantiate(indicatorT);
		indicator=indicatorT.gameObject.GetComponent<ParticleSystem>();
		indicator.emissionRate=0;
		
		path=gameObject.GetComponent<PathTD>();
		
		StartCoroutine(EmitRoutine());
	}
	
	IEnumerator EmitRoutine(){
		
		yield return null;
		
		Transform[] waypoints=path.GetPath();

		
		while(true){
			
			float dist=Vector3.Distance(waypoints[wpCounter].position, indicatorT.position);
			
			float thisStep=stepDist;
			if(dist<stepDist) {
				thisStep=stepDist-dist;
				indicatorT.position=waypoints[wpCounter].position;
				
				wpCounter+=1;
				if(wpCounter>=waypoints.Length){
					wpCounter=0;
					indicatorT.position=waypoints[wpCounter].position;
				}
			}
			
			if(thisStep>0){
				//rotate towards destination
				Vector3 pos=new Vector3(waypoints[wpCounter].position.x, waypoints[wpCounter].position.y, waypoints[wpCounter].position.z);
				Vector3 dir=pos-indicatorT.position;
				//~ Quaternion wantedRot;
				if(dir!=Vector3.zero){
					Quaternion wantedRot=Quaternion.LookRotation(dir);
				
					//set particlesystem to wantedRot
					indicator.startRotation=(wantedRot.eulerAngles.y-45)*Mathf.Deg2Rad;
					
					indicatorT.LookAt(waypoints[wpCounter]);
					
					//move, with speed take distance into accrount so the unit wont over shoot
					indicatorT.Translate(Vector3.forward*thisStep);
					
					indicator.Emit(1);
				}
			}
			
			yield return new WaitForSeconds(updateRate*Time.timeScale);
		}
	}
	
	
	
}



