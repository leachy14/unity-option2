using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Platform : MonoBehaviour {
	
	//buildable tower type on this platform
	public _TowerType[] buildableType=new _TowerType[1];
	
	public int[] specialBuildableID;
	
	//indicate if creep can walk pass this platform, true if this platform is part of a path
	private bool walkable;
	
	[HideInInspector] public GameObject thisObj;
	[HideInInspector] public Transform thisT;
	
	void Awake(){
		thisObj=gameObject;
		thisT=transform;
		
		thisObj.layer=LayerManager.LayerPlatform();
		
		if(specialBuildableID!=null && specialBuildableID.Length>0){
			for(int i=0; i<specialBuildableID.Length; i++){
				specialBuildableID[i]=Mathf.Max(0, specialBuildableID[i]);
			}
		}
	}
	
}
