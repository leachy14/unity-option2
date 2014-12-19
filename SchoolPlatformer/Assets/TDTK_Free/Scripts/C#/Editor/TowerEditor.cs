using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;


public class TowerEditor : EditorWindow {
    
	int levelCap=1;
	
	[SerializeField] static string[] nameList=new string[0];
	
	[SerializeField] static UnitTower[] towerList=new UnitTower[0];
	
	
	UnitTower newTower;
	
	UnitTower tower;
	
	int index=0;
	int towerType=0;
	int towerTargetMode=0;
	//~ int towerTargetPriority=0;
	//~ int towerTargetArea=0;
	//~ int turretAnimateMode=0;
	//~ int turretRotationMode=0;
	
	static string[] towerTypeLabel=new string[3];
	static string[] towerTargetModeLabel=new string[3];
	static string[] towerTargetPriorityLabel=new string[4];
	static string[] towerTargetAreaLabel=new string[3];
	static string[] turretAnimateModeLabel=new string[3];
	static string[] turretRotationModeLabel=new string[2];
    
	private bool[] indicatorFlags=new bool[1];
	
	private bool showAnimationList=false;
	private string showAnimationText="Show animation configuration";
	
	private bool showSoundList=false;
	private string showSoundText="Show sfx list";
	
	static private TowerEditor window;
	
    // Add menu named "TowerEditor" to the Window menu
    //[MenuItem ("TDTK/TowerEditor")]
    public static void Init () {
        // Get existing open window or if none, make a new one:
        window = (TowerEditor)EditorWindow.GetWindow(typeof (TowerEditor));
		window.minSize=new Vector2(610, 650);
		
		window.indicatorFlags[0]=true;
		
		GetTower();
		
		towerTypeLabel[0]="Turret Tower";
		towerTypeLabel[1]="AOE Tower";
		towerTypeLabel[2]="Support Tower";
		
		towerTargetModeLabel[0]="Hybrid";
		towerTargetModeLabel[1]="Air";
		towerTargetModeLabel[2]="Ground";
		
		towerTargetPriorityLabel[0]="Nearest";
		towerTargetPriorityLabel[1]="Weakest";
		towerTargetPriorityLabel[2]="Toughest";
		towerTargetPriorityLabel[3]="Random";
		
		towerTargetAreaLabel[0]="AllAround";
		towerTargetAreaLabel[1]="DirectionalCone";
		towerTargetAreaLabel[2]="StraightLine";
		
		turretAnimateModeLabel[0]="Full";
		turretAnimateModeLabel[1]="Y-Axis Only";
		turretAnimateModeLabel[2]="None";
		
		turretRotationModeLabel[0]="FullTurret";
		turretRotationModeLabel[1]="SeparatedBarrel";
		
    }
    
	static BuildManager buildManager;
	
	static void GetTower(){
		towerList=new UnitTower[0];
		nameList=new string[0];
		
		buildManager=(BuildManager)FindObjectOfType(typeof(BuildManager));
		
		if(buildManager!=null){
			towerList=buildManager.towers;
			
			nameList=new string[towerList.Length];
			for(int i=0; i<towerList.Length; i++){
				nameList[i]=towerList[i].name;
			}
		}
		else{
			towerList=new UnitTower[0];
			nameList=new string[0];
		}
		
	}
	
	float startX, startY, height, spaceY, lW;
	int rscCount=1;
	
	private Vector2 scrollPos;
	
    void OnGUI () {
		scrollPos = GUI.BeginScrollView(new Rect(0, 0, window.position.width, window.position.height), scrollPos, new Rect(0, 0, Mathf.Max(window.position.width, 610+(levelCap-3)*180), 1270));
		
		GUI.changed = false;
		
		
		{
			startX=3;
			startY=3;
			height=18;
			spaceY=height+startX;
			
			lW=100;	//label width, the offset from label to the editable field
			
			if(towerList.Length>0) {
				index = EditorGUI.Popup(new Rect(startX, startY, 300, height), "Tower:", index, nameList);
				
				//new tower, update index
				levelCap=towerList[index].levelCap;			
				EditorGUI.LabelField(new Rect(320+startX, startY, 200, height), "LevelCap: "+towerList[index].levelCap.ToString());
				EditorGUI.LabelField(new Rect(395+startX, startY, 200, height), "Change to: ");
				levelCap = EditorGUI.IntField(new Rect(startX+465, startY, 20, height), levelCap);
				levelCap = Mathf.Max(1, levelCap);
				UpdateIndicatorFlags(levelCap);
				
				if(levelCap!=towerList[index].levelCap){
					towerList[index].levelCap=levelCap;
					towerList[index].UpdateTowerUpgradeStat(levelCap-1);
				}
				
				//assign appropriate towerType index
				if(towerList[index].type==_TowerType.TurretTower) towerType=0;
				else if(towerList[index].type==_TowerType.AOETower) towerType=1;
				//~ else if(towerList[index].type==_TowerType.DirectionalAOETower) towerType=2;
				else if(towerList[index].type==_TowerType.SupportTower) towerType=3;
				//~ else if(towerList[index].type==_TowerType.ResourceTower) towerType=4;
				//~ else if(towerList[index].type==_TowerType.Mine) towerType=5;
				//~ else if(towerList[index].type==_TowerType.Block) towerType=6;
				
				//assign appropriate towerTargetMode index
				if(towerList[index].targetMode==_TargetMode.Hybrid) towerTargetMode=0;
				else if(towerList[index].targetMode==_TargetMode.Air) towerTargetMode=1;
				else if(towerList[index].targetMode==_TargetMode.Ground) towerTargetMode=2;
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ if(towerList[index].animateTurret==_TurretAni.Full) turretAnimateMode=0;
					//~ if(towerList[index].animateTurret==_TurretAni.YAxis) turretAnimateMode=1;
					//~ if(towerList[index].animateTurret==_TurretAni.None) turretAnimateMode=2;
				//~ }
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ if(towerList[index].turretRotationModel==_RotationMode.FullTurret) turretRotationMode=0;
					//~ if(towerList[index].turretRotationModel==_RotationMode.SeparatedBarrel) turretRotationMode=1;
				//~ }
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ if(towerList[index].targetPriority==_TargetPriority.Nearest) towerTargetPriority=0;
					//~ if(towerList[index].targetPriority==_TargetPriority.Weakest) towerTargetPriority=1;
					//~ if(towerList[index].targetPriority==_TargetPriority.Toughest) towerTargetPriority=2;
					//~ if(towerList[index].targetPriority==_TargetPriority.Random) towerTargetPriority=3;
				//~ }
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ if(towerList[index].targetingArea==_TargetingArea.AllAround) towerTargetArea=0;
					//~ if(towerList[index].targetingArea==_TargetingArea.DirectionalCone) towerTargetArea=1;
					//~ if(towerList[index].targetingArea==_TargetingArea.StraightLine) towerTargetArea=2;
				//~ }
				
				if(GUI.Button(new Rect(Mathf.Max(startX+500, window.position.width-160), startY, 140, height), "Update Build Manager")){
					GetTower();
					GUI.EndScrollView();
					return;
				}
				
				towerList[index].unitName = EditorGUI.TextField(new Rect(startX, startY+=30, 300, height-3), "TowerName:", towerList[index].unitName);
				
				EditorGUI.LabelField(new Rect(startX+320, startY, 70, height), "Icon: ");
				towerList[index].icon=(Texture)EditorGUI.ObjectField(new Rect(startX+360, startY, 70, 70), towerList[index].icon, typeof(Texture), false);
						  
				towerType = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TowerType:", towerType, towerTypeLabel);
				if(towerType==0) towerList[index].type=_TowerType.TurretTower;
				else if(towerType==1) towerList[index].type=_TowerType.AOETower;
				//~ else if(towerType==2) towerList[index].type=_TowerType.DirectionalAOETower;
				else if(towerType==3) towerList[index].type=_TowerType.SupportTower;
				//~ else if(towerType==4) towerList[index].type=_TowerType.ResourceTower;
				//~ else if(towerType==5) towerList[index].type=_TowerType.Mine;
				//~ else if(towerType==6) towerList[index].type=_TowerType.Block;
				
				if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.AOETower){//|| towerList[index].type==_TowerType.DirectionalAOETower
					towerTargetMode = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TargetingMode:", towerTargetMode, towerTargetModeLabel);
					if(towerTargetMode==0) towerList[index].targetMode=_TargetMode.Hybrid;
					else if(towerTargetMode==1) towerList[index].targetMode=_TargetMode.Air;
					else if(towerTargetMode==2) towerList[index].targetMode=_TargetMode.Ground;
				}
				else startY+=20;

				//towerList[index].armorType=EditorGUI.IntField(new Rect(startX, startY+=20, 300, height-3), "ArmorType:", towerList[index].armorType);
				//if(towerList[index].type!=_TowerType.SupportTower && towerList[index].type!=_TowerType.ResourceTower && towerList[index].type!=_TowerType.Block){
				//	towerList[index].armorType=EditorGUI.IntField(new Rect(startX, startY+=20, 300, height-3), "DamageType:", towerList[index].damageType);
				//}
				//else startY+=20;
				
				//~ if(towerList[index].type==_TowerType.TurretTower){// || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ towerTargetArea = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TargetingArea:", towerTargetArea, towerTargetAreaLabel);
					//~ if(towerTargetArea==0) towerList[index].targetingArea=_TargetingArea.AllAround;
					//~ else if(towerTargetArea==1) towerList[index].targetingArea=_TargetingArea.DirectionalCone;
					//~ else if(towerTargetArea==2) towerList[index].targetingArea=_TargetingArea.StraightLine;
					
					//~ if(towerList[index].targetingArea!=_TargetingArea.AllAround){
						//~ towerList[index].matchTowerDir2TargetDir=EditorGUI.Toggle(new Rect(startX+205, startY+20-1, 300, height-3), towerList[index].matchTowerDir2TargetDir);
						//~ EditorGUI.LabelField(new Rect(startX+220, startY+20-1, 300, height-3), "FaceDirection");
						//~ towerList[index].targetingDirection = EditorGUI.FloatField(new Rect(startX, startY+=20, 195, height-3), "TargetingDirection:", towerList[index].targetingDirection);
					//~ }
					//~ else startY+=20;
				//~ }
				//~ else startY+=40;
				
				
				
				//~ if(towerList[index].targetingArea==_TargetingArea.DirectionalCone){
					//~ towerList[index].targetingFOV = EditorGUI.FloatField(new Rect(startX, startY+=20, 300, height-3), "TargetingFOV:", towerList[index].targetingFOV);
				//~ }
				//~ else startY+=20;
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ if(towerList[index].targetingArea!=_TargetingArea.StraightLine){
						//~ towerTargetPriority = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TargetingPriority:", towerTargetPriority, towerTargetPriorityLabel);
						//~ if(towerTargetPriority==0) towerList[index].targetPriority=_TargetPriority.Nearest;
						//~ else if(towerTargetPriority==1) towerList[index].targetPriority=_TargetPriority.Weakest;
						//~ else if(towerTargetPriority==2) towerList[index].targetPriority=_TargetPriority.Toughest;
						//~ else if(towerTargetPriority==2) towerList[index].targetPriority=_TargetPriority.Random;
					//~ }
					//~ //else startY+=20;
				//~ }
				//~ else startY+=20;
				
				//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
					//~ turretAnimateMode = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TurretAnimateMode:", turretAnimateMode, turretAnimateModeLabel);
					//~ if(turretAnimateMode==0) towerList[index].animateTurret=_TurretAni.Full;
					//~ else if(turretAnimateMode==1) towerList[index].animateTurret=_TurretAni.YAxis;
					//~ else if(turretAnimateMode==2) towerList[index].animateTurret=_TurretAni.None;
					
					//~ turretRotationMode = EditorGUI.Popup(new Rect(startX, startY+=20, 300, 15), "TurretRotationMode:", turretRotationMode, turretRotationModeLabel);
					//~ if(turretRotationMode==0) towerList[index].turretRotationModel=_RotationMode.FullTurret;
					//~ else if(turretRotationMode==1) towerList[index].turretRotationModel=_RotationMode.SeparatedBarrel;
				//~ }
				//~ else startY+=20;
				
				//~ if(towerList[index].type==_TowerType.DirectionalAOETower){
					//~ towerList[index].aoeConeAngle = EditorGUI.FloatField(new Rect(startX, startY+=20, 300, height-3), "AOE Cone Angle:", towerList[index].aoeConeAngle);
				//~ }
				
				
				//~ if(towerList[index].type==_TowerType.Mine){
					//~ towerList[index].mineOneOff=EditorGUI.Toggle(new Rect(startX, startY, 300, 15), "DestroyUponTriggered:", towerList[index].mineOneOff);
				//~ }
				
				startY+=5;
				towerList[index].buildingEffect=(GameObject)EditorGUI.ObjectField(new Rect(startX, startY+=20, 300, 15), "BuildingEffect: ", towerList[index].buildingEffect, typeof(GameObject), false);
				towerList[index].buildingDoneEffect=(GameObject)EditorGUI.ObjectField(new Rect(startX, startY+=20, 300, 15), "BuildingDoneEffect: ", towerList[index].buildingDoneEffect, typeof(GameObject), false);
				startY+=5;
				
				showAnimationList=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, 300, 15), showAnimationList, showAnimationText);
				if(showAnimationList){
					showAnimationText="Hide build animation list";
					towerList[index].turretBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - TurretAnimationComponent: ", towerList[index].turretBuildAnimationBody, typeof(Animation), false);
					towerList[index].turretBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - TurretAnimation: ", towerList[index].turretBuildAnimation, typeof(AnimationClip), false);
					towerList[index].baseBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BaseAnimationComponent: ", towerList[index].baseBuildAnimationBody, typeof(Animation), false);
					towerList[index].baseBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BaseAnimation: ", towerList[index].baseBuildAnimation, typeof(AnimationClip), false);
					//~ towerList[index].fireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBody: ", towerList[index].fireAnimationBody, typeof(Animation), false);
					//~ towerList[index].fireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimation: ", towerList[index].fireAnimation, typeof(AnimationClip), false);
					//~ towerList[index].fireAnimationBaseBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBaseBody: ", towerList[index].fireAnimationBaseBody, typeof(Animation), false);
					//~ towerList[index].fireAnimationBase=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBase: ", towerList[index].fireAnimationBase, typeof(AnimationClip), false);
				}
				else{
					showAnimationText="Show build animation list";
				}
				
				
				showSoundList=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, 300, 15), showSoundList, showSoundText);
				if(showSoundList){
					towerList[index].shootSound=(AudioClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - ShootSound: ", towerList[index].shootSound, typeof(AudioClip), false);
					towerList[index].buildingSound=(AudioClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BuildingSound: ", towerList[index].buildingSound, typeof(AudioClip), false);
					towerList[index].builtSound=(AudioClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BuiltSound: ", towerList[index].builtSound, typeof(AudioClip), false);
					towerList[index].soldSound=(AudioClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - SoldSound: ", towerList[index].soldSound, typeof(AudioClip), false);
					
					showSoundText="Hide sfx list";
				}
				else{
					showSoundText="Show sfx list";
				}
				
				
				EditorGUI.LabelField(new Rect(startX, startY+=25, 150, height), "Tower Description: ");
				towerList[index].description=EditorGUI.TextArea(new Rect(startX, startY+=17, 485, 50), towerList[index].description);
				startY+=25;
				
				
				//position in which the stat editor for tower levels start
				startY+=20;
				float tabYPos=startY;
				
				
				rscCount=1;
				
				//TowerStat section
				//if(index>=0 && index<towerList.Length){
					
					indicatorFlags[0] = EditorGUI.Toggle(new Rect(startX, startY+spaceY-10, 20, height), indicatorFlags[0]);
					startY+=10;
					
					if(indicatorFlags[0]){
						GUI.Box(new Rect(startX, startY+spaceY-1, 175, 465+(rscCount*20)), "");
						startX+=3;
						
						EditorGUI.LabelField(new Rect(50+startX, startY+=spaceY, 200, height), "Level 1: ");
						
						
						if(rscCount!=towerList[index].baseStat.costs.Length){
							UpdateBaseStatCost(index, rscCount);
						}
						
						if(towerList[index].baseStat.costs.Length==1){
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cost: ");
							towerList[index].baseStat.costs[0] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.costs[0]);
						}
						else{
							EditorGUI.LabelField(new Rect(startX, startY+=spaceY-5, 200, height), "Cost: ");
							for(int i=0; i<towerList[index].baseStat.costs.Length; i++){
								EditorGUI.LabelField(new Rect(startX, startY+spaceY-3, 200, height), " - resource: ");
								towerList[index].baseStat.costs[i] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY-3, 50, height-2), towerList[index].baseStat.costs[i]);
							}
							startY+=8;
						}
						
						EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BuildDuration: ");
						towerList[index].baseStat.buildDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.buildDuration);
						
						startY+=3;
						
						TypeDependentBaseStat(index);
						
						spaceY+=2;	startY+=8;
						
						//~ if(!(towerList[index].type==_TowerType.Mine || towerList[index].type==_TowerType.Block)){
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ShootObj: ");
							towerList[index].baseStat.shootObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].baseStat.shootObject, typeof(Transform), false);
							
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "TurretObj: ");
							towerList[index].baseStat.turretObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].baseStat.turretObject, typeof(Transform), false);
							
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BaseObj: ");
							towerList[index].baseStat.baseObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].baseStat.baseObject, typeof(Transform), false);
						
							//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
								//~ if(towerList[index].turretRotationModel==_RotationMode.SeparatedBarrel){
									//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BarrelObj: ");
									//~ towerList[index].baseStat.barrelObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].baseStat.barrelObject, typeof(Transform), false);
								//~ }
								//~ else startY+=spaceY;
							//~ }
							//~ else startY+=spaceY;
						//~ }
						
						//~ startY=870;
						startY+=10;
						startX-=3;
						
						//~ showAnimationList=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, 300, 15), showAnimationList, showAnimationText);
						//~ if(showAnimationList){
							//~ towerList[index].buildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BuildAnimationBody: ", towerList[index].buildAnimationBody, typeof(Animation), false);
							//~ towerList[index].buildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - BuildAnimation: ", towerList[index].buildAnimation, typeof(AnimationClip), false);
							//~ towerList[index].fireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBody: ", towerList[index].fireAnimationBody, typeof(Animation), false);
							//~ towerList[index].fireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimation: ", towerList[index].fireAnimation, typeof(AnimationClip), false);
							//~ towerList[index].fireAnimationBaseBody=(Animation)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBaseBody: ", towerList[index].fireAnimationBaseBody, typeof(Animation), false);
							//~ towerList[index].fireAnimationBase=(AnimationClip)EditorGUI.ObjectField(new Rect(startX, startY+=spaceY, 300, 17), " - FireAnimationBase: ", towerList[index].fireAnimationBase, typeof(AnimationClip), false);
							
							
							
							//~ showAnimationText="Hide animation list";
							
						//~ if(!(towerList[index].type==_TowerType.Mine || towerList[index].type==_TowerType.Block)){
							spaceY-=3;
							GUI.Box(new Rect(startX, startY+spaceY-1, 175, 133), "");
							startX+=3;
							
							EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "- Turret Fire Animation: ");
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
							towerList[index].baseStat.turretFireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.turretFireAnimation, typeof(AnimationClip), false);
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
							towerList[index].baseStat.turretFireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.turretFireAnimationBody, typeof(Animation), false);
							startY+=10;
							
							EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "- Base Fire Animation: ");
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
							towerList[index].baseStat.baseFireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.baseFireAnimation, typeof(AnimationClip), false);
							EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
							towerList[index].baseStat.baseFireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.baseFireAnimationBody, typeof(Animation), false);
							startY+=10;
							
							//~ EditorGUI.LabelField(new Rect(startX+18, startY+=spaceY, 200, height), "Turret Build Animation: ");
							//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
							//~ towerList[index].baseStat.turretBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.turretBuildAnimation, typeof(AnimationClip), false);
							//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
							//~ towerList[index].baseStat.turretBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.turretBuildAnimationBody, typeof(Animation), false);
							//~ startY+=10;
							
							//~ EditorGUI.LabelField(new Rect(startX+18, startY+=spaceY, 200, height), "Base Build Animation: ");
							//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
							//~ towerList[index].baseStat.baseBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.baseBuildAnimation, typeof(AnimationClip), false);
							//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
							//~ towerList[index].baseStat.baseBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].baseStat.baseBuildAnimationBody, typeof(Animation), false);
						//~ }	
							//EditorGUI.LabelField(new Rect(50+startX, startY+=spaceY, 200, height), "Level 1: ");
						//~ }
						//~ else{
							//~ showAnimationText="Show animation list";
						//~ }
						
						startX+=200;	startY=tabYPos;	spaceY=21;
						
					}
					else{
						EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "1");
						startX+=35;	startY=tabYPos;	
					}
					
					for(int i=0; i<towerList[index].upgradeStat.Length; i++){
						
						if(towerList[index]!=null && towerList[index].upgradeStat[i]!=null){
						
							indicatorFlags[i+1] = EditorGUI.Toggle(new Rect(startX, startY+spaceY-10, 20, height), indicatorFlags[i+1]);
							startY+=10;
							
							if(indicatorFlags[i+1]){
								GUI.Box(new Rect(startX, startY+spaceY-1, 175, 465+(rscCount*20)), "");
								startX+=3;
								
								EditorGUI.LabelField(new Rect(50+startX, startY+=spaceY, 200, height), "Level "+(i+2).ToString()+": ");
								
								//EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cost: ");
								//towerList[index].upgradeStat[i].cost = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[i].cost);
								
								if(rscCount!=towerList[index].upgradeStat[i].costs.Length){
									UpdateUpgradeStatCost(index, rscCount);
								}
								
								if(towerList[index].upgradeStat[i].costs.Length==1){
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cost: ");
									towerList[index].upgradeStat[i].costs[0] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[i].costs[0]);
								}
								else{
									EditorGUI.LabelField(new Rect(startX, startY+=spaceY-5, 200, height), "Cost: ");
									for(int j=0; j<towerList[index].upgradeStat[i].costs.Length; j++){
										EditorGUI.LabelField(new Rect(startX, startY+spaceY-3, 200, height), " - resource: ");
										towerList[index].upgradeStat[i].costs[j] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY-3, 50, height-2), towerList[index].upgradeStat[i].costs[j]);
									}
									startY+=8;
								}
								
								
								EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BuildDuration: ");
								towerList[index].upgradeStat[i].buildDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[i].buildDuration);
								startY+=3;
								
								TypeDependentUpgradeStat(index, i);
								
								spaceY+=2;	startY+=8;
								
								//~ if(!(towerList[index].type==_TowerType.Mine || towerList[index].type==_TowerType.Block)){
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ShootObj: ");
									towerList[index].upgradeStat[i].shootObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].upgradeStat[i].shootObject, typeof(Transform), false);
									
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "TurretObj: ");
									towerList[index].upgradeStat[i].turretObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].upgradeStat[i].turretObject, typeof(Transform), false);
									
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BaseObj: ");
									towerList[index].upgradeStat[i].baseObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].upgradeStat[i].baseObject, typeof(Transform), false);
									
									//~ if(towerList[index].type==_TowerType.TurretTower || towerList[index].type==_TowerType.DirectionalAOETower){
										//~ if(towerList[index].turretRotationModel==_RotationMode.SeparatedBarrel){
											//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "BarrelObj: ");
											//~ towerList[index].upgradeStat[i].barrelObject=(Transform)EditorGUI.ObjectField(new Rect(startX+lW-30, startY+=spaceY, 100, height-2), towerList[index].upgradeStat[i].barrelObject, typeof(Transform), false);
										//~ }
										//~ else startY+=spaceY;
									//~ }
									//~ else startY+=spaceY;
								//~ }
								
								
								//~ if(showAnimationList){
									//~ showAnimationText="Hide animation list";
									
								//~ if(!(towerList[index].type==_TowerType.Mine || towerList[index].type==_TowerType.Block)){
									spaceY-=3;
									startY+=10;//870+spaceY;
									startX-=3;
									
									//~ GUI.Box(new Rect(startX, startY+spaceY-1, 175, 275), "");
									GUI.Box(new Rect(startX, startY+spaceY-1, 175, 133), "");
									startX+=3;
									
									EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "- Turret Fire Animation: ");
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
									towerList[index].upgradeStat[i].turretFireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].turretFireAnimation, typeof(AnimationClip), false);
									//EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
									//towerList[index].upgradeStat[i].turretFireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].turretFireAnimationBody, typeof(Animation), false);
									startY+=10;
									
									EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "Base Fire Animation: ");
									EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
									towerList[index].upgradeStat[i].baseFireAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].baseFireAnimation, typeof(AnimationClip), false);
									//EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
									//towerList[index].upgradeStat[i].baseFireAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].baseFireAnimationBody, typeof(Animation), false);
									startY+=10;
									
									//~ EditorGUI.LabelField(new Rect(startX+18, startY+=spaceY, 200, height), "Turret Build Animation: ");
									//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
									//~ towerList[index].upgradeStat[i].turretBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].turretBuildAnimation, typeof(AnimationClip), false);
									//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
									//~ towerList[index].upgradeStat[i].turretBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].turretBuildAnimationBody, typeof(Animation), false);
									//~ startY+=10;
									
									//~ EditorGUI.LabelField(new Rect(startX+18, startY+=spaceY, 200, height), "Base Build Animation: ");
									//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Clip: ");
									//~ towerList[index].upgradeStat[i].baseBuildAnimation=(AnimationClip)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].baseBuildAnimation, typeof(AnimationClip), false);
									//~ EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Component: ");
									//~ towerList[index].upgradeStat[i].baseBuildAnimationBody=(Animation)EditorGUI.ObjectField(new Rect(startX+lW-25, startY+=spaceY, 95, height-2), towerList[index].upgradeStat[i].baseBuildAnimationBody, typeof(Animation), false);
								//~ }
									
									//EditorGUI.LabelField(new Rect(50+startX, startY+=spaceY, 200, height), "Level 1: ");
								//~ }
								
								startX+=190;
								startY=tabYPos;
								spaceY=21;
								
							}
							else{
								EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), (i+2).ToString());
								startX+=25;	startY=tabYPos;
							}
						}
					
					}
					
					
				//}
				
				HPAttributeEditor(index);
			}
			else{
				if(GUI.Button(new Rect(startX, startY, 140, height), "Find Build Manager")) GetTower();
				//return;
			}
			
			//~ if(towerList.Length>0) {
				
				
			//~ }
		}
	
		
		
		if(GUI.changed) EditorUtility.SetDirty(towerList[index]);
		
		GUI.EndScrollView();
    }
	
	
	
	
	
	
	
	void HPAttributeEditor(int index){
		float startY=133;
		float startX=320;
		
		float space=105;
		
		EditorGUI.LabelField(new Rect(startX, startY, 200, height), "Tower HP: ");
		towerList[index].HPAttribute.fullHP = EditorGUI.FloatField(new Rect(startX+space, startY, 50, height-2), towerList[index].HPAttribute.fullHP);
		
		//~ EditorGUI.LabelField(new Rect(startX, startY+20, 200, height), "Tower Shield: ");
		//~ towerList[index].HPAttribute.fullShield = EditorGUI.FloatField(new Rect(startX+space, startY+=20, 50, height-2), towerList[index].HPAttribute.fullShield);
		
		//~ if(towerList[index].HPAttribute.fullShield>0){
			//~ EditorGUI.LabelField(new Rect(startX, startY+20, 200, height), "Shield Recharge: ");
			//~ towerList[index].HPAttribute.shieldRechargeRate = EditorGUI.FloatField(new Rect(startX+space, startY+=20, 50, height-2), towerList[index].HPAttribute.shieldRechargeRate);
			
			//~ EditorGUI.LabelField(new Rect(startX, startY+20, 200, height), "Shield Stagger: ");
			//~ towerList[index].HPAttribute.shieldStagger = EditorGUI.FloatField(new Rect(startX+space, startY+=20, 50, height-2), towerList[index].HPAttribute.shieldStagger);
		//~ }
		
		startY=133;
		startX=320+105+50+10;
		space=100;
		
		EditorGUI.LabelField(new Rect(startX, startY, 200, height), "HP Overlay: ");
		towerList[index].HPAttribute.overlayHP=(Transform)EditorGUI.ObjectField(new Rect(startX+space, startY, 100, height-2), towerList[index].HPAttribute.overlayHP, typeof(Transform), false);
		
		//~ EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "Shield Overlay: ");
		//~ towerList[index].HPAttribute.overlayShield=(Transform)EditorGUI.ObjectField(new Rect(startX+space, startY, 100, height-2), towerList[index].HPAttribute.overlayShield, typeof(Transform), false);
		
		EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "Base Overlay: ");
		towerList[index].HPAttribute.overlayBase=(Transform)EditorGUI.ObjectField(new Rect(startX+space, startY, 100, height-2), towerList[index].HPAttribute.overlayBase, typeof(Transform), false);
		
		EditorGUI.LabelField(new Rect(startX, startY+=spaceY, 200, height), "Always Show Overlay: ");
		towerList[index].HPAttribute.alwaysShowOverlay= EditorGUI.Toggle(new Rect(startX+space+40, startY, 100, height-2), towerList[index].HPAttribute.alwaysShowOverlay);
	}
	
	
	
	
	
	void TypeDependentBaseStat(int index){
		//turretTower
		if(towerType==0){
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].baseStat.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].baseStat.damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ReloadDuration: ");
			towerList[index].baseStat.reloadDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.reloadDuration);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ClipSize: ");
			towerList[index].baseStat.clipSize = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.clipSize);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].baseStat.range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "AoeRadius: ");
			towerList[index].baseStat.aoeRadius = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.aoeRadius);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].baseStat.stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].baseStat.slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].baseStat.dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].baseStat.dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
			//startY+=5;
			
		}
		//DirectionalAOETower
		else if(towerType==1){
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].baseStat.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].baseStat.damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].baseStat.range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].baseStat.stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].baseStat.slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].baseStat.dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].baseStat.dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
		}
		//AOETower
		else if(towerType==2){
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].baseStat.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].baseStat.damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ReloadDuration: ");
			towerList[index].baseStat.reloadDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.reloadDuration);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ClipSize: ");
			towerList[index].baseStat.clipSize = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.clipSize);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].baseStat.range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].baseStat.stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].baseStat.slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].baseStat.dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].baseStat.dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
			//startY+=5;
			
		}
		//SupportTower
		else if(towerType==3){
			
			spaceY+=5;
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Effective Range: ");
			towerList[index].baseStat.range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.range);
			
			spaceY+=5;
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Buff Effect: ");
			spaceY-=5;
			startY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].baseStat.buff.damageBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.buff.damageBuff);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Cooldown: ");
			towerList[index].baseStat.buff.cooldownBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.buff.cooldownBuff);
			towerList[index].baseStat.buff.cooldownBuff = Mathf.Clamp(towerList[index].baseStat.buff.cooldownBuff, -0.8f, 0.8f);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Range: ");
			towerList[index].baseStat.buff.rangeBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.buff.rangeBuff);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- RegenHP: ");
			towerList[index].baseStat.buff.regenHP = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+3, 50, height-2), towerList[index].baseStat.buff.regenHP);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "Cooldown: ");
			towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].baseStat.cooldown);
			
			spaceY-=5;
		}
		//ResourceTower
		else if(towerType==4){
			
			//EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "IncomeValue: ");
			//towerList[index].baseStat.incomeValue = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].baseStat.incomeValue);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "Cooldown: ");
			towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].baseStat.cooldown);
			
			startY+=10;
			
			if(rscCount!=towerList[index].baseStat.incomes.Length){
				UpdateBaseStatIncomes(index, rscCount);
			}
			
			if(towerList[index].baseStat.incomes.Length==1){
				EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "resources:");
				towerList[index].baseStat.incomes[0] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.incomes[0]);
			}
			else{
				EditorGUI.LabelField(new Rect(startX, startY+=spaceY-5, 200, height), "Resources Per CD:");
				for(int i=0; i<towerList[index].baseStat.incomes.Length; i++){
					EditorGUI.LabelField(new Rect(startX, startY+spaceY-3, 200, height), " - resource: ");
					towerList[index].baseStat.incomes[i] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY-3, 50, height-2), towerList[index].baseStat.incomes[i]);
				}
				startY+=8;
			}
			
			
		}
		//mine
		else if(towerType==5){
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].baseStat.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].baseStat.damage);
			
			if(!towerList[index].mineOneOff){
				EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
				towerList[index].baseStat.cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.cooldown);
			}
				
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "EffectiveRange: ");
			towerList[index].baseStat.range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].baseStat.stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].baseStat.slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].baseStat.dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].baseStat.dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].baseStat.dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].baseStat.dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
		
			spaceY+=3;
		}
		
	}
	
	
	
	
	
	
	
	
	void TypeDependentUpgradeStat(int index, int lvl){
		if(towerType==0){
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].upgradeStat[lvl].damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].upgradeStat[lvl].damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].upgradeStat[lvl].cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ReloadDuration: ");
			towerList[index].upgradeStat[lvl].reloadDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].reloadDuration);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ClipSize: ");
			towerList[index].upgradeStat[lvl].clipSize = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].clipSize);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].upgradeStat[lvl].range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "AoeRadius: ");
			towerList[index].upgradeStat[lvl].aoeRadius = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].aoeRadius);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].upgradeStat[lvl].stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].upgradeStat[lvl].slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].upgradeStat[lvl].dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].upgradeStat[lvl].dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
			//startY+=5;
			
		}
		else if(towerType==1){
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].upgradeStat[lvl].damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].upgradeStat[lvl].damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].upgradeStat[lvl].cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].upgradeStat[lvl].range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].upgradeStat[lvl].stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].upgradeStat[lvl].slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].upgradeStat[lvl].dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].upgradeStat[lvl].dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.interval);
			
			float ttDmg=towerList[index].baseStat.dot.damage*towerList[index].baseStat.dot.duration/towerList[index].baseStat.dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
		}
		else if(towerType==2){
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+5, 200, height), "Damage: ");
			towerList[index].upgradeStat[lvl].damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+5, 50, height-2), towerList[index].upgradeStat[lvl].damage);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Cooldown: ");
			towerList[index].upgradeStat[lvl].cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].cooldown);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ReloadDuration: ");
			towerList[index].upgradeStat[lvl].reloadDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].reloadDuration);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "ClipSize: ");
			towerList[index].upgradeStat[lvl].clipSize = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].clipSize);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Range: ");
			towerList[index].upgradeStat[lvl].range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].range);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "StunDuration: ");
			towerList[index].upgradeStat[lvl].stunDuration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].stunDuration);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Slow Effect: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].slow.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- SlowFactor: ");
			towerList[index].upgradeStat[lvl].slow.slowFactor = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].slow.slowFactor);
			
			spaceY+=3;
			
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "DamageOverTime: ");
			spaceY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].upgradeStat[lvl].dot.damage = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.damage);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Duration: ");
			towerList[index].upgradeStat[lvl].dot.duration = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.duration);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Interval: ");
			towerList[index].upgradeStat[lvl].dot.interval = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].dot.interval);
			
			float ttDmg=towerList[index].upgradeStat[lvl].dot.damage*towerList[index].upgradeStat[lvl].dot.duration/towerList[index].upgradeStat[lvl].dot.interval;
			EditorGUI.LabelField(new Rect(startX+10, startY+=spaceY+3, 160, height), "TotalDamage:  "+ttDmg.ToString("f1"));
			
			spaceY+=3;
			//startY+=5;
			
		}
		else if(towerType==3){
			
			spaceY+=5;
			EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "Effective Range: ");
			towerList[index].upgradeStat[lvl].range = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].range);
			
			spaceY+=5;
			EditorGUI.LabelField(new Rect(startX, startY+=spaceY+3, 200, height), "Buff Effect: ");
			spaceY-=5;
			startY-=3;
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Damage: ");
			towerList[index].upgradeStat[lvl].buff.damageBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].buff.damageBuff);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Cooldown: ");
			towerList[index].upgradeStat[lvl].buff.cooldownBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].buff.cooldownBuff);
			towerList[index].upgradeStat[lvl].buff.cooldownBuff = Mathf.Clamp(towerList[index].upgradeStat[lvl].buff.cooldownBuff, -0.8f, 0.8f);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- Range: ");
			towerList[index].upgradeStat[lvl].buff.rangeBuff = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].buff.rangeBuff);
			
			EditorGUI.LabelField(new Rect(startX+10, startY+spaceY, 200, height), "- RegenHP: ");
			towerList[index].upgradeStat[lvl].buff.regenHP = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+3, 50, height-2), towerList[index].upgradeStat[lvl].buff.regenHP);
			
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "Cooldown: ");
			towerList[index].upgradeStat[lvl].cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].upgradeStat[lvl].cooldown);
		
			spaceY-=5;
		}
		else if(towerType==4){
			
			//EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "Income Value: ");
			//towerList[index].upgradeStat[lvl].incomeValue = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].upgradeStat[lvl].incomeValue);
			
			EditorGUI.LabelField(new Rect(startX, startY+spaceY+10, 200, height), "Cooldown: ");
			towerList[index].upgradeStat[lvl].cooldown = EditorGUI.FloatField(new Rect(startX+lW, startY+=spaceY+10, 50, height-2), towerList[index].upgradeStat[lvl].cooldown);
		
			startY+=10;
			
			if(rscCount!=towerList[index].upgradeStat[lvl].incomes.Length){
				UpdateUpgradeStatIncomes(index, rscCount);
			}
			
			if(towerList[index].upgradeStat[lvl].incomes.Length==1){
				EditorGUI.LabelField(new Rect(startX, startY+spaceY, 200, height), "resources:");
				towerList[index].upgradeStat[lvl].incomes[0] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY, 50, height-2), towerList[index].upgradeStat[lvl].incomes[0]);
			}
			else{
				EditorGUI.LabelField(new Rect(startX, startY+=spaceY-5, 200, height), "Resources Per CD:");
				for(int i=0; i<towerList[index].upgradeStat[lvl].incomes.Length; i++){
					EditorGUI.LabelField(new Rect(startX, startY+spaceY-3, 200, height), " - resource: ");
					towerList[index].upgradeStat[lvl].incomes[i] = EditorGUI.IntField(new Rect(startX+lW, startY+=spaceY-3, 50, height-2), towerList[index].upgradeStat[lvl].incomes[i]);
				}
				startY+=8;
			}
		
		}
		
	}
	
	void UpdateIndicatorFlags(int size){
		if(indicatorFlags.Length!=size){
			indicatorFlags=new bool[size];
			for(int i=0; i<indicatorFlags.Length; i++) indicatorFlags[i]=true;
		}
	}
	
	
	void UpdateBaseStatIncomes(int id, int length){
		int[] tempIncList=towerList[index].baseStat.incomes;
		
		towerList[index].baseStat.incomes=new int[length];
		
		for(int i=0; i<length; i++){
			if(i>=tempIncList.Length){
				towerList[index].baseStat.incomes[i]=0;
			}
			else{
				towerList[index].baseStat.incomes[i]=tempIncList[i];
			}
		}
	}
	
	void UpdateUpgradeStatIncomes(int id, int length){
		for(int j=0; j<towerList[index].upgradeStat.Length; j++){
			int[] tempIncList=towerList[index].upgradeStat[j].incomes;
			
			towerList[index].upgradeStat[j].incomes=new int[length];
			
			for(int i=0; i<length; i++){
				if(i>=tempIncList.Length){
					towerList[index].upgradeStat[j].incomes[i]=0;
				}
				else{
					towerList[index].upgradeStat[j].incomes[i]=tempIncList[i];
				}
			}
		}
	}
	
	void UpdateBaseStatCost(int id, int length){
		int[] tempCostList=towerList[index].baseStat.costs;
		
		towerList[index].baseStat.costs=new int[length];
		
		for(int i=0; i<length; i++){
			if(i>=tempCostList.Length){
				towerList[index].baseStat.costs[i]=0;
			}
			else{
				towerList[index].baseStat.costs[i]=tempCostList[i];
			}
		}
	}
	
	void UpdateUpgradeStatCost(int id, int length){
		for(int j=0; j<towerList[index].upgradeStat.Length; j++){
			int[] tempCostList=towerList[index].upgradeStat[j].costs;
			
			towerList[index].upgradeStat[j].costs=new int[length];
			
			for(int i=0; i<length; i++){
				if(i>=tempCostList.Length){
					towerList[index].upgradeStat[j].costs[i]=0;
				}
				else{
					towerList[index].upgradeStat[j].costs[i]=tempCostList[i];
				}
			}
		}
	}
	

}





