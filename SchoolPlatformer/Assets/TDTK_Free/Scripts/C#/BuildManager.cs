using UnityEngine;
using System.Collections;

public class BuildManager : MonoBehaviour {

	public UnitTower[] towers;
	
	static private float _gridSize=0;
	public float gridSize=2;
	public Transform[] platforms;
	private Platform[] buildPlatforms;
	
	public bool autoSearchForPlatform=false;
	
	public bool autoAdjstTextureToGrid=true;
	
	public bool enableTileIndicator=true;
	
	public bool retainPrefabShaderForSamples=false;
	
	public int terrainColliderLayer=-1;
	
	static public BuildManager buildManager;
	
	static private BuildableInfo currentBuildInfo;
	
	static private int towerCount=0;
	
	public static int PrePlaceTower(){
		return towerCount+=1;
	}
	public static int GetTowerCount(){
		return towerCount;
	}
	
	void Awake(){
		buildManager=this;
		
		foreach(UnitTower tower in towers){
			tower.thisObj=tower.gameObject;
		}
		
		towerCount=0;
		
		gridSize=Mathf.Clamp(gridSize, 0.5f, 3.0f);
		_gridSize=gridSize;
		
		InitPlatform();
	}
	

	// Use this for initialization
	void InitPlatform() {
		
		if(autoSearchForPlatform){
			LayerMask mask=1<<LayerManager.LayerPlatform();
			Collider[] platformCols=Physics.OverlapSphere(Vector3.zero, Mathf.Infinity, mask);
			platforms=new Transform[platformCols.Length];
			for(int j=0; j<platformCols.Length; j++){
				platforms[j]=platformCols[j].transform;
			}
		}

		buildPlatforms=new Platform[platforms.Length];
		
		int i=0;
		foreach(Transform basePlane in platforms){
			//if the platform object havent got a platform componet on it, assign it
			Platform platform=basePlane.gameObject.GetComponent<Platform>();
			
			if(platform==null){
				platform=basePlane.gameObject.AddComponent<Platform>();
				platform.buildableType=new _TowerType[7];
				
				//by default, all tower type is builidable
				platform.buildableType[0]=_TowerType.TurretTower;
				platform.buildableType[1]=_TowerType.AOETower;
				//~ platform.buildableType[2]=_TowerType.DirectionalAOETower;
				platform.buildableType[3]=_TowerType.SupportTower;
				//~ platform.buildableType[4]=_TowerType.ResourceTower;
				//~ platform.buildableType[5]=_TowerType.Mine;
				//~ platform.buildableType[6]=_TowerType.Block;
			}
			
			buildPlatforms[i]=platform;
			
			//make sure the plane is perfectly horizontal, rotation around the y-axis is presreved
			basePlane.eulerAngles=new Vector3(0, basePlane.rotation.eulerAngles.y, 0);
			
			//adjusting the scale
			float scaleX=Mathf.Floor(UnitUtility.GetWorldScale(basePlane).x*10/gridSize)*gridSize*0.1f;
			float scaleZ=Mathf.Floor(UnitUtility.GetWorldScale(basePlane).z*10/gridSize)*gridSize*0.1f;
			
			if(scaleX==0) scaleX=gridSize*0.1f;
			if(scaleZ==0) scaleZ=gridSize*0.1f;
			
			basePlane.localScale=new Vector3(scaleX, 1, scaleZ);
			
			//adjusting the texture
			if(autoAdjstTextureToGrid){
				Material mat=basePlane.renderer.material;
				
				float x=(UnitUtility.GetWorldScale(basePlane).x*10f)/gridSize;
				float z=(UnitUtility.GetWorldScale(basePlane).z*10f)/gridSize;
				
				mat.mainTextureOffset=new Vector2(0.5f, 0.5f);
				mat.mainTextureScale=new Vector2(x, z);
			}
			
			//get the platform component, if any
			//Platform p=basePlane.gameObject.GetComponent<Platform>();
			//buildPlatforms[i]=new BuildPlatform(basePlane, p);
			i++;
		}

	}
	
	private static GameObject indicator;
	private static GameObject indicator2;
	
	void Start(){
		if(buildManager.enableTileIndicator){
			indicator=GameObject.CreatePrimitive(PrimitiveType.Cube);
			indicator.name="indicator";
			indicator.active=false;
			indicator.transform.localScale=new Vector3(gridSize, 0.025f, gridSize);
			indicator.transform.renderer.material=(Material)Resources.Load("IndicatorSquare");
			
			indicator2=GameObject.CreatePrimitive(PrimitiveType.Cube);
			indicator2.name="indicator2";
			indicator2.active=false;
			indicator2.transform.localScale=new Vector3(gridSize, 0.025f, gridSize);
			indicator2.transform.renderer.material=(Material)Resources.Load("IndicatorSquare");
			
			Destroy(indicator.collider);
			Destroy(indicator2.collider);
		}
		
		BuildManager.InitiateSampleTower();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	static public void ClearBuildPoint(){
		currentBuildInfo=null;
		ClearIndicator();
	}
	
	static public void ClearIndicator(){
		if(indicator!=null) indicator.active=false;
	}
	
	//called to set indicator to a particular node, set the color as well
	//not iOS performance friendly
	static public void SetIndicator(Vector3 pointer){
		
		if(!buildManager.enableTileIndicator) return;
		
		//layerMask for platform only
		LayerMask maskPlatform=1<<LayerManager.LayerPlatform();
		//layerMask for detect all collider within buildPoint
		LayerMask maskAll=1<<LayerManager.LayerPlatform();
		if(buildManager.terrainColliderLayer>=0) maskAll|=1<<buildManager.terrainColliderLayer;
		
		Ray ray = Camera.main.ScreenPointToRay(pointer);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, maskPlatform)){
			
			for(int i=0; i<buildManager.buildPlatforms.Length; i++){
				
				Transform basePlane=buildManager.buildPlatforms[i].thisT;
				if(hit.transform==basePlane){
					
					//calculating the build center point base on the input position
					
					//check if the row count is odd or even number
					float remainderX=UnitUtility.GetWorldScale(basePlane).x*10/_gridSize%2;
					float remainderZ=UnitUtility.GetWorldScale(basePlane).z*10/_gridSize%2;
					
					//get the rotation offset of the plane
					Quaternion rot=Quaternion.LookRotation(hit.point-basePlane.position);
					
					//get the x and z distance from the centre of the plane in the baseplane orientation
					//from this point on all x and z will be in reference to the basePlane orientation
					float dist=Vector3.Distance(hit.point, basePlane.position);
					float distX=Mathf.Sin((rot.eulerAngles.y-basePlane.rotation.eulerAngles.y)*Mathf.Deg2Rad)*dist;
					float distZ=Mathf.Cos((rot.eulerAngles.y-basePlane.rotation.eulerAngles.y)*Mathf.Deg2Rad)*dist;
					
					//get the sign (1/-1) of the x and y direction
					float signX=distX/Mathf.Abs(distX);
					float signZ=distZ/Mathf.Abs(distZ);
					
					//calculate the tile number selected in z and z direction
					float numX=Mathf.Round((distX+(remainderX-1)*(signX*_gridSize/2))/_gridSize);
					float numZ=Mathf.Round((distZ+(remainderZ-1)*(signZ*_gridSize/2))/_gridSize);
					
					//calculate offset in x-axis, 
					float offsetX=-(remainderX-1)*signX*_gridSize/2;
					float offsetZ=-(remainderZ-1)*signZ*_gridSize/2;
					
					//get the pos and apply the offset
					Vector3 p=basePlane.TransformDirection(new Vector3(numX, 0, numZ)*_gridSize);
					p+=basePlane.TransformDirection(new Vector3(offsetX, 0, offsetZ));
					
					//set the position;
					Vector3 pos=p+basePlane.position;
					
					
					
					indicator2.active=true;
		
					indicator2.transform.position=pos;
					indicator2.transform.rotation=basePlane.rotation;
					
					Collider[] cols=Physics.OverlapSphere(pos, _gridSize/2*0.9f, ~maskAll);
					if(cols.Length>0){
						indicator2.renderer.material.SetColor("_TintColor", Color.red);
					}
					else{
						indicator2.renderer.material.SetColor("_TintColor", Color.green);
					}
				}
			}
		}
		else indicator2.active=false;
	}
	
	
	static public bool CheckBuildPoint(Vector3 pointer){
		
		//if(currentBuildInfo!=null) return false;
		
		BuildableInfo buildableInfo=new BuildableInfo();
		
		//layerMask for platform only
		LayerMask maskPlatform=1<<LayerManager.LayerPlatform();
		//layerMask for detect all collider within buildPoint
		LayerMask maskAll=1<<LayerManager.LayerPlatform();
		if(buildManager.terrainColliderLayer>=0) maskAll|=1<<buildManager.terrainColliderLayer;
		
		Ray ray = Camera.main.ScreenPointToRay(pointer);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, maskPlatform)){
			
			for(int i=0; i<buildManager.buildPlatforms.Length; i++){
				
				Transform basePlane=buildManager.buildPlatforms[i].thisT;
				if(hit.transform==basePlane){
					
					//calculating the build center point base on the input position
					
					//check if the row count is odd or even number
					float remainderX=UnitUtility.GetWorldScale(basePlane).x*10/_gridSize%2;
					float remainderZ=UnitUtility.GetWorldScale(basePlane).z*10/_gridSize%2;
					
					//get the rotation offset of the plane
					Quaternion rot=Quaternion.LookRotation(hit.point-basePlane.position);
					
					//get the x and z distance from the centre of the plane in the baseplane orientation
					//from this point on all x and z will be in reference to the basePlane orientation
					float dist=Vector3.Distance(hit.point, basePlane.position);
					float distX=Mathf.Sin((rot.eulerAngles.y-basePlane.rotation.eulerAngles.y)*Mathf.Deg2Rad)*dist;
					float distZ=Mathf.Cos((rot.eulerAngles.y-basePlane.rotation.eulerAngles.y)*Mathf.Deg2Rad)*dist;
					
					//get the sign (1/-1) of the x and y direction
					float signX=distX/Mathf.Abs(distX);
					float signZ=distZ/Mathf.Abs(distZ);
					
					//calculate the tile number selected in z and z direction
					float numX=Mathf.Round((distX+(remainderX-1)*(signX*_gridSize/2))/_gridSize);
					float numZ=Mathf.Round((distZ+(remainderZ-1)*(signZ*_gridSize/2))/_gridSize);
					
					//calculate offset in x-axis, 
					float offsetX=-(remainderX-1)*signX*_gridSize/2;
					float offsetZ=-(remainderZ-1)*signZ*_gridSize/2;
					
					//get the pos and apply the offset
					Vector3 p=basePlane.TransformDirection(new Vector3(numX, 0, numZ)*_gridSize);
					p+=basePlane.TransformDirection(new Vector3(offsetX, 0, offsetZ));
					
					//set the position;
					Vector3 pos=p+basePlane.position;
					
					//check if the position is blocked, by any other obstabcle other than the baseplane itself
					Collider[] cols=Physics.OverlapSphere(pos, _gridSize/2*0.9f, ~maskAll);
					if(cols.Length>0){
						//Debug.Log("something's in the way "+cols[0]);
						return false;
					}
					else{
						//confirm that we can build here
						buildableInfo.buildable=true;
						buildableInfo.position=pos;
						buildableInfo.platform=buildManager.buildPlatforms[i];
					}
					
					buildableInfo.buildableType=buildManager.buildPlatforms[i].buildableType;
					buildableInfo.specialBuildableID=buildManager.buildPlatforms[i].specialBuildableID;
					
					break;
				}
				
			}

		}
		else return false;
		
		currentBuildInfo=buildableInfo;
		
		if(buildManager.enableTileIndicator){
			indicator.active=true;
			indicator.transform.position=currentBuildInfo.position;
			indicator.transform.rotation=currentBuildInfo.platform.thisT.rotation;
		}
			
		return true;
	}
	
	//similar to CheckBuildPoint but called by UnitTower in DragNDrop mode, check tower type before return
	public static bool CheckBuildPoint(Vector3 pointer, _TowerType type){
		return CheckBuildPoint(pointer, type, -1);
	}
	
	public static bool CheckBuildPoint(Vector3 pointer, _TowerType type, int specialID){
		if(!CheckBuildPoint(pointer)) return false;
		
		if(specialID>0){
			if(currentBuildInfo.specialBuildableID!=null && currentBuildInfo.specialBuildableID.Length>0){
				foreach(int specialBuildableID in currentBuildInfo.specialBuildableID){
					if(specialBuildableID==specialID){
						return true;
					}
				}
			}
			return false;
		}
		else{
			if(currentBuildInfo.specialBuildableID!=null && currentBuildInfo.specialBuildableID.Length>0){
				return false;
			}
			
			foreach(_TowerType buildabletype in currentBuildInfo.buildableType){
				if(type==buildabletype){
					return true;
				}
			}
		}
		
		currentBuildInfo.buildable=false;
		return false;
	}
	
	
	//called when a tower building is initated in DragNDrop, instantiate the tower and set it in DragNDrop mode
	public static string BuildTowerDragNDrop(UnitTower tower){
		
		if(GameControl.HaveSufficientResource(tower.GetCost())){
			//~ Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//~ Vector3 pos=ray.GetPoint(10000);
			
			//~ GameObject towerObj=(GameObject)Instantiate(tower.thisObj, pos, Quaternion.identity);
			//~ UnitTower towerCom=towerObj.GetComponent<UnitTower>();
			
			//~ towerCom.StartCoroutine(towerCom.DragNDropRoutine(buildManager.dragNDropStatusFlicker));
			
			int ID=0;
			for(int i=0; i<buildManager.towers.Length; i++){
				if(buildManager.towers[i]==tower){
					ID=i;
					break;
				}
			}
			buildManager.sampleTower[ID].thisObj.SetActiveRecursively(true);
			GameControl.ShowIndicator(buildManager.sampleTower[ID]);
			UnitTower towerCom=buildManager.sampleTower[ID];
			towerCom.StartCoroutine(towerCom.DragNDropRoutine(!buildManager.retainPrefabShaderForSamples));
			
			return "";
		}
		
		//GameMessage.DisplayMessage("Insufficient Resource");
		return "Insufficient Resource";
	}
	
	public static string DragNDropBuilt(UnitTower tower){
		
		//~ if(currentBuildInfo.platform!=null){
			//~ tower.SetTowerID(towerCount+=1);
			//~ if(tower.type!=_TowerType.Mine)
				//~ currentBuildInfo.platform.Build(currentBuildInfo.position, tower);
			//~ ClearBuildPoint();
			//~ return "";
		//~ }
		
			int ID=0;
			for(int i=0; i<buildManager.towers.Length; i++){
				if(buildManager.sampleTower[i]==tower){
					ID=i;
					break;
				}
			}
			
			BuildManager.ClearSampleTower();
			
			return BuildTowerPointNBuild(buildManager.towers[ID]);
		
		//~ return "Invalid Build Point";
	}
	
	//called by any external component to build tower, uses currentBuildInfo, return false if there isnt one
	public static string BuildTowerPointNBuild(UnitTower tower){
		if(currentBuildInfo==null) return "Select a Build Point First";
		
		return BuildTowerPointNBuild(tower, currentBuildInfo.position, currentBuildInfo.platform);
	}
	
	//called by any external component to build tower
	public static string BuildTowerPointNBuild(UnitTower tower, Vector3 pos, Platform platform){
		
		bool matched=false;
		foreach(_TowerType type in platform.buildableType){
			if(tower.type==type){
				matched=true;
				break;
			}
		}
		if(!matched) return "Invalid Tower Type"; 
		
		//check if there are sufficient resource
		int[] cost=tower.GetCost();
		if(GameControl.HaveSufficientResource(cost)){
			GameControl.SpendResource(cost);
			
			GameObject towerObj=(GameObject)Instantiate(tower.thisObj, pos, platform.thisT.rotation);
			UnitTower towerCom=towerObj.GetComponent<UnitTower>();
			towerCom.InitTower(towerCount+=1);
			
			
			//clear the build info and indicator for build manager
			ClearBuildPoint();
			
			return "";
		}
		
		//GameMessage.DisplayMessage("Insufficient Resource");
		return "Insufficient Resource";
	}
	
	
	private UnitTower[] sampleTower;
	private int currentSampleID=-1;
	public static void InitiateSampleTower(){
		buildManager.sampleTower=new UnitTower[buildManager.towers.Length];
		for(int i=0; i<buildManager.towers.Length; i++){
			GameObject towerObj=(GameObject)Instantiate(buildManager.towers[i].gameObject);
			buildManager.sampleTower[i]=towerObj.GetComponent<UnitTower>();
			
			if(!buildManager.retainPrefabShaderForSamples) UnitUtility.SetMat2AdditiveRecursively(buildManager.sampleTower[i].thisT);
			
			if(towerObj.collider!=null) Destroy(towerObj.collider);
			UnitUtility.DestroyColliderRecursively(towerObj.transform);
			towerObj.SetActiveRecursively(false);
			
			//UnitUtility.SetAdditiveMatColorRecursively(towerObj.transform, Color.green);
		}
	}
	
	static public void ShowSampleTower(int ID){
		buildManager._ShowSampleTower(ID);
	}
	public void _ShowSampleTower(int ID){
		if(currentSampleID==ID || currentBuildInfo==null) return;
		
		if(currentSampleID>0){
			ClearSampleTower();
		}
		
		bool matched=false;
		foreach(_TowerType type in currentBuildInfo.buildableType){
			if(type==sampleTower[ID].type){
				matched=true;
				break;
			}
		}
		
		if(!retainPrefabShaderForSamples){
			if(matched) UnitUtility.SetAdditiveMatColorRecursively(sampleTower[ID].thisT, Color.green);
			else UnitUtility.SetAdditiveMatColorRecursively(sampleTower[ID].thisT, Color.red);
		}
		
		currentSampleID=ID;
		sampleTower[ID].thisT.position=currentBuildInfo.position;
		sampleTower[ID].thisObj.SetActiveRecursively(true);
		GameControl.ShowIndicator(sampleTower[ID]);
	}
	
	static public void ClearSampleTower(){
		buildManager._ClearSampleTower();
	}
	public void _ClearSampleTower(){
		if(currentSampleID<0) return;
		
		sampleTower[currentSampleID].thisObj.SetActiveRecursively(false);
		GameControl.ClearIndicator();
		currentSampleID=-1;
	}
	
	
	static public BuildableInfo GetBuildInfo(){
		return currentBuildInfo;
	}
	
	static public UnitTower[] GetTowerList(){
		return buildManager.towers;
	}
	
	static public float GetGridSize(){
		return _gridSize;
	}
	
	Vector3 poss;
	//public bool debugSelectPos=true;
	void OnDrawGizmos(){
		
		//if(debugSelectPos) Gizmos.DrawCube(SelectBuildPos(Input.mousePosition), new Vector3(gridSize, 0, gridSize));
		
	}
	
}





[System.Serializable]
public class BuildableInfo{
	public bool buildable=false;
	public Vector3 position=Vector3.zero;
	public Platform platform;
	public _TowerType[] buildableType=null;
	//public GameObject[] buildableTower=null;
	
	public int[] specialBuildableID;
	
	//cant build
	public void BuildSpotInto(){}
	
	//can build anything
	public void BuildSpotInto(Vector3 pos){
		position=pos;
	}
	
	//can build with restriction to certain tower type
	public void BuildSpotInto(Vector3 pos, _TowerType[] bT){
		position=pos;
		buildableType=bT;
	}
}