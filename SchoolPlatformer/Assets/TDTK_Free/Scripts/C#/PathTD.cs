using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathTD : MonoBehaviour {

	
	public Transform[] waypoints;
	
	public bool generatePathObject=true;
	public bool showGizmo=true;
	
	private Transform thisT;
	
	
	void Awake(){
		thisT=transform;
		//thisObj=gameObject;
	}
	
	void Start(){
		if(generatePathObject){
			CreateLinePath();
		}
	}

	//create line-renderer along the path as indicator
	void CreateLinePath(){
		
		Vector3 offsetPos=new Vector3(0, 0, 0);
		
		for(int i=1; i<waypoints.Length; i++){
				GameObject obj=new GameObject();
				obj.name="path"+i.ToString();
				
				Transform objT=obj.transform;
				objT.parent=thisT;
				
				LineRenderer line=obj.AddComponent<LineRenderer>();
				line.material=(Material)Resources.Load("PathMaterial");
				line.SetWidth(0.3f, 0.3f);
				
				line.SetPosition(0, waypoints[i-1].position+offsetPos);
				line.SetPosition(1, waypoints[i].position+offsetPos);
		}
		
		for(int i=1; i<waypoints.Length-1; i++){
			GameObject obj=(GameObject)Instantiate((GameObject)Resources.Load("wpNode"), waypoints[i].position+offsetPos, Quaternion.identity);
			obj.transform.parent=transform;
		}
		
	}
	
	
	public Transform[] GetPath(){
		return waypoints;
	}
	
	void OnDrawGizmos(){
		if(showGizmo){
			Gizmos.color = Color.blue;
			if(waypoints!=null && waypoints.Length>0){
				
				for(int i=1; i<waypoints.Length; i++){
					if(waypoints[i-1]!=null && waypoints[i]!=null)
						Gizmos.DrawLine(waypoints[i-1].position, waypoints[i].position);
				}
			}
		}
	}
	
}

