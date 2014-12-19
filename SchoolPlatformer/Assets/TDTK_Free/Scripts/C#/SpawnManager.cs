using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum _SpawnMode{Continuous, WaveCleared, RoundBased, SkippableContinuous, SkippableWaveCleared}

public class SpawnManager : MonoBehaviour {

	public delegate void WaveStartSpawnHandler(int waveID);
	public static event WaveStartSpawnHandler onWaveStartSpawnE;

	public delegate void WaveSpawnedHandler(int waveID);
	public static event WaveSpawnedHandler onWaveSpawnedE;
	
	public delegate void WaveClearedHandler(int waveID);
	public static event WaveClearedHandler onWaveClearedE;
	
	public delegate void WaveClearedForSpawningHandler(bool flag);
	public static event WaveClearedForSpawningHandler onClearForSpawningE;
	
	public _SpawnMode spawnMode=_SpawnMode.Continuous;
	
	public PathTD defaultPath;
	private List<Vector3> waypoints;
	
	public Wave[] waves=new Wave[1];
	
	private bool isClearForSpawning=true;
	private int currentWave=0;
	
	private float timeLastSpawn;
	private float waitDuration;
	
	static private SpawnManager spawnManager;
	
	private int totalSpawnCount=0;
	
	void Awake(){
		spawnManager=this;
	}
	
	void Start () {
		//prespawn the unit
		foreach(Wave wave in waves){
			foreach(SubWave subWave in wave.subWaves){
				ObjectPoolManager.New(subWave.unit, subWave.count);
			}
		}
	}
	
	void OnEnable(){
		Unit.onDeadE += OnCheckIsWaveCleared;
		UnitCreep.onScoreE += OnCheckIsWaveCleared;
	}
	
	void OnDisable(){
		Unit.onDeadE -= OnCheckIsWaveCleared;
		UnitCreep.onScoreE -= OnCheckIsWaveCleared;
	}
	
	//external call for spawing of any kind, return true if spawning is successful, else return false
	public bool _Spawn(){
		if(isClearForSpawning && GameControl.gameState!=_GameState.Ended){
			//if currentwave has exceed available wave length
			if(currentWave>=waves.Length){
				Debug.Log("All wave has been spawned");
				return false;
			}
			else{
				//set gamestate to started if this is the first wave
				if(currentWave==0){
					GameControl.gameState=_GameState.Started;
					Debug.Log("game started");
				}
				
				//initiate corresponding spawn routine
				if(spawnMode==_SpawnMode.Continuous || spawnMode==_SpawnMode.SkippableContinuous){
					if(currentWave==0) StartCoroutine(ContinousTimedSpawn());
					else ContinousTimedSpawnSkip();
				}
				if(spawnMode==_SpawnMode.WaveCleared || spawnMode==_SpawnMode.SkippableWaveCleared){
					if(currentWave==0) StartCoroutine(WaveClearedSpawn());
					else WaveClearedSpawnSkip();
				}
				if(spawnMode==_SpawnMode.RoundBased){
					SpawnWave();
				}
			}
		}
		else {
			//anything else
			Debug.Log("SpawnManager is not ready for spawning next wave");
			return false;
		}
		
		return true;
	}
	
	//call when a unit is dead, check if a wave has been cleared
	void OnCheckIsWaveCleared(int waveID){
		//reduce the acvitve unit of the corrsponding wave
		waves[waveID].activeUnitCount-=1;
		//if all the unit in that wave is spawned and the acitve unit count is 0, then the wave is cleared
		if(waves[waveID].spawned && waves[waveID].activeUnitCount==0){
			waves[waveID].cleared=true;
			
			//GameControl.GainResource(waves[waveID].resource);
			GameControl.GainResource(waves[waveID].resourceGain);
			//for(int i=0; i<waves[waveID].resourceGain.Length; i++){
			//	GameControl.GainResource(i, waves[waveID].resourceGain[i]);
			//}
			
			if(onWaveClearedE!=null) onWaveClearedE(waveID);
			
			AudioManager.PlayWaveClearedSound();
			
			//trigger event that the spawnmanger is clear to spawn again
			if(spawnMode==_SpawnMode.RoundBased && currentWave<waves.Length){
				isClearForSpawning=true;
				onClearForSpawningE(isClearForSpawning);
			}
		}
	}
	
	
	//call to skip ContinousTimedSpawn waiting time and spawn instantly
	private void ContinousTimedSpawnSkip(){
		if(GameControl.gameState!=_GameState.Ended){
			timeLastSpawn=Time.time;
			waitDuration=waves[currentWave].waveInterval;
			SpawnWave();
		}
		else Debug.Log("The game is over");
	}
	
	//continous spawn mode, spawn wave according to predefined interval
	IEnumerator ContinousTimedSpawn(){
		waitDuration=waves[currentWave].waveInterval;
		timeLastSpawn=-waitDuration;
		while(currentWave<waves.Length && GameControl.gameState!=_GameState.Ended){
			if(Time.time-timeLastSpawn>=waitDuration){
				timeLastSpawn=Time.time;
				waitDuration=waves[currentWave].waveInterval;
				SpawnWave();
			}
			yield return null;
		}
	}
	
	//call to skip wave cleared spawn mode, will instantly spwan next wave
	private void WaveClearedSpawnSkip(){
		if(GameControl.gameState!=_GameState.Ended){
			SpawnWave();
		}
	}
	
	//wave cleared spawn mode, spawn new wave automatically when current wave is cleared
	IEnumerator WaveClearedSpawn(){
		SpawnWave();
		while(currentWave<waves.Length && GameControl.gameState!=_GameState.Ended){
			if(waves[currentWave-1].cleared){
				SpawnWave();
			}
			yield return null;
		}
	}
	
	//spawning call for individual wave
	//from here, multiple spawn routine for each creep type in the wave is called
	public void SpawnWave(){
		//Debug.Log("spawn wave "+(currentWave+1));
		GameMessage.DisplayMessage("Incoming wave "+(currentWave+1)+"!");
		
		AudioManager.PlayNewWaveSound();
		
		//disable user initiated spawning until current wave has done spawning
		isClearForSpawning=false;
		onClearForSpawningE(isClearForSpawning);
		
		foreach(SubWave subWave in waves[currentWave].subWaves){
			StartCoroutine(SpawnSubwave(subWave, waves[currentWave], currentWave));
		}
		
		StartCoroutine(CheckSpawn(currentWave));
		
		currentWave+=1;
		if(onWaveStartSpawnE!=null) onWaveStartSpawnE(currentWave+1);
	}

	//actual spawning routine, responsible for spawning one type of creep only
	IEnumerator SpawnSubwave(SubWave subWave, Wave parentWave, int waveID){
		yield return new WaitForSeconds(subWave.delay);
		int spawnCount=0;
		while(spawnCount<subWave.count){

			Vector3 pos;
			Quaternion rot;
			
			PathTD tempPath;
			if(subWave.path==null) tempPath=defaultPath;
			else tempPath=subWave.path;
			
			pos=tempPath.waypoints[0].position;
			rot=tempPath.waypoints[0].rotation;
			
			GameObject obj=ObjectPoolManager.Spawn(subWave.unit, pos, rot);
			//Unit unit=obj.GetComponent<Unit>();
			UnitCreep unit=obj.GetComponent<UnitCreep>();
			if(subWave.overrideHP>0) unit.SetFullHP(subWave.overrideHP);
			if(subWave.overrideMoveSpd>0) unit.SetMoveSpeed(subWave.overrideMoveSpd);
			
			List<Vector3> waypoints=new List<Vector3>();
			foreach(Transform pointT in tempPath.waypoints){
				waypoints.Add(pointT.position);
			}
			unit.Init(waypoints, totalSpawnCount, waveID);
			//~ unit.Init(tempPath, totalSpawnCount, waveID);
			
			totalSpawnCount+=1;
			
			parentWave.activeUnitCount+=1;
			
			spawnCount+=1;
			if(spawnCount==subWave.count) break;
			
			yield return new WaitForSeconds(subWave.interval);
		}
		
		subWave.spawned=true;
	}
	
	//check if all the spawning in one individual wave is done
	IEnumerator CheckSpawn(int waveID){
		while(true){
			bool allSpawned=true;
			foreach(SubWave subWave in waves[waveID].subWaves){
				if(!subWave.spawned) allSpawned=false;
			}
			if(allSpawned) break;
			yield return null;
		}
		
		Debug.Log("wave "+(currentWave-1)+" has done spawning");
		
		//set the wave spawn flag to true, so check can be run to see if this wave is cleared
		waves[waveID].spawned=true;
		
		if(currentWave<waves.Length){
			//enabled flag so next wave can be skip if skiappable spawn mode is selected
			if(spawnMode==_SpawnMode.SkippableContinuous || spawnMode==_SpawnMode.SkippableWaveCleared)
				isClearForSpawning=true;
			
			//trigger event if there are listener
			//telling listener is current wave is done spawning
			if(onWaveSpawnedE!=null) onWaveSpawnedE(waveID);
			//telling listener if spawning is now available/unavailable depending on flag isClearForSpawning 
			if(onClearForSpawningE!=null) onClearForSpawningE(isClearForSpawning);
		}
	}
	
	static public int NewUnitID(){
		spawnManager.totalSpawnCount+=1;
		return spawnManager.totalSpawnCount-1;
	}
	
	static public void AddActiveUnit(int waveID, int num){
		spawnManager.waves[waveID].activeUnitCount+=num;
	}
	
	public float _TimeNextSpawn(){
		return timeLastSpawn+waitDuration-Time.time;
	}
	
	static public bool IsClearForSpawning(){
		return spawnManager.isClearForSpawning;
	}
	
	static public bool Spawn(){
		return spawnManager._Spawn();
	}
	
	static public int GetCurrentWave(){
		return spawnManager.currentWave;
	}
	
	static public int GetTotalWave(){
		return spawnManager.waves.Length;
	}
	
	static public float GetTimeNextSpawn(){
		return spawnManager._TimeNextSpawn();
	}
	
	static public _SpawnMode GetSpawnMode(){
		return spawnManager.spawnMode;
	}
}

[System.Serializable]
public class SubWave{
	public GameObject unit;
	public int count;
	public float interval=1;
	public float delay;
	public PathTD path;
	public float overrideHP=0;
	public float overrideMoveSpd=0;
	
	[HideInInspector] public bool spawned=false;
	
//	[HideInInspector] public UnitCreep[] unitList;
}

[System.Serializable]
public class Wave{
	public SubWave[] subWaves=new SubWave[1];
	public float waveInterval;
	//public int resource;
	public int[] resourceGain=new int[1];
	
//	[HideInInspector] public List<UnitCreep> activeUnitList=new List<UnitCreep>();

	[HideInInspector] public int activeUnitCount=0;
	
	[HideInInspector] public bool spawned=false; //flag indicating weather all unit in the wave have been spawn
	[HideInInspector] public bool cleared=false; //flag indicating weather the wave has been cleared
	
}
