using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCreep : Unit {

	public delegate void ScoreHandler(int waveID);
	public static event ScoreHandler onScoreE;
	
	public delegate void DeductLifeHandler(int waveID);
	public static event DeductLifeHandler onDeductLifeE;
	
	private int unitID;
	public int GetUnitID(){ return unitID; }
	
	public float moveSpeed=3;
	public bool immuneToSlow=false;
	
	public bool flying=false;
	public float flightHeightOffset=3f;
	
	public int[] value=new int[1];
	
	public GameObject spawnEffect;
	public GameObject deadEffect;
	public GameObject scoreEffect;
	
	public GameObject animationBody;
	private Animation aniBody;
	public AnimationClip[] animationSpawn;
	public AnimationClip[] animationMove;
	public AnimationClip[] animationHit;
	public AnimationClip[] animationDead;
	public AnimationClip[] animationScore;
	public float moveAnimationModifier=1.0f;
	
	public AudioClip audioSpawn;
	public AudioClip audioHit;
	public AudioClip audioDead;
	public AudioClip audioScore;
	
	[HideInInspector] public int waveID;
	
	
	public override void Awake () {
		base.Awake();
		
		SetSubClassInt(this);
		
		if(thisObj.collider==null){
			SphereCollider col=thisObj.AddComponent<SphereCollider>();
			col.center=new Vector3(0, 0.0f, 0);
			col.radius=0.25f;
		}
		
		if(animationBody!=null){
			if(aniBody==null){
				aniBody=animationBody.GetComponent<Animation>();
				if(aniBody==null) aniBody=animationBody.AddComponent<Animation>();
			}
			
			if(animationSpawn!=null && animationSpawn.Length>0){
				foreach(AnimationClip clip in animationSpawn){
					aniBody.AddClip(clip, clip.name);
					aniBody.animation[clip.name].layer=1;
					aniBody.animation[clip.name].wrapMode=WrapMode.Once;
				}
			}
			
			if(animationMove!=null && animationMove.Length>0){
				foreach(AnimationClip clip in animationMove){
					aniBody.AddClip(clip, clip.name);
					aniBody.animation[clip.name].layer=0;
					aniBody.animation[clip.name].wrapMode=WrapMode.Loop;
				}
			}
			
			if(animationHit!=null && animationHit.Length>0){
				foreach(AnimationClip clip in animationHit){
					aniBody.AddClip(clip, clip.name);
					aniBody.animation[clip.name].layer=3;
					aniBody.animation[clip.name].wrapMode=WrapMode.Once;
				}
			}
			
			if(animationDead!=null && animationDead.Length>0){
				foreach(AnimationClip clip in animationDead){
					aniBody.AddClip(clip, clip.name);
					aniBody.animation[clip.name].layer=3;
					aniBody.animation[clip.name].wrapMode=WrapMode.Once;
				}
			}
			
			if(animationScore!=null && animationScore.Length>0){
				foreach(AnimationClip clip in animationScore){
					aniBody.AddClip(clip, clip.name);
					aniBody.animation[clip.name].layer=3;
					aniBody.animation[clip.name].wrapMode=WrapMode.Once;
				}
			}
		}
		
		if(spawnEffect!=null) ObjectPoolManager.New(spawnEffect, 5, false);
		if(deadEffect!=null) ObjectPoolManager.New(deadEffect, 5, false);
	}

	public override void Start () {
		if(!flying) thisObj.layer=LayerManager.LayerCreep();
		else thisObj.layer=LayerManager.LayerCreepF();
		
		base.Start();
	}
	
	public void SetMoveSpeed(float moveSpd){
		moveSpeed=moveSpd;
	}
	
	
	//init a unitCreep, give it a list of Vector3 point as the path it should follow
	public void Init(List<Vector3> waypoints, int uID, int wID){
		base.Init();
		
		//reset waypoint
		SetWPCounter(0);
		
		unitID=uID;
		waveID=wID;
		wp=waypoints;
		wpMode=true;
		
		if(flying) thisT.position+=new Vector3(0, flightHeightOffset, 0);
		
		if(!stopMoving) currentMoveSpd=moveSpeed;
		
		if(aniBody!=null && animationMove!=null && animationMove.Length>0){
			foreach(AnimationClip clip in animationMove){
				aniBody.animation[clip.name].speed=currentMoveSpd*moveAnimationModifier;
			}
		}
		
		PlaySpawn();
		PlayMove();
	}
	
	//init a unitCreep, give it a path instance so it can retirve the path
	public void Init(PathTD p, int uID, int wID){
		base.Init();
		
		//reset waypoint
		SetWPCounter(0);
		
		unitID=uID;
		waveID=wID;
		path=p;
		wpMode=false;
		
		if(flying) thisT.position+=new Vector3(0, flightHeightOffset, 0);
		
		if(!stopMoving) currentMoveSpd=moveSpeed;
		
		if(aniBody!=null && animationMove!=null && animationMove.Length>0){
			foreach(AnimationClip clip in animationMove){
				aniBody.animation[clip.name].speed=currentMoveSpd*moveAnimationModifier;
			}
		}
		
		if(spawnEffect!=null) ObjectPoolManager.Spawn(spawnEffect, thisT.position, Quaternion.identity);
		PlaySpawn();
		PlayMove();
		
	}
	
	public void Dead(){
		GameControl.GainResource(value);
		
		if(deadEffect!=null) ObjectPoolManager.Spawn(deadEffect, thisT.position, Quaternion.identity);
		float duration=PlayDead();
		StartCoroutine(Unspawn(duration));
		
	}
	
	
	
	public override void Update () {
		base.Update();
		
		if(!stunned && !dead) {
			MoveWPMode();
		}
	}
	
	
	//~ protected PathSection currentPS;
	protected List<Vector3> subPath=new List<Vector3>();
	protected int currentPathID=0;
	protected int subWPCounter=0;
	
	
	//for using simple point to point path
	void MoveWPMode(){
		//execute as long as there are unreached waypoint in the path
		if(wpCounter<wp.Count){
			//move to the next waypoint, if return true, then update to the next waypoint
			if(MoveToPoint(wp[wpCounter])){
				wpCounter+=1;
			}
		}
		else ReachFinalWayPoint();
	}
	
	//function call to rotate and move toward a pecific point, return true when the point is reached
	public override bool MoveToPoint(Vector3 point){
		//Debug.DrawLine(thisT.position, thisT.position+new Vector3(0, 2, 0), Color.red, 0.1f);
		
		if(flying) point+=new Vector3(0, flightHeightOffset, 0);
		
		float dist=Vector3.Distance(point, thisT.position);
		
		//if the unit have reached the point specified
		if(dist<0.15f) return true;
		
		//rotate towards destination
		Quaternion wantedRot=Quaternion.LookRotation(point-thisT.position);
		thisT.rotation=Quaternion.Slerp(thisT.rotation, wantedRot, rotateSpd*Time.deltaTime);
		
		//move, with speed take distance into accrount so the unit wont over shoot
		Vector3 dir=(point-thisT.position).normalized;
		thisT.Translate(dir*Mathf.Min(dist, currentMoveSpd * Time.deltaTime * slowModifier), Space.World);
		
		return false;
	}
	
	
	void ReachFinalWayPoint(){
		//score and killself
		if(!scored){
			scored=true;
			if(onScoreE!=null) onScoreE(waveID);
			Score();
		}
	}
	
	
	public void Score(){
		if(onDeductLifeE!=null) onDeductLifeE(1);
		if(scoreEffect!=null) ObjectPoolManager.Spawn(scoreEffect, thisT.position, Quaternion.identity);
		float duration=PlayScore();
		
		StartCoroutine(Unspawn(duration));
	}
	
	
	public void Stunned(){
		if(aniBody!=null){
			if(animationMove!=null && animationMove.Length>0){
				for(int i=0; i<animationMove.Length; i++){
					aniBody.Stop(animationMove[i].name);
				}
			}
		}
	}
	
	public void Unstunned(){
		PlayMove();
	}
	
	private AnimationClip[] animationAttack;
	private AnimationClip animationIdle;
	
	public void SetAttackAnimation(AnimationClip aniAttack){
		AnimationClip[] aniAttacks=new AnimationClip[1];
		aniAttacks[0]=aniAttack;
		SetAttackAnimation(aniAttacks);
	}
	
	public void SetAttackAnimation(AnimationClip[] aniAttack){
		animationAttack=aniAttack;
		
		if(animationBody==null) return;
		else {
			if(aniBody==null){
				aniBody=animationBody.GetComponent<Animation>();
				if(aniBody==null) aniBody=animationBody.AddComponent<Animation>();
			}
		}
		
		if(aniBody!=null && animationAttack!=null && animationAttack.Length>0){
			foreach(AnimationClip clip in animationAttack){
				aniBody.AddClip(clip, clip.name);
				aniBody.animation[clip.name].layer=5;
				aniBody.animation[clip.name].wrapMode=WrapMode.Once;
			}
		}
	}
	
	public void SetIdleAnimation(AnimationClip aniIdle){
		animationIdle=aniIdle;
		if(aniBody!=null){
			aniBody.AddClip(animationIdle, animationIdle.name);
			aniBody.animation[animationIdle.name].layer=-1;
			aniBody.animation[animationIdle.name].wrapMode=WrapMode.Loop;
		}
	}
	
	public void StopAnimation(){
		if(aniBody!=null) aniBody.Stop();
		
		if(aniBody!=null && animationIdle!=null){
			aniBody.Play(animationIdle.name);
		}
	}
	
	public void ResumeAnimation(){
		PlayMove();
	}
	
	public bool PlayAttack(){
		if(aniBody!=null && animationAttack!=null && animationAttack.Length>0){
			aniBody.CrossFade(animationAttack[Random.Range(0, animationAttack.Length-1)].name);
			return true;
		}
		return false;
	}
	
	public void PlayMove(){
		if(aniBody!=null && animationMove!=null && animationMove.Length>0){
			aniBody.Play(animationMove[Random.Range(0, animationMove.Length-1)].name);
		}
	}
	
	public void PlaySpawn(){
		if(aniBody!=null && animationSpawn!=null && animationSpawn.Length>0){
			aniBody.CrossFade(animationSpawn[Random.Range(0, animationSpawn.Length-1)].name);
		}
		
		if(audioSpawn!=null) AudioManager.PlaySound(audioSpawn, thisT.position);
	}
	
	public void PlayHit(){
		if(aniBody!=null && animationHit!=null && animationHit.Length>0){
			aniBody.CrossFade(animationHit[Random.Range(0, animationHit.Length-1)].name);
		}
		
		if(audioHit!=null) AudioManager.PlaySound(audioHit, thisT.position);
	}
	
	public float PlayDead(){
		float duration=0;
		
		if(aniBody!=null){
			aniBody.Stop();
		}
		
		if(aniBody!=null && animationDead!=null && animationDead.Length>0){
			int rand=Random.Range(0, animationDead.Length-1);
			aniBody.CrossFade(animationDead[rand].name);
			duration=animationDead[rand].length;
		}
		
		if(audioDead!=null){
			AudioManager.PlaySound(audioDead, thisT.position);
			//duration=Mathf.Max(audioDead.length, duration);
		}
		
		return duration;
	}
	
	public float PlayScore(){
		float duration=0;
		
		if(aniBody!=null && animationScore!=null && animationScore.Length>0){
			int rand=Random.Range(0, animationDead.Length-1);
			aniBody.CrossFade(animationScore[rand].name);
			duration=animationScore[rand].length;
		}
		
		if(audioScore!=null) {
			AudioManager.PlaySound(audioScore, thisT.position);
			duration=Mathf.Max(audioScore.length, duration);
		}
		
		return duration;
	}
	
	IEnumerator Unspawn(float duration){
		yield return new WaitForSeconds(duration);
		ObjectPoolManager.Unspawn(thisObj);
	}

}
