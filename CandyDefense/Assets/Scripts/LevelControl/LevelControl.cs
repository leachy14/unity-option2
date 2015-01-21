using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour
{
	public int wave;            //the wave we're on
	public bool roundIsOver;    //are we inbetween rounds?
	public int maxEnemies;        //max ammount of enemies on the map at any given time
	public int currentEnemies;  //amount of enemies currently on the map
	public float startTimer;    //countdown until timer ends
	public string round;
	public GameObject[] enemies;     //array of enemies
	public GameObject[] spawnPoints; //array of spawnpoints
	public GameObject enemy;
	public Vector2 scrollPosition = Vector2.zero;
	
	
	
	public void CountDown ()  //countsdown  1 every 1 second
	{
		
		if (startTimer != 0) {
			startTimer -= 1;
		}
		if (startTimer == 0) {
			CancelInvoke ();
				}
	}
	
	// Use this for initialization
	void Start ()
	{
		wave = 0;
		maxEnemies = 5;
		roundIsOver = true;
		startTimer = 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (startTimer > 0) {          //if we're either counting down or still waiting
			roundIsOver = true;    //round is still in over state
		} else {                         //otherwise
			roundIsOver = false;   //the round is over and we need to
			NextWave ();           //start the next wave
		}
		
		
		
		if (roundIsOver) {                                            //if the round is in over state
			if (Input.GetKeyDown ("space")) {                   //"press space to start the countdown timer
				if (startTimer > 0) {                        // but only if its not 0
					InvokeRepeating ("CountDown", 1, 1);
					
				}    
				
			}
		}     
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		round = "<color=#ff0000ff><size=30>Round: " + wave + "</size></color>";
	}
	
	
	void NextWave ()   //resets the spawn timer, adds 1 to the wave #, and then spawns based on wave
	{
		currentEnemies = 0;
		startTimer = 2;
		SetWave (wave + 1);
		maxEnemies ++;
		roundIsOver = false;
		Debug.Log ("spawning?");
		StartCoroutine (Spawn (0, 5));
		
	}
	
	void SetWave (int waveToSet) //sets the wave number
	{
		wave = waveToSet;
	}
	IEnumerator Spawn (int arrayIndex, int amount)  //spawns in specific spawnPoint
	{ //call with StartCoroutine (Spawn (enemy array index, number to spawn, spawnpoint array index));
		for (int i = 0; i <= amount; i++) {
			if (currentEnemies < maxEnemies) {
				Instantiate (enemy, transform.position, transform.rotation);
				yield return new WaitForSeconds (1);
				currentEnemies ++;
			}
			
		}
	}
	void OnGUI () {
		//guiText.fontSize = 30;
		scrollPosition = GUI.BeginScrollView(new Rect(10, 300, 100, 100), scrollPosition, new Rect(0, 0, 220, 200));
		GUI.Button(new Rect(0, 0, 100, 20), "Top-left");
		GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
		GUI.Button(new Rect(0, 180, 100, 20), "Bottom-left");
		GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");
		GUI.EndScrollView();
		GUI.Label (new Rect (10, 300, 130, 200), round);
		if(GUI.Button(new Rect(10, 30, 100, 50), "Start Round")) {
			if(roundIsOver){
				if (startTimer > 0) {                        // but only if its not 0
					InvokeRepeating ("CountDown", 1, 1);
					
				}
			}
		}
	}
}
