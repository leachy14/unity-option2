using UnityEngine;
using System.Collections;

public class OverlayManager : MonoBehaviour {

	static private GUITexture[] overlays=new GUITexture[3];
	static private GUITexture[] overlaysB=new GUITexture[3];
	static private bool[] inUseFlags=new bool[3];
	
	static public OverlayManager overlayManager;
	
	static private Transform camT;
	
	static public float widthModifier=1f;
	static public float heightModifier=1f;
	static public Vector3 offset=Vector3.zero;
	
	public static void SetModifier(float w, float h){
		widthModifier=w;
		heightModifier=h;
	}
	
	public static void SetOffset(Vector3 os){
		offset=os;
	}
	
	void Awake(){
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	static public void Init(){
		
		Camera cam=Camera.main;
		if(cam!=null)	camT=Camera.main.transform;
		
		GameObject obj=new GameObject();
		obj.name="OverlayManger";
		overlayManager=obj.AddComponent<OverlayManager>();
		
		int poolSize=3;
		overlays=new GUITexture[poolSize];
		overlaysB=new GUITexture[poolSize];
		inUseFlags=new bool[poolSize];
		
		if(overlayManager!=null){
			for(int i=0; i<poolSize; i++){
				obj=new GameObject();
				obj.name="overlay";
				obj.transform.parent=overlayManager.transform;
				obj.transform.localScale=new Vector3(0, 0, 1);
				
				overlays[i]=obj.AddComponent<GUITexture>();
				overlays[i].texture=(Texture)Resources.Load("GreenBar");
				overlays[i].pixelInset=new Rect(0, 0, 0, 0);
				
				obj=new GameObject();
				obj.name="overlayB";
				obj.transform.parent=overlayManager.transform;
				obj.transform.localScale=new Vector3(0, 0, 1);
				
				overlaysB[i]=obj.AddComponent<GUITexture>();
				overlaysB[i].texture=(Texture)Resources.Load("GreyBar");
				overlaysB[i].pixelInset=new Rect(0, 0, 0, 0);
				
				inUseFlags[i]=false;
			}
		}
	}
	
	static public void Building(UnitTower unit){
		if(overlayManager==null) Init();
		
		int num=GetUnuseFlag();
		inUseFlags[num]=true;
		
		overlayManager.StartCoroutine(overlayManager.Building(num, unit));
	}
	
	static public void Unbuilding(UnitTower unit){
		if(overlayManager==null) Init();
		
		int num=GetUnuseFlag();
		inUseFlags[num]=true;
		
		overlayManager.StartCoroutine(overlayManager.Unbuilding(num, unit));
	}
	
	static private int GetUnuseFlag(){
		for(int i=0; i<inUseFlags.Length; i++){
			if(!inUseFlags[i])	return i;
		}
		
		return 0;
	}
	
	IEnumerator Building(int i, UnitTower unit){
		
		float gridSize=BuildManager.GetGridSize();
		float totalDuration=unit.GetCurrentBuildDuration();
		
		while(true){
			if(unit==null) break;
			
			Vector3 screenPos=Camera.main.WorldToScreenPoint(unit.thisT.position+offset);
			screenPos=new Vector3((screenPos.x), screenPos.y, 0); 
			
			float remainDuration=unit.GetRemainingBuildDuration();
			if(remainDuration<=0) break;
			
			float dist=Vector3.Distance(camT.position, unit.thisT.position);
			
			float ratio=remainDuration/totalDuration;
			float width=widthModifier*20*gridSize*20/dist;
			float height=heightModifier*30/dist;
			
			overlaysB[i].pixelInset=new Rect(screenPos.x-width/2, screenPos.y, width, height);
			overlays[i].pixelInset=new Rect(screenPos.x-width/2, screenPos.y, width*(1-ratio), height);
			
			yield return null;
		}
		
		overlaysB[i].pixelInset=new Rect(0, 0, 0, 0);
		overlays[i].pixelInset=new Rect(0, 0, 0, 0);
		inUseFlags[i]=false;
	}
	
	IEnumerator Unbuilding(int i, UnitTower unit){
		
		float gridSize=BuildManager.GetGridSize();
		float totalDuration=unit.GetCurrentBuildDuration();
		
		while(true){
			if(unit==null) break;
			
			Vector3 screenPos=Camera.main.WorldToScreenPoint(unit.thisT.position+offset);
			screenPos=new Vector3((screenPos.x), screenPos.y, 0); 
			
			float remainDuration=unit.GetRemainingBuildDuration();
			if(remainDuration>=totalDuration) break;
			
			float dist=Vector3.Distance(camT.position, unit.thisT.position);
			
			float ratio=remainDuration/totalDuration;
			float width=widthModifier*20*gridSize*20/dist;
			float height=heightModifier*30/dist;
			
			overlaysB[i].pixelInset=new Rect(screenPos.x-width/2, screenPos.y, width, height);
			overlays[i].pixelInset=new Rect(screenPos.x-width/2, screenPos.y, width*(1-ratio), height);
			
			yield return null;
		}
		
		overlaysB[i].pixelInset=new Rect(0, 0, 0, 0);
		overlays[i].pixelInset=new Rect(0, 0, 0, 0);
		inUseFlags[i]=false;
	}
	
	
}
