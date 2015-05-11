using UnityEngine;
using System.Collections;
using Store;
using UnityEngine.UI;

namespace Level
{
		public class LevelControl : MonoBehaviour
		{

				//Strings
				public string round;
	
				//Textures


	
				//Game Objects	
				public GameObject[] enemies;     //array of enemies
				public GameObject[] spawnPoints; //array of spawnpoints
				public GameObject[] Turrets;
				public GameObject[] Bars;
				public GameObject[] Projectiles;
				public GameObject[] FireShots;
				public GameObject[] Bombs;
				public GameObject[] Ices;
				public GameObject Raptor;
				public GameObject SanicRaptor;
				public GameObject DinoFuck;
				public GameObject Bar;
				public Toggle SpeedTog;
				public GameObject Pause;
	
				//Bools	
				public bool roundIsOver;    //are we inbetween rounds?
				public bool pause = false;
				public bool Called = true;

				//integers
				public int wave;            //the wave we're on
				public int RoundHeight;
				public int maxEnemies;        //max ammount of enemies on the map at any given time
				public int currentEnemies;  //amount of enemies currently on the map
				public int pauseX;
				public int pauseY;
				public int current_enemy_amount;
				public int Tutorial;

				//floats
				public float hSliderValue = 5.0F;
				public float startTimer;    //countdown until timer ends	
				public float SpawnRate;
				public float lives;
				public Image speedon;
	
	
				//Access other scripts
				public GameObject store_accessor;
				public StoreControl storecontrol;
	
				public void CountDown ()  //countsdown  1 every 1 second
				{
						if (startTimer != 0) {
								startTimer -= 1;
						}
						if (startTimer == 0) {
								NextWave (); 
								CancelInvoke ();
						}
				}
	
				// Use this for initialization
				void Start ()
				{
						wave = 0;
						maxEnemies = 1;
						roundIsOver = true;
						startTimer = 2;
						current_enemy_amount = 0;
						Tutorial = 1;
						lives = 20;
						store_accessor = GameObject.Find ("Store");
						storecontrol = store_accessor.GetComponent <StoreControl> ();
						SpawnRate = 1F;
						currentEnemies = 1;
				}
	
				// Update is called once per frame
				void Update ()
				{
						if (startTimer > 0 && currentEnemies == maxEnemies && enemies.Length == 0) {          //if we're either counting down or still waiting
								if (Called == false) {
										roundEnd ();
								}				
								roundIsOver = true; 		//round is still in over state

						} else {                         //otherwise
								//the round is over and we need to
								//start the next wave
								roundIsOver = false;
						}
	
						if (roundIsOver == true) {                                            //if the round is in over state
								if (Input.GetKeyDown ("space")) {                   //"press space to start the countdown timer
										if (startTimer > 0) {                        // but only if its not 0
												InvokeRepeating ("CountDown", 1, 1);
										}    
								}

								//current_enemy_amount = enemies.Length;
						}     
						
						round = "<color=#ff0000ff><size=30>Round: " + wave + "</size></color>";
						RoundHeight = (Screen.height - (Screen.height - 75));
						current_enemy_amount = enemies.Length;
						
						

						if (Input.GetKeyDown ("escape") && pause == false) {
							
								Instantiate (Pause, transform.position, transform.rotation);	
								pause = true;
								foreach (GameObject objs in Turrets) {
										objs.SetActive (false);
								}
								foreach (GameObject Enem in enemies) {
										Enem.SetActive (false);
								}
								foreach (GameObject bArs in Bars) {
										bArs.SetActive (false);
								}
								foreach (GameObject bOmb in Bombs) {
										bOmb.SetActive (false);
								}
								foreach (GameObject ProJ in Projectiles) {
										ProJ.SetActive (false);
								}
								foreach (GameObject fIre in FireShots) {
										fIre.SetActive (false);
								}
								foreach (GameObject iCes in Ices) {
										iCes.SetActive (false);
								}
						} else if (Input.GetKeyDown ("escape") && pause == true) {
								pause = false;
								foreach (GameObject objs in Turrets) {
										objs.SetActive (true);
								}
								foreach (GameObject Enem in enemies) {
										Enem.SetActive (true);
								}
								foreach (GameObject bArs in Bars) {
										bArs.SetActive (true);
								}
								foreach (GameObject bOmb in Bombs) {
										bOmb.SetActive (true);
								}
								foreach (GameObject ProJ in Projectiles) {
										ProJ.SetActive (true);
								}
								foreach (GameObject fIre in FireShots) {
										fIre.SetActive (true);
								}
								foreach (GameObject iCes in Ices) {
										iCes.SetActive (true);
								}
								GameObject PauseScreen = GameObject.Find ("Pause Screen(Clone)");
								Destroy (PauseScreen);
						}
						if (pause == false) {
								enemies = GameObject.FindGameObjectsWithTag ("Enemy");
								Turrets = GameObject.FindGameObjectsWithTag ("Turret");
								Bars = GameObject.FindGameObjectsWithTag ("Bar");
								FireShots = GameObject.FindGameObjectsWithTag ("Fire");
								Projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
								Ices = GameObject.FindGameObjectsWithTag ("Ice");
								Bombs = GameObject.FindGameObjectsWithTag ("Bomb");
						}

						pauseY = (Screen.height / 2 - 100);
						pauseX = (Screen.width / 2 - 200);

						
						if (lives <= 0) {
								Application.LoadLevel ("Forest");
						}
						if (SpeedTog.isOn == true) {
								Time.timeScale = 2f;
						} else if (SpeedTog.isOn == false) {
								Time.timeScale = 1f;			
						}
				}

				void NextWave ()   //resets the spawn timer, adds 1 to the wave #, and then spawns based on wave
				{
		
						startTimer = 1;
						currentEnemies = 0;
						SetWave (wave + 1);
				
						if (wave <= 3) {
								maxEnemies ++;
						} else if (wave >= 4 && wave <= 24) {
		 
								maxEnemies = Mathf.RoundToInt (maxEnemies * 1.25f);

						} else if (wave >= 25 && wave <= 50) {

								maxEnemies = Mathf.RoundToInt (maxEnemies * 1.5f);
						} else {
								maxEnemies = 1000;
						}
						roundIsOver = false;
						StartCoroutine (Spawn (0, maxEnemies));
						if (SpawnRate > 0.06f) {
								SpawnRate = SpawnRate + -0.05F;
						}	
				}
	
				void SetWave (int waveToSet) //sets the wave number
				{
						wave = waveToSet;
				}

				IEnumerator Spawn (int arrayIndex, int amount)  //spawns in specific spawnPoint
				{ //call with StartCoroutine (Spawn (enemy array index, number to spawn, spawnpoint array index));
						for (int i = 0; i <= amount; i++) {
								if (pause == false) {
										if (currentEnemies < maxEnemies) {
												Instantiate (Raptor, transform.position, transform.rotation);
												yield return new WaitForSeconds (0.05f);
												Instantiate (Bar, transform.position, transform.rotation);
												yield return new WaitForSeconds (SpawnRate);
												currentEnemies ++;
												if (wave >= 4 && currentEnemies < maxEnemies) {
														Instantiate (SanicRaptor, transform.position, transform.rotation);
														yield return new WaitForSeconds (0.05f);
														Instantiate (Bar, transform.position, transform.rotation);
														yield return new WaitForSeconds (SpawnRate / 2);
														currentEnemies ++;
												}
												if (wave >= 7 && currentEnemies < maxEnemies) {
														Instantiate (DinoFuck, transform.position, transform.rotation);
														yield return new WaitForSeconds (0.05f);
														Instantiate (Bar, transform.position, transform.rotation);
														yield return new WaitForSeconds (SpawnRate / 2);
														currentEnemies ++;
												}
										}

								}
						}
						Called = false;
				}

				/*void OnGUI ()
				{

						if (pause == true) {
								GUI.BeginGroup (new Rect (pauseX, pauseY, 800, 600));
								GUI.Box (new Rect (0, 0, 400, 200), "This is a title");
								if (GUI.Button (new Rect (10, 50, 100, 50), "Quit Game")) {
										Application.Quit ();
								}
								GUI.EndGroup ();
						}
	
				}*/

				public void StartRound ()
				{
						if (roundIsOver == true) {
								NextWave ();
						}
				}

				void roundEnd ()
				{
						storecontrol.Coins = storecontrol.Coins + 100;
						Called = true;
				}

				public void PauseOn ()
				{
						Instantiate (Pause, transform.position, transform.rotation);
						pause = true;
						foreach (GameObject objs in Turrets) {
								objs.SetActive (false);
						}
						foreach (GameObject Enem in enemies) {
								Enem.SetActive (false);
						}
						foreach (GameObject bArs in Bars) {
								bArs.SetActive (false);
						}
						foreach (GameObject bOmb in Bombs) {
								bOmb.SetActive (false);
						}
						foreach (GameObject ProJ in Projectiles) {
								ProJ.SetActive (false);
						}
						foreach (GameObject fIre in FireShots) {
								fIre.SetActive (false);
						}
						foreach (GameObject iCes in Ices) {
								iCes.SetActive (false);
						}
				}

		}
}
