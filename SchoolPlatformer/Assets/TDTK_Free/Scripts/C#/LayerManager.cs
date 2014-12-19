using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LayerManager : MonoBehaviour {
	
	public static bool initiated=false;
	
	public int layerCreep=31;
	public int layerCreepF=30;
	public int layerTower=29;
	public int layerPlatform=28;
	public int layerOverlay=25;
	public int layerMiscUI=27;
	
	public static LayerManager layerManager;

	void Awake(){
		layerManager=this;
		
		#if UNITY_EDITOR
			GameControl gameControl=gameObject.GetComponent<GameControl>();
			if(gameControl!=null){
				gameControl.layerManager=this;
			}
		#endif
	}

	
	
	public static void Init(){
		if(layerManager==null){
			GameObject obj=new GameObject();
			obj.name="LayerManager";
			
			layerManager=obj.AddComponent<LayerManager>();
			
			//Debug.Log("init   "+layerManager);
		}
	}
	
	public static int LayerCreep(){
		return layerManager.layerCreep;
		//return layerCreep;
	}
	
	public static int LayerCreepF(){
		return layerManager.layerCreepF;
		//return layerCreepF;
	}
	
	public static int LayerTower(){
		return layerManager.layerTower;
		//return layerTower;
	}
	
	public static int LayerPlatform(){
		return layerManager.layerPlatform;
		//return layerPlatform;
	}
	
	public static int LayerOverlay(){
		return layerManager.layerOverlay;
		//return layerPlatform;
	}
	
	public static int LayerMiscUIOverlay(){
		return layerManager.layerMiscUI;
		//return layerPlatform;
	}
	
}
