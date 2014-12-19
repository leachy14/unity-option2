using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class UI : MonoBehaviour {

	public enum _BuildMenuType{Fixed, Box, Pie}
	public _BuildMenuType buildMenuType;
	
	public enum _BuildMode{PointNBuild, DragNDrop}
	public _BuildMode buildMode;
	
	public bool showBuildSample=true;
	
	public int fastForwardSpeed=3;
	
	public bool enableTargetPrioritySwitch=true;
	
	public bool alwaysEnableNextButton=true;
	
	public string nextLevel="";
	public string mainMenu="";

	private bool enableSpawnButton=true;
	
	private bool buildMenu=false;
	
	private UnitTower currentSelectedTower;
	
	private int[] currentBuildList=new int[0];
	
	//indicate if the player have won or lost the game
	private bool winLostFlag=false;
	
	private bool paused=false;
	
	
	private static UI ui;
	void Awake(){
		ui=this;
	}
	
	private string gameMessage="";
	private float lastMsgTime=0;
	public static void ShowMessage(string msg){
		ui.gameMessage=ui.gameMessage+msg+"\n";
		ui.lastMsgTime=Time.time;
	}
	IEnumerator MessageCountDown(){
		while(true){
			if(gameMessage!=""){
				while(Time.time-lastMsgTime<3){
					yield return null;
				}
				gameMessage="";
			}
			yield return null;
		}
	}
	
	// Use this for initialization
	void Start () {		
		
		//init the rect to be used for drawing ui box
		topPanelRect=new Rect(-3, -3, Screen.width+6, 28);
		
		bottomPanelRect=new Rect(-3, Screen.height-25, Screen.width+6, 28);
		
		//init ui rect for DragNDrop mode, this will be use throughout the entire game
		if(buildMode==_BuildMode.DragNDrop){
			UnitTower[] fullTowerList=BuildManager.GetTowerList();
			int width=50;
			int height=50;
			
			int x=0;
			int y=Screen.height-height-6-(int)bottomPanelRect.height;
			int menuLength=(fullTowerList.Length)*(width+3);
			
			buildListRect=new Rect(x, y, menuLength+3, height+6);
		}
		
		//this two lines are now obsolete
		//initiate sample menu, so player can preview the tower in pointNBuild buildphase
		//if(buildMode==_BuildMode.PointNBuild && showBuildSample) BuildManager.InitiateSampleTower();
		
		StartCoroutine(MessageCountDown());
	}
	
	
	void OnEnable(){
		SpawnManager.onClearForSpawningE += OnClearForSpawning;
		GameControl.onGameOverE += OnGameOver;
		UnitTower.onDestroyE += OnTowerDestroy;
	}
	
	void OnDisable(){
		SpawnManager.onClearForSpawningE -= OnClearForSpawning;
		GameControl.onGameOverE -= OnGameOver;
		UnitTower.onDestroyE -= OnTowerDestroy;
	}
	
	//called when game is over, flag passed indicate user win or lost
	void OnGameOver(bool flag){
		winLostFlag=flag;
	}
	
	//called if a tower is unbuilt or destroyed, check to see if that tower is selected, if so, clear the selection
	void OnTowerDestroy(UnitTower tower){
		if(currentSelectedTower==tower) currentSelectedTower=null;
	}
	
	//caleed when SpawnManager clearFor Spawing event is detected, enable spawnButton
	void OnClearForSpawning(bool flag){
		enableSpawnButton=flag;
	}
	
	//call to enable/disable pause
	void TogglePause(){
		paused=!paused;
		if(paused){
			Time.timeScale=0;
			
			//close all the button, so user can interact with game when it's paused
			if(currentSelectedTower!=null){
				GameControl.ClearSelection();
				currentSelectedTower=null;
			}
			if(buildMenu){
				buildMenu=false;
				BuildManager.ClearBuildPoint();
			}
		}
		else Time.timeScale=1;
	}
	
	// Update is called once per frame
	void Update () {
		#if !UNITY_IPHONE && !UNITY_ANDROID
			if(buildMode==_BuildMode.PointNBuild)
				BuildManager.SetIndicator(Input.mousePosition);
		#endif
		
		if(Input.GetMouseButtonUp(0) && !IsCursorOnUI(Input.mousePosition) && !paused){
			
			//check if user click on towers
			UnitTower tower=GameControl.Select(Input.mousePosition);
			
			//if user click on tower, select the tower
			if(tower!=null){
				currentSelectedTower=tower;
				
				//if build mode is active, disable buildmode
				if(buildMenu){
					buildMenu=false;
					BuildManager.ClearBuildPoint();
					ClearBuildListRect();
				}
			}
			//no tower is selected
			else{
				//if a tower is selected previously, clear the selection
				if(currentSelectedTower!=null){
					GameControl.ClearSelection();
					currentSelectedTower=null;
					towerUIRect=new Rect(0, 0, 0, 0);
					//UIRect.RemoveRect(towerUIRect);
				}
				
				//if we are in PointNBuild Mode
				if(buildMode==_BuildMode.PointNBuild){
				
					//check for build point, if true initiate build menu
					if(BuildManager.CheckBuildPoint(Input.mousePosition)){
						UpdateBuildList();
						InitBuildListRect();
						buildMenu=true;
					}
					//if there are no valid build point but we are in build mode, disable it
					else{
						if(buildMenu){
							buildMenu=false;
							BuildManager.ClearBuildPoint();
							ClearBuildListRect();
						}
					}
					
				}
			}
			
		}
		//if right click, 
		else if(Input.GetMouseButtonUp(1)){
			//clear the menu
			if(buildMenu){
				buildMenu=false;
				BuildManager.ClearBuildPoint();
				ClearBuildListRect();
			}
			
			//if there are tower currently being selected
			if(currentSelectedTower!=null){
				CheckForTarget();
			}
		}
		
		//if escape key is pressed, toggle pause
		if(Input.GetKeyUp(KeyCode.Escape)){
			TogglePause();
		}
	}
	
	
	//check if user has click on creep, if yes and if current tower is eligible to attack it, set the assign it as tower target
	void CheckForTarget(){
		currentSelectedTower.CheckForTarget(Input.mousePosition);
		//~ Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//~ RaycastHit hit;
		//~ LayerMask mask=currentSelectedTower.GetTargetMask();
		//~ if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)){
			//~ Unit unit=hit.collider.gameObject.GetComponent<Unit>();
			//~ if(unit!=null){
				//~ currentSelectedTower.AssignTarget(unit);
			//~ }
		//~ }
	}
	
	
	private Rect topPanelRect=new Rect(-3, -3, Screen.width+6, 28);
	private Rect bottomPanelRect;
	private Rect buildListRect;
	private Rect towerUIRect;
	private Rect[] scatteredRectList=new Rect[0];	//for piemenu
	
	//check for all UI screen space, see if user cursor is within any of them, return true if yes
	//this is to prevent user from being able to interact with in game object even when clicking on UI panel and buttons
	public bool IsCursorOnUI(Vector3 point){
		Rect tempRect=new Rect(0, 0, 0, 0);
		
		tempRect=topPanelRect;
		tempRect.y=Screen.height-tempRect.y-tempRect.height;
		if(tempRect.Contains(point)) return true;
		
		tempRect=bottomPanelRect;
		tempRect.y=Screen.height-tempRect.y-tempRect.height;
		if(tempRect.Contains(point)) return true;
		
		tempRect=buildListRect;
		tempRect.y=Screen.height-tempRect.y-tempRect.height;
		if(tempRect.Contains(point)) return true;
		
		tempRect=towerUIRect;
		tempRect.y=Screen.height-tempRect.y-tempRect.height;
		if(tempRect.Contains(point)) return true;
		
		for(int i=0; i<scatteredRectList.Length; i++){
			tempRect=scatteredRectList[i];
			tempRect.y=Screen.height-tempRect.y-tempRect.height;
			if(tempRect.Contains(point)) return true;
		}
		
		return false;
	}
	
	//clear all ui space occupied by build menu
	void ClearBuildListRect(){
		if(buildMode==_BuildMode.PointNBuild){
			if(buildMenuType==_BuildMenuType.Pie) scatteredRectList=new Rect[0];
			else buildListRect=new Rect(0, 0, 0, 0);
		}
	}
	
	//initiate ui space that will be occupied by build menu
	void InitBuildListRect(){
		if(buildMode==_BuildMode.PointNBuild){
			if(buildMenuType==_BuildMenuType.Fixed){
				int width=50;
				int height=50;
						
				int x=0;
				int y=Screen.height-height-6-(int)bottomPanelRect.height;
						
				int menuLength=(currentBuildList.Length+1)*(width+3);
				
				//calculate the buildlist rect
				buildListRect=new Rect(x, y, menuLength+3, height+6);
			}
			else if(buildMenuType==_BuildMenuType.Box){
				//since this is a floating menu, the actual location cannot be pre-calculated
				//instead it will be calculated in every frame in OnGUI()
			}
			else if(buildMenuType==_BuildMenuType.Pie){
				//since this is a floating menu, the actual location cannot be pre-calculated
				//instead it will be calculated in every frame in OnGUI()
			}
		}
		else if(buildMode==_BuildMode.DragNDrop){
			//DragNDrop mode will always have build menu enable, so no need to calculate it
		}
	}
	
	//calculate the position occupied by each individual button based on number of button, position on screen and button size
	public Vector2[] GetPieMenuPos(int num, Vector3 pos, int size){
		//first off calculate the radius require to accomodate all buttons
		float radius=(num*size*1.8f)/(2*Mathf.PI);
		
		//create the rect array and the angle spacing required for the number of button
		Vector2[] piePos=new Vector2[num];
		float angle=200/(Mathf.Max(1, num-1));
		
		//loop through and calculate the button's position
		for(int i=0; i<num; i++){
			float x = pos.x+radius*Mathf.Sin((80)*Mathf.Deg2Rad+i*angle*Mathf.PI/180);
			float y = pos.y+radius*-Mathf.Cos((80)*Mathf.Deg2Rad+i*angle*Mathf.PI/180);
			
			piePos[i]=new Vector2(x, y);
		}
		
		return piePos;
	}
	
	
	//draw GUI
	public GUISkin skin;
	void OnGUI(){
		GUI.depth=100;
		
		GUI.skin=skin;
		
		//general infobox
		//draw top panel
		GUI.BeginGroup (topPanelRect);
			for(int i=0; i<2; i++) GUI.Box(new Rect(0, 0, topPanelRect.width, topPanelRect.height), "");
		
			int buttonX=8;
		
			//if not pause, draw the spawnButton and fastForwardButton
			if(!paused){
				//if SpawnManager is ready to spawn
				if(enableSpawnButton){
					if(GUI.Button(new Rect(buttonX, 5, 60, 20), "Spawn")){
						//if the game is not ended
						if(GameControl.gameState!=_GameState.Ended){
							//if spawn is successful, disable the spawnButton
							if(SpawnManager.Spawn())
								enableSpawnButton=false;
						}
					}
					buttonX+=65;
				}
				
				//display the fastforward button based on current time scale
				if(Time.timeScale==1){
					if(GUI.Button(new Rect(buttonX, 5, 60, 20), "Timex"+fastForwardSpeed.ToString())){
						Time.timeScale=fastForwardSpeed;
					}
				}
				else{
					if(GUI.Button(new Rect(buttonX, 5, 60, 20), "Timex1")){
						Time.timeScale=1;
					}
				}
			}
			//shift the cursor to where the next element will be drawn
			else buttonX+=65;
			buttonX+=130;
			
			//draw life and wave infomation
			GUI.Label(new Rect(200, 5, 100, 30), "Life: "+GameControl.GetPlayerLife());
			GUI.Label(new Rect(200+(topPanelRect.width-35-buttonX)/2, 5, 100, 30), "Wave: "+SpawnManager.GetCurrentWave()+"/"+SpawnManager.GetTotalWave());
			
			//pause button
			if(GUI.Button(new Rect(topPanelRect.width-30, 5, 25, 20), "II")) {
				TogglePause();
			}
		GUI.EndGroup ();
			
		//draw bottom panel
		GUI.BeginGroup (bottomPanelRect);
			for(int i=0; i<2; i++) GUI.Box(new Rect(0, 0, bottomPanelRect.width, bottomPanelRect.height), "");
		
			//get all the resource information
			int resource=GameControl.GetResourceVal();
			float subStartX=10; float subStartY=0;
			
			GUI.Label(new Rect(subStartX, subStartY+2, 250f, 25f), "Resource: "+resource.ToString());
			subStartX+=70;
		GUI.EndGroup ();
		
		
		//if game is still not over
		if(GameControl.gameState!=_GameState.Ended){
			
			//clear tooltip, each button when hovered will assign tooltip with corresponding ID
			//the tooltip will be checked later, if there's anything, we show the corresponding tooltip
			GUI.tooltip="";
			
			//build menu
			if(buildMode==_BuildMode.PointNBuild){
				//if PointNBuild mode, only show menu when there are active buildpoint (build menu on)
				if(buildMenu){
					if(buildMenuType==_BuildMenuType.Fixed) BuildMenuFix();
					else if(buildMenuType==_BuildMenuType.Box) BuildMenuBox();
					else if(buildMenuType==_BuildMenuType.Pie) BuildMenuPie();
				}
				//if there's no build menu, clear the buildList rect
				else buildListRect=new Rect(0, 0, 0, 0);
			}
			else if(buildMode==_BuildMode.DragNDrop){
				//if DragNDrop mode, show menu all time
				BuildMenuAllTowersFix();
			}
			
			//if the cursor is hover on top of tower button, the tooltip will bear the ID of the Buildable Tower
			if(GUI.tooltip!=""){
				int ID=int.Parse(GUI.tooltip);
				//show the build tooltip
				ShowToolTip(ID);
				//if no preview has been showing, preview the sample tower on the grid, only needed in PointNBuild mode
				if(buildMode==_BuildMode.PointNBuild && showBuildSample) BuildManager.ShowSampleTower(ID); 
			}
			else{
				//if a sample tower is shown on the grid, only needed in PointNBuild mode
				if(buildMode==_BuildMode.PointNBuild && showBuildSample) BuildManager.ClearSampleTower();
			}
			
			//selected tower information UI
			if(currentSelectedTower!=null){
				SelectedTowerUI();
			}
			//else towerUIRect=new Rect(0, 0, 0, 0);
			
			//if paused, draw the pause menu
			if(paused){
				float startX=Screen.width/2-100;
				float startY=Screen.height*0.35f;
				
				for(int i=0; i<4; i++) GUI.Box(new Rect(startX, startY, 200, 150), "Game Paused");
				
				startX+=50;
				
				if(GUI.Button(new Rect(startX, startY+=30, 100, 30), "Resume Game")){
					TogglePause();
				}
				if(GUI.Button(new Rect(startX, startY+=35, 100, 30), "Next Level")){
					Application.LoadLevel(Application.loadedLevelName);
				}
				if(GUI.Button(new Rect(startX, startY+=35, 100, 30), "Main Menu")){
					if(mainMenu!="") Application.LoadLevel(mainMenu);
				}
			}
			
		}
		//gameOver, draw the game over screen
		else{
			
			float startX=Screen.width/2-100;
			float startY=Screen.height*0.35f;
			
			string levelCompleteString="Level Complete";
			if(!winLostFlag) levelCompleteString="Level Lost";
		
			for(int i=0; i<4; i++) GUI.Box(new Rect(startX, startY, 200, 150), levelCompleteString);
			
			startX+=50;
			
			if(GUI.Button(new Rect(startX, startY+=30, 100, 30), "Restart Level")){
				Application.LoadLevel(Application.loadedLevelName);
			}
			if(alwaysEnableNextButton || winLostFlag){
				if(GUI.Button(new Rect(startX, startY+=35, 100, 30), "Next Level")){
					if(nextLevel!="") Application.LoadLevel(nextLevel);
				}
			}
			if(GUI.Button(new Rect(startX, startY+=35, 100, 30), "Main Menu")){
				if(mainMenu!="") Application.LoadLevel(mainMenu);
			}
		
		}
		
		
		//if game message is not empty, show the game message.
		//shift the text alignment to LowerRight first then back to UpperLeft after the message
		if(gameMessage!=""){
			GUI.skin.label.alignment=TextAnchor.LowerRight;
			GUI.Label(new Rect(0, 0, Screen.width-5, Screen.height+12), gameMessage);
			GUI.skin.label.alignment=TextAnchor.UpperLeft;
		}
		
	}
	
	//show tooptip when a build button is hovered
	void ShowToolTip(int ID){
		
		//get the tower component first
		UnitTower[] towerList=BuildManager.GetTowerList();
		UnitTower tower=towerList[ID];
		
		//create a new GUIStyle to highlight the tower name with different font size and color
		GUIStyle tempGUIStyle=new GUIStyle();
		
		tempGUIStyle.fontStyle=FontStyle.Bold;
		tempGUIStyle.fontSize=16;
		
		//create the GUIContent that shows the tower's name
		string towerName=tower.unitName;
		GUIContent guiContentTitle=new GUIContent(towerName); 
		
		//calculate the height required
		float heightT=tempGUIStyle.CalcHeight(guiContentTitle, 150);
		
		//switch to normal guiStyle and calculate the height needed to display cost
		tempGUIStyle.fontStyle=FontStyle.Normal;
		int[] cost=tower.GetCost();
		float heightC=0;	
		for(int i=0; i<cost.Length; i++){
			if(cost[i]>0){
				heightC+=25;
			}
		}
		
		//create a guiContent showing the tower description
		string towerDesp=tower.GetDescription();
		GUIContent guiContent=new GUIContent(""+towerDesp); 
		//calculate the height require to show the tower description
		float heightD= GUI.skin.GetStyle("Label").CalcHeight(guiContent, 150);
		
		//sum up all the height, so the tooltip box size can be known
		float height=heightT+heightC+heightD+10;
		
		//set the tooltip draw position
		int y=32;
		int x=5;
		//if this is a fixed or drag and drop mode, tooltip always appear at bottom left corner instaed of top left corner
		if(buildMenuType==_BuildMenuType.Fixed || buildMode==_BuildMode.DragNDrop){
			y=(int)(Screen.height-height-buildListRect.height-bottomPanelRect.height-2);
			x=(int)(Mathf.Floor(Input.mousePosition.x/50))*50;
		}

		//define the rect then draw the box
		Rect rect=new Rect(x, y, 160, height);
		for(int i=0; i<2; i++) GUI.Box(rect, "");
		
		//display all the guiContent assigned ealier
		GUI.BeginGroup(rect);
			//show tower name, format it to different text style, size and color
			tempGUIStyle.fontStyle=FontStyle.Bold;
			tempGUIStyle.fontSize=16;
			tempGUIStyle.normal.textColor=new Color(1, 1, 0, 1f);
			GUI.Label(new Rect(5, 5, 150, heightT), guiContentTitle, tempGUIStyle);
		
			//show tower's cost
			//get the resourceList from GameControl so we have all the information
			int[] rsc=tower.GetCost();
			GUI.Label(new Rect(5, 5+heightT, 150, 25), " - "+rsc[0].ToString()+"resource");
			
			//show desciption
			GUI.Label(new Rect(5, 5+heightC+heightT, 150, heightD), guiContent);
		GUI.EndGroup();
	}
	
	
	
	//for drag and drop, show all available tower
	void BuildMenuAllTowersFix(){
		UnitTower[] towerList=BuildManager.GetTowerList();
		
		int width=50;
		int height=50;
		
		int x=0;
		int y=Screen.height-height-6-(int)bottomPanelRect.height;
		
		for(int i=0; i<3; i++) GUI.Box(buildListRect, "");
		
		x+=3;	y+=3;
		
		for(int i=0; i<towerList.Length; i++){
			UnitTower tower=towerList[i];
				
			GUIContent guiContent=new GUIContent(tower.icon, i.ToString()); 
			if(GUI.Button(new Rect(x, y, width, height), guiContent)){
				BuildManager.BuildTowerDragNDrop(tower);
			}
			
			x+=width+3;
		}
	}
	
	
	void BuildMenuPie(){
		BuildableInfo currentBuildInfo=BuildManager.GetBuildInfo();
				
		//calculate the position in which the build list ui will be appear at
		Vector3 screenPos = Camera.main.WorldToScreenPoint(currentBuildInfo.position);
		
		int width=50;
		int height=50;
		
		Vector2[] piePos=GetPieMenuPos(currentBuildList.Length, screenPos, (int)1.414f*(width+height)/2);
		
		scatteredRectList=new Rect[currentBuildList.Length+1];

		//show up the build buttons, scrolling through currentBuildList initiated whevenever the menu is first brought up
		UnitTower[] towerList=BuildManager.GetTowerList();
			
		for(int num=0; num<currentBuildList.Length; num++){
			int ID=currentBuildList[num];
			
			if(ID>=0){
				UnitTower tower=towerList[ID];
				Vector2 point=piePos[num];
				
				scatteredRectList[num]=new Rect(point.x-width/2, Screen.height-point.y-height*0.75f, width, height);
				
				GUIContent guiContent=new GUIContent(tower.icon, ID.ToString()); 
				if(GUI.Button(scatteredRectList[num], guiContent)){
					//if building was successful, break the loop can close the panel
					if(BuildButtonPressed(tower)) return;
				}
				
			}
		}
			
		//clear buildmode button
		scatteredRectList[currentBuildList.Length]=new Rect(screenPos.x-width/2, Screen.height-screenPos.y+height*0.5f, width, height);
		
		if(GUI.Button(scatteredRectList[currentBuildList.Length], "X")){
			buildMenu=false;
			BuildManager.ClearBuildPoint();
			ClearBuildListRect();
		}
	}
	
	
	void BuildMenuBox(){
		BuildableInfo currentBuildInfo=BuildManager.GetBuildInfo();
				
		//calculate the position in which the build list ui will be appear at
		Vector3 screenPos = Camera.main.WorldToScreenPoint(currentBuildInfo.position);
		
		int width=50;
		int height=50;
				
		int x=(int)screenPos.x-width;
		x=Mathf.Clamp(x, 0, Screen.width-width*2);
				
		int menuLength=((int)Mathf.Floor((currentBuildList.Length+2)/2))*(height+3);
		int y=Screen.height-(int)screenPos.y;	//invert the height
		y-=menuLength/2-3;
		y=Mathf.Clamp(y, 29, Screen.height-menuLength-(int)bottomPanelRect.height);
		
		//calculate the buildlist rect
		buildListRect=new Rect(x-3, y-3, width*2+6+3, menuLength+4);
		for(int i=0; i<3; i++) GUI.Box(buildListRect, "");
		
		//show up the build buttons, scrolling through currentBuildList initiated whevenever the menu is first brought up
		UnitTower[] towerList=BuildManager.GetTowerList();
			
		for(int num=0; num<currentBuildList.Length; num++){
			int ID=currentBuildList[num];
			
			if(ID>=0){
				UnitTower tower=towerList[ID];
				
				GUIContent guiContent=new GUIContent(tower.icon, ID.ToString()); 
				if(GUI.Button(new Rect(x, y, width, height), guiContent)){
					//if building was successful, break the loop can close the panel
					if(BuildButtonPressed(tower)) return;
				}
				
				if(num%2==1){
					x-=width+3;
					y+=height+3;
				}
				else x+=width+3;
			}
		}
			
		//clear buildmode button
		if(GUI.Button(new Rect(x, y, width, height), "X")){
			buildMenu=false;
			BuildManager.ClearBuildPoint();
			ClearBuildListRect();
		}
	}
	
	
	void BuildMenuFix(){
		
		int width=50;
		int height=50;
				
		int x=0;
		int y=Screen.height-height-6-(int)bottomPanelRect.height;
		
		for(int i=0; i<3; i++) GUI.Box(buildListRect, "");
		
		//show up the build buttons, scrolling through currentBuildList initiated whevenever the menu is first brought up
		UnitTower[] towerList=BuildManager.GetTowerList();
		
		x+=3;	y+=3;
			
		for(int num=0; num<currentBuildList.Length; num++){
			int ID=currentBuildList[num];
			
			if(ID>=0){
				UnitTower tower=towerList[ID];
				
				GUIContent guiContent=new GUIContent(tower.icon, ID.ToString()); 
				if(GUI.Button(new Rect(x, y, width, height), guiContent)){
					//if building was successful, break the loop can close the panel
					if(BuildButtonPressed(tower)) return;
				}
				
				else x+=width+3;
			}
		}
			
		//clear buildmode button
		if(GUI.Button(new Rect(x, y, width, height), "X")){
			buildMenu=false;
			BuildManager.ClearBuildPoint();
			ClearBuildListRect();
		}
	}
	
	private bool BuildButtonPressed(UnitTower tower){
		if(BuildManager.BuildTowerPointNBuild(tower)==""){
			//built, clear the build menu flag
			buildMenu=false;
			ClearBuildListRect();
			return true;
		}	
		else{
			//Debug.Log("build failed. invalide position");
			return false;
		}
	}
	
	
	//show to draw the selected tower info panel, which include the upgrade and sell button
	void SelectedTowerUI(){
		
		float startX=Screen.width-260;
		float startY=Screen.height-455-bottomPanelRect.height;
		float widthBox=250;
		float heightBox=450;
		
		towerUIRect=new Rect(startX, startY, widthBox, heightBox);
		for(int i=0; i<3; i++) GUI.Box(towerUIRect, "");
		
		startX=Screen.width-260+20;
		startY=Screen.height-455-bottomPanelRect.height+20;
		
		float width=250-40;
		float height=20;
		
		GUIStyle tempGUIStyle=new GUIStyle();
		
		tempGUIStyle.fontStyle=FontStyle.Bold;
		tempGUIStyle.fontSize=16;
		tempGUIStyle.normal.textColor=new Color(1, 1, 0, 1f);
		
		string towerName=currentSelectedTower.unitName;
		GUIContent guiContentTitle=new GUIContent(towerName); 
		
		GUI.Label(new Rect(startX, startY, width, height), guiContentTitle, tempGUIStyle);
		
		tempGUIStyle.fontStyle=FontStyle.Bold;
		tempGUIStyle.fontSize=13;
		tempGUIStyle.normal.textColor=new Color(1, 0, 0, 1f);
		
		GUI.Label(new Rect(startX, startY+=height, width, height), "Level: "+currentSelectedTower.GetLevel().ToString(), tempGUIStyle);
		
		startY+=20;
		
		
		string towerInfo="";
		
		_TowerType type=currentSelectedTower.type;
		
		//display relevent information based on tower type
		if(type==_TowerType.SupportTower){
			
			//show buff info only
			BuffStat buffInfo=currentSelectedTower.GetBuff();
			
			string buff="";
			if(buffInfo.damageBuff!=0) buff+="Buff damage by "+(buffInfo.damageBuff*100).ToString("f1")+"%\n";
			if(buffInfo.rangeBuff!=0) buff+="Buff range by "+(buffInfo.rangeBuff*100).ToString("f1")+"%\n";
			if(buffInfo.cooldownBuff!=0) buff+="Reduce CD by "+(buffInfo.cooldownBuff*100).ToString("f1")+"%\n";
			if(buffInfo.regenHP!=0) buff+="Renegerate HP by "+(buffInfo.regenHP).ToString("f1")+" per seconds\n";
			
			towerInfo+=buff;
			
		}
		else if(type==_TowerType.TurretTower || type==_TowerType.AOETower){
			
			//show the basic info for turret and aoeTower
			if(currentSelectedTower.GetDamage()>0)
				towerInfo+="Damage: "+currentSelectedTower.GetDamage().ToString("f1")+"\n";
			if(currentSelectedTower.GetCooldown()>0)
				towerInfo+="Cooldown: "+currentSelectedTower.GetCooldown().ToString("f1")+"sec\n";
			if(type==_TowerType.TurretTower && currentSelectedTower.GetAoeRadius()>0)
				towerInfo+="AOE Radius: "+currentSelectedTower.GetAoeRadius().ToString("f1")+"\n";
			if(currentSelectedTower.GetStunDuration()>0)
				towerInfo+="Stun target for "+currentSelectedTower.GetStunDuration().ToString("f1")+"sec\n";
			
			//if the tower have damage over time value, display it
			Dot dot=currentSelectedTower.GetDot();
			float totalDot=dot.damage*(dot.duration/dot.interval);
			if(totalDot>0){
				string dotInfo="Cause "+totalDot.ToString("f1")+" damage over the next "+dot.duration+" sec\n";
				
				towerInfo+=dotInfo;
			}
			
			//if the tower have slow value, display it
			Slow slow=currentSelectedTower.GetSlow();
			if(slow.duration>0){
				string slowInfo="Slow target by "+(slow.slowFactor*100).ToString("f1")+"% for "+slow.duration.ToString("f1")+"sec\n";
				towerInfo+=slowInfo;
			}
		}
		
		
		//show the tower's description
		towerInfo+="\n\n"+currentSelectedTower.description;
		
		GUIContent towerInfoContent=new GUIContent(towerInfo);
			
		//draw all the information on screen
		float contentHeight= GUI.skin.GetStyle("Label").CalcHeight(towerInfoContent, 200);
		GUI.Label(new Rect(startX, startY+=20, width, contentHeight), towerInfoContent);
	
		
		//reset the draw position
		startY=Screen.height-180-bottomPanelRect.height;
		if(enableTargetPrioritySwitch){
			if(currentSelectedTower.type==_TowerType.TurretTower){
				//~ if(currentSelectedTower.targetingArea!=_TargetingArea.StraightLine){
					GUI.Label(new Rect(startX, startY, 120, 30), "Targeting Priority:");
					if(GUI.Button(new Rect(startX+120, startY-3, 100, 30), currentSelectedTower.targetPriority.ToString())){
						if(currentSelectedTower.targetPriority==_TargetPriority.Nearest) currentSelectedTower.SetTargetPriority(1);
						else if(currentSelectedTower.targetPriority==_TargetPriority.Weakest) currentSelectedTower.SetTargetPriority(2);
						else if(currentSelectedTower.targetPriority==_TargetPriority.Toughest) currentSelectedTower.SetTargetPriority(3);
						else if(currentSelectedTower.targetPriority==_TargetPriority.Random) currentSelectedTower.SetTargetPriority(0);
					}
					startY+=30;
				//~ }
			}
		}
		

		//check if the tower can be upgrade
		bool upgradable=false;
		if(!currentSelectedTower.IsLevelCapped() && currentSelectedTower.IsBuilt()){
			upgradable=true;
		}
		
		//reset the draw position
		startY=Screen.height-50-bottomPanelRect.height;
		
		//if the tower is eligible to upgrade, draw the upgrade button
		if(upgradable){
			if(GUI.Button(new Rect(startX, startY, 100, 30), new GUIContent("Upgrade", "1"))){
				//upgrade the tower, if false is return, player have insufficient fund
				if(!currentSelectedTower.Upgrade()) Debug.Log("Insufficient Resource");
			}
		}
		//sell button
		if(currentSelectedTower.IsBuilt()){
			if(GUI.Button(new Rect(startX+110, startY, 100, 30), new GUIContent("Sell", "2"))){
				currentSelectedTower.Sell();
			}
		}
		
		//if the cursor is hover on the upgrade button, show the cost
		if(GUI.tooltip=="1"){
			
			int[] cost=currentSelectedTower.GetCost();
			GUI.Label(new Rect(startX+10, startY, 150, 25), " - "+cost[0].ToString()+"resource");
			
		}
		//if the cursor is hover on the sell button, show the resource gain
		else if(GUI.tooltip=="2"){
			int[] sellValue=currentSelectedTower.GetTowerSellValue();
			GUI.Label(new Rect(startX+120, startY, 150, 25), " + "+sellValue[0].ToString()+"resource");
		}
	}
	
	
	//called whevenever the build list is called up
	//compute the number of tower that can be build in this build pointer
	//store the tower that can be build in an array of number that reference to the towerlist
	//this is so these dont need to be calculated in every frame in OnGUI()
	void UpdateBuildList(){
		
		//get the current buildinfo in buildmanager
		BuildableInfo currentBuildInfo=BuildManager.GetBuildInfo();
		
		//get the current tower list in buildmanager
		UnitTower[] towerList=BuildManager.GetTowerList();
		
		//construct a temporary interger array the length of the buildinfo
		int[] tempBuildList=new int[towerList.Length];
		//for(int i=0; i<currentBuildList.Length; i++) tempBuildList[i]=-1;
		
		//scan through the towerlist, if the tower matched the build type, 
		//put the tower ID in the towerlist into the interger array
		int count=0;	//a number to record how many towers that can be build
		for(int i=0; i<towerList.Length; i++){
			UnitTower tower=towerList[i];
				
			if(currentBuildInfo.specialBuildableID!=null && currentBuildInfo.specialBuildableID.Length>0){
				foreach(int specialBuildableID in currentBuildInfo.specialBuildableID){
					if(specialBuildableID==tower.specialID){
						count+=1;
						break;
					}
				}
			}
			else{
				if(tower.specialID<0){
					//check if this type of tower can be build on this platform
					foreach(_TowerType type in currentBuildInfo.buildableType){
						if(tower.type==type && tower.specialID<0){
							tempBuildList[count]=i;
							count+=1;
							break;
						}
					}
				}
			}
		}
		
		//for as long as the number that can be build, copy from the temp buildList to the real buildList
		currentBuildList=new int[count];
		for(int i=0; i<currentBuildList.Length; i++) currentBuildList[i]=tempBuildList[i];
	}
	
	void OnDrawGizmos(){
		if(buildMenu){
			//BuildableInfo currentBuildInfo=BuildManager.GetBuildInfo();
			//float gridSize=BuildManager.GetGridSize();
			//Gizmos.DrawCube(currentBuildInfo.position, new Vector3(gridSize, 0, gridSize));
		}
	}
	
	
}
