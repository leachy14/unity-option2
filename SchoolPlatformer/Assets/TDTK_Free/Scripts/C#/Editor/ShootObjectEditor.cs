using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor(typeof(ShootObject))]
public class ShootObjectEditor : Editor {

	
	//~ [MenuItem ("TDTK/LayerEditor")]
    //~ static void Init(){
        //~ // Get existing open window or if none, make a new one:
        //~ window = (LayerEditor)EditorWindow.GetWindow(typeof (LayerEditor));
		//~ window.minSize=new Vector2(300, 300);
		
		//~ layerCreep=PlayerPrefs .GetInt("CreepLayer", 31);
		//~ layerCreepF=PlayerPrefs .GetInt("CreepFLayer", 30);
			
		//~ layerTower=PlayerPrefs .GetInt("TowerLayer", 29);
		//~ layerPlatform=PlayerPrefs .GetInt("PlatformLayer", 28);
		
		//~ GetSpawnManager();
    //~ }
	
	private static int type=0;	
	//private static _ShootObjectType tType;
	private static string[] typeLabel=new string[4];
	private static int[] typeID=new int[4];
	private static bool init=false;
	
	private static int effectType=0;
	private static string[] effectTypeLabel=new string[2];
	private static int[] effectTypeID=new int[2];
	
	static void Init(){
		//public enum _ShootObjectType{Projectile, Missile, Instant, Effect}
	
		init=true;
		
		typeLabel[0]="Projectile";
		typeLabel[1]="Missile";
		typeLabel[2]="Beam|Instant";
		typeLabel[3]="Effect";
		
		typeID[0]=0;
		typeID[1]=1;
		typeID[2]=2;
		typeID[3]=3;
		
		effectTypeLabel[0]="ParticleSystem";
		effectTypeLabel[1]="LegacyParticleSystem";
		
		effectTypeID[0]=1;
		effectTypeID[1]=2;
		
	}
	
	public override void OnInspectorGUI(){
		ShootObject shootObj = (ShootObject)target;
		
		if(!init) Init();
		
		GUI.changed = false;
		
		//shootObj.type
		
		//shootObj.type= EditorGUILayout.EnumPopup("Type: ", shootObj.type);
		
		if(shootObj.type==_ShootObjectType.Projectile) type=0;
		else if(shootObj.type==_ShootObjectType.Missile) type=1;
		else if(shootObj.type==_ShootObjectType.Beam) type=2;
		else if(shootObj.type==_ShootObjectType.Effect) type=3;
		
		type = EditorGUILayout.IntPopup("Type: ", type, typeLabel, typeID);
		EditorGUILayout.Space();
		
		if(type==0){
			shootObj.type=_ShootObjectType.Projectile;
		//if(shootObj.type==_ShootObjectType.Projectile){
			
			
			shootObj.speed = EditorGUILayout.FloatField("Speed:", shootObj.speed);
			shootObj.maxShootRange = EditorGUILayout.FloatField("Max Range:", shootObj.maxShootRange);
			shootObj.maxShootAngle = EditorGUILayout.FloatField("Max Angle:", shootObj.maxShootAngle);
		}
		else if(type==1){
			shootObj.type=_ShootObjectType.Missile;
		//if(shootObj.type==_ShootObjectType.Missile){
			
			
			shootObj.speed = EditorGUILayout.FloatField("Speed:", shootObj.speed);
			shootObj.maxShootRange = EditorGUILayout.FloatField("Max Range:", shootObj.maxShootRange);
			shootObj.maxShootAngle = EditorGUILayout.FloatField("Max Angle:", shootObj.maxShootAngle);
		}
		else if(type==2){
			shootObj.type=_ShootObjectType.Beam;
		//if(shootObj.type==_ShootObjectType.Instant){
			
			
			shootObj.lineRenderer=(LineRenderer)EditorGUILayout.ObjectField("LineRenderer:", shootObj.lineRenderer, typeof(LineRenderer), false);
			shootObj.beamLength = EditorGUILayout.FloatField("Beam Length:", shootObj.beamLength);
			shootObj.duration = EditorGUILayout.FloatField("Active Duration:", shootObj.duration);
			shootObj.continousDamage = EditorGUILayout.Toggle("ContinousDamage", shootObj.continousDamage);
			
			if(shootObj.lineRenderer==null) shootObj.lineRenderer=shootObj.gameObject.GetComponent<LineRenderer>();
		}
		else if(type==3){
			shootObj.type=_ShootObjectType.Effect;
		//if(shootObj.type==_ShootObjectType.Effect){
			
			
			shootObj.pType=EditorGUILayout.IntPopup("EffectType: ", shootObj.pType, effectTypeLabel, effectTypeID);
			if(effectType==1){
				//if(shootObj.GetParticleSystem()==null) shootObj.GetParticleSystem();
			}
			else if(effectType==2){
				//if(shootObj.GetParticleEmitter()==null) shootObj.GetParticleEmitter();
				shootObj.oneShot = EditorGUILayout.Toggle("OneShot", shootObj.oneShot);
			}
		}
		
		EditorGUILayout.Space();
		
		
			shootObj.shootAudio=(AudioClip)EditorGUILayout.ObjectField("Shoot Sound: ", shootObj.shootAudio, typeof(AudioClip), false);
		
		if(type!=3){
			shootObj.hitAudio=(AudioClip)EditorGUILayout.ObjectField("Hit Sound: ", shootObj.hitAudio, typeof(AudioClip), false);
			EditorGUILayout.Space();
			
		
			shootObj.shootEffect=(GameObject)EditorGUILayout.ObjectField("Shoot Effect: ", shootObj.shootEffect, typeof(GameObject), false);
			shootObj.hitEffect=(GameObject)EditorGUILayout.ObjectField("hit Effect: ", shootObj.hitEffect, typeof(GameObject), false);
			EditorGUILayout.Space();
		}
		
		if(GUI.changed) EditorUtility.SetDirty(shootObj);
	}
}
