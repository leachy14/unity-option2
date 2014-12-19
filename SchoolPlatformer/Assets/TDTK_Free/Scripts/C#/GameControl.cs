using UnityEngine;
using System.Collections;

public enum _GameState{Idle, Started, Ended}

[RequireComponent (typeof (LayerManager))]


public class GameControl : MonoBehaviour {
	
	public delegate void GameOverHandler(bool win); //true if win
	public static event GameOverHandler onGameOverE;
	
	public delegate void ResourceHandler(); 
	public static event ResourceHandler onResourceE;
	
	public delegate void LifeHandler(); 
	public static event LifeHandler onLifeE;

	static public _GameState gameState=_GameState.Idle;
	
	public int playerLife=10;
	public int playerResource=100;
	
	public float sellTowerRefundRatio=0.5f;

	[HideInInspector] public LayerManager layerManager;
	public SpawnManager spawnManager;
	private int totalWaveCount;
	private int currentWave=0;
	
	public Transform rangeIndicatorH;
	public Transform rangeIndicatorF;
	
	
	static public GameControl gameControl;
	
	public float buildingBarWidthModifier=1f;
	public float buildingBarHeightModifier=1f;
	public Vector3 buildingBarPosOffset=new Vector3(0, -0.5f, 0);
	

	public bool autoCreateOverlayCamera=false;
	

	void Awake(){
		ObjectPoolManager.Init();
		
		GameMessage gameMessage=(GameMessage)FindObjectOfType(typeof(GameMessage));
		if(gameMessage==null){
			GameMessage.Init();
		}
		
		AudioManager audioManager=(AudioManager)FindObjectOfType(typeof(AudioManager));
		if(audioManager==null){
			AudioManager.Init();
		}
		
		//LayerManager.Init();

		gameControl=this;
		
		gameState=_GameState.Idle;
		
		if(rangeIndicatorH){
			rangeIndicatorH=(Transform)Instantiate(rangeIndicatorH);
			rangeIndicatorH.parent=transform;
		}
		if(rangeIndicatorF){
			rangeIndicatorF=(Transform)Instantiate(rangeIndicatorF);
			rangeIndicatorF.parent=transform;
		}
		ClearIndicator();
		
		OverlayManager.SetModifier(buildingBarWidthModifier, buildingBarHeightModifier);
		OverlayManager.SetOffset(buildingBarPosOffset);
		
	}
	

	// Use this for initialization
	void Start () {
		AudioManager.PlayNewWaveSound ();
		
		totalWaveCount=spawnManager.waves.Length;
		
		Camera mainCam=Camera.main;
		LayerMask layerUI=1<<LayerManager.LayerMiscUIOverlay();
		mainCam.cullingMask=mainCam.cullingMask&~layerUI;
		
		//Create OverlayCamera
		if(autoCreateOverlayCamera){
			
			Transform mainCamT=mainCam.transform;
			
			GameObject overlayCamObj=new GameObject();
			overlayCamObj.name="OverlayCamera";
			
			LayerMask layer=1<<LayerManager.LayerOverlay();
			
			mainCam.cullingMask=mainCam.cullingMask&~layer;
			
			Camera overlayCam=overlayCamObj.AddComponent<Camera>();
			
			overlayCam.orthographic=mainCam.orthographic;
			overlayCam.orthographicSize=mainCam.orthographicSize;

			overlayCam.clearFlags=CameraClearFlags.Depth;
			overlayCam.depth=mainCam.depth + 1;
			overlayCam.cullingMask=layer;
			overlayCam.fieldOfView=mainCam.fieldOfView;
			
			overlayCamObj.transform.parent=mainCamT;
			overlayCamObj.transform.rotation=mainCamT.rotation;
			overlayCamObj.transform.localPosition=Vector3.zero;
		}
		
		Time.timeScale=1;
	}
	
	void OnEnable(){
		SpawnManager.onWaveStartSpawnE += OnWaveStartSpawned;
		SpawnManager.onWaveClearedE += OnWaveCleared;
		
		UnitCreep.onDeductLifeE += OnDeductLife;
		UnitTower.onDestroyE += OnTowerDestroy;
		UnitTower.onDragNDropE += onTowerDragNDropEnd;
	}
	
	void OnDisable(){
		SpawnManager.onWaveStartSpawnE -= OnWaveStartSpawned;
		SpawnManager.onWaveClearedE -= OnWaveCleared;
		
		UnitCreep.onDeductLifeE -= OnDeductLife;
		UnitTower.onDestroyE -= OnTowerDestroy;
		UnitTower.onDragNDropE -= onTowerDragNDropEnd;
	}
	
	void OnDeductLife(int lifeCost){
		playerLife-=lifeCost;
		if(playerLife<=0) playerLife=0;
		
		if(onLifeE!=null) onLifeE();
		
		if(playerLife==0){
			//game over, player lost
			gameState=_GameState.Ended;
			if(onGameOverE!=null) onGameOverE(false);
		}
	}
	
	void OnTowerDestroy(UnitTower tower){
		if(selectedTower==tower || selectedTower==null || !selectedTower.thisObj.active){
			ClearSelection();
		}
	}
	
	//IEnumerator _TowerDestroy(UnitTower tower)
	
	
	public static int GetPlayerLife(){
		return gameControl.playerLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	private Texture icon;
	private bool loadAttempt=false;
	void OnGUI(){
		if(icon==null){
			if(!loadAttempt) icon=(Texture)Resources.Load("Title&Icon/icon_free");
			
			GUI.Label(new Rect(Screen.width-80, Screen.height-25-25, 100, 30), "TDTK Free");
		}
		else{
			//GUIContent guiContent=new GUIContent(" TDTK Free", icon); 
			//~ GUI.Label(new Rect(Screen.width-100, Screen.height-30-25, 100, 30), guiContent);
			GUI.Label(new Rect(Screen.width-65, Screen.height-65-25, 60, 60), icon);
		}
	}
	*/
	
	void OnWaveStartSpawned(int waveID){
		currentWave+=1;
		
		//if game is not yet started, start it now
		if(gameState==_GameState.Idle) gameState=_GameState.Started;
	}
	
	void WaveSpawned(int waveID){
		
	}
	
	void OnWaveCleared(int waveID){
		Debug.Log("Wave "+waveID+" has been cleared");
		if(waveID==totalWaveCount-1){
			//game over, player won
			gameState=_GameState.Ended;
			if(onGameOverE!=null) onGameOverE(true);
		}
	}
	
	
	
	static public UnitTower selectedTower;
	
	static public UnitTower Select(Vector3 pointer){
		//change this
		int layer=LayerManager.LayerTower();
		
		LayerMask mask=1<<layer;
		Ray ray = Camera.main.ScreenPointToRay(pointer);
		RaycastHit hit;
		if(!Physics.Raycast(ray, out hit, Mathf.Infinity, mask)){
			return null;
		}
		
		selectedTower=hit.transform.gameObject.GetComponent<UnitTower>();
		//selectedTower.Select();
		
		gameControl._ShowIndicator(selectedTower);
		
		return selectedTower;
	}
	
	public static void ShowIndicator(UnitTower tower){
		gameControl._ShowIndicator(tower);
	}
	
	public void _ShowIndicator(UnitTower tower){
		
		//show range indicator on the tower
		//for support tower, show friendly range indicator
		if(tower.type==_TowerType.SupportTower){
			//Debug.Log(tower.type);
			float range=tower.GetRange();
			if(rangeIndicatorF!=null){
				rangeIndicatorF.position=tower.thisT.position;
				rangeIndicatorF.localScale=new Vector3(2*range/10, 1, 2*range/10);
				rangeIndicatorF.renderer.enabled=true;
			}
			if(rangeIndicatorH!=null) rangeIndicatorH.renderer.enabled=false;
		}
		//for support tower, show hostile range indicator
		else{
			float range=tower.GetRange();
			if(rangeIndicatorH!=null){
				rangeIndicatorH.position=tower.thisT.position;
				rangeIndicatorH.localScale=new Vector3(2*range/10, 1, 2*range/10);
				rangeIndicatorH.renderer.enabled=true;
			}
			if(rangeIndicatorF!=null) rangeIndicatorF.renderer.enabled=false;
		}
	}
	
	private bool dragNDrop=false;
	public static void DragNDropIndicator(UnitTower tower){
		gameControl._ShowIndicator(tower);
		gameControl.StartCoroutine(gameControl._DragNDropIndicator(tower));
	}
	IEnumerator _DragNDropIndicator(UnitTower tower){
		
		dragNDrop=true;
		
		while(dragNDrop){
			if(tower.type==_TowerType.SupportTower){
				if(rangeIndicatorF!=null) rangeIndicatorF.position=tower.thisT.position;
			}
			else{
				if(rangeIndicatorH!=null) rangeIndicatorH.position=tower.thisT.position;
			}
				
			yield return null;
		}
		ClearIndicator();
	}
	
	void onTowerDragNDropEnd(string msg){
		dragNDrop=false;
	}
	
	public static void ClearIndicator(){
		gameControl._ClearIndicator();
	}
	
	public void _ClearIndicator(){
		if(rangeIndicatorH!=null) rangeIndicatorH.renderer.enabled=false;
		if(rangeIndicatorF!=null) rangeIndicatorF.renderer.enabled=false;
	}
	
	static public void ClearSelection(){
		selectedTower=null;
		gameControl._ClearIndicator();
	}
	
	//call when a tower complete upgrade, if tower is currently selected, update the range indicator
	static public void TowerUpgradeComplete(UnitTower tower){
		if(tower==selectedTower){
			gameControl._ShowIndicator(tower);
		}
	}
	
	
	static public void GainResource(int val){
		gameControl.playerResource+=val;
		if(onResourceE!=null) onResourceE();
	}
	
	static public void GainResource(int[] val){
		gameControl.playerResource+=val[0];
		if(onResourceE!=null) onResourceE();
	}
	
	static public void SpendResource(int val){
		gameControl.playerResource-=val;
		if(onResourceE!=null) onResourceE();
	}
	
	static public void SpendResource(int[] val){
		gameControl.playerResource-=val[0];
		if(onResourceE!=null) onResourceE();
	}
	
	static public int GetResourceVal(){
		return gameControl.playerResource;
	}

	static public bool HaveSufficientResource(int val){
		if(gameControl.playerResource>=val) return true;
		return false;
	}
	
	static public bool HaveSufficientResource(int[] vals){
		if(gameControl.playerResource>=vals[0]) return true;
		return false;
	}
	
	static public float GetSellTowerRefundRatio(){
		return gameControl.sellTowerRefundRatio;
	}
	
	static public void GradualPause(){
		
	}
	
	static public void GradualResume(){
		
	}
}
