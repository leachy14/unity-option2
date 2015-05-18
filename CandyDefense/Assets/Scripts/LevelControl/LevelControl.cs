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
	
				//Game Objects	
				public GameObject[] enemies;     //array of enemies
				public GameObject[] spawnPoints; //array of spawnpoints
				public GameObject[] Turrets;
				public GameObject[] Bars;
				public GameObject[] Projectiles;
				public GameObject[] FireShots;
				public GameObject[] Bombs;
				public GameObject[] Bomber;
				public GameObject[] FlameThrowers;
				public GameObject[] Ices;
				public GameObject[] Snipers;
				public GameObject[] FreezeRay;
				public GameObject Raptor;
				public GameObject SanicRaptor;
				public GameObject DinoFuck;
				public GameObject Bar;
				public Toggle SpeedTog;
				public GameObject Pause;
				public GameObject flame_tur;
				public GameObject Green;
				public GameObject IcesP;
				public GameObject Bombers;
	
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
				public int num;
				public int NumFlameSP = 1;
				public int NumGreenSP = 1;
				public int NumSniperSP = 1;
				public int NumIceSP = 1;
				public int NumBombSP = 1;

				//floats
				public float hSliderValue = 5.0F;
				public float startTimer;    //countdown until timer ends	
				public float SpawnRate;
				public float lives;
				public Image speedon;
	
	
				//Access other scripts
				public GameObject store_accessor;
				public StoreControl storecontrol;

				
				//Turret Positions
				public Vector3 FlamePos;
				public Vector3 SnipersPos;
				public Vector3 TurretPos;
				public Vector3 IcePos;
				public Vector3 BomberPos;

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
						num = 1;
						wave = PlayerPrefs.GetInt ("Round");
						maxEnemies = PlayerPrefs.GetInt ("MaxEnemies");
						roundIsOver = true;
						startTimer = 2;
						current_enemy_amount = 0;
						Tutorial = 1;
						lives = PlayerPrefs.GetFloat ("Health");
						store_accessor = GameObject.Find ("Store");
						storecontrol = store_accessor.GetComponent <StoreControl> ();
						SpawnRate = 1F;
						currentEnemies = PlayerPrefs.GetInt ("currentEnemies");
						for (NumFlameSP = 1; NumFlameSP <= PlayerPrefs.GetInt("NumFlame"); NumFlameSP++) {
								FlamePos.x = PlayerPrefs.GetFloat ("FlameX" + NumFlameSP.ToString ());
								FlamePos.y = PlayerPrefs.GetFloat ("FlameY" + NumFlameSP.ToString ());
								Instantiate (flame_tur, FlamePos, transform.rotation);
								PlayerPrefs.SetInt ("NumFlameSpawn", NumFlameSP);
						}
						for (NumGreenSP = 1; NumGreenSP <= PlayerPrefs.GetInt("NumGreen"); NumGreenSP++) {
								TurretPos.x = PlayerPrefs.GetFloat ("GreenX" + NumGreenSP.ToString ());
								TurretPos.y = PlayerPrefs.GetFloat ("GreenY" + NumGreenSP.ToString ());
								Instantiate (Green, TurretPos, transform.rotation);
								PlayerPrefs.SetInt ("NumGreenSpawn", NumGreenSP);
						}
						for (NumIceSP = 1; NumIceSP <= PlayerPrefs.GetInt("NumIce"); NumIceSP++) {
								TurretPos.x = PlayerPrefs.GetFloat ("IceX" + NumIceSP.ToString ());
								TurretPos.y = PlayerPrefs.GetFloat ("IceY" + NumIceSP.ToString ());
								Instantiate (IcesP, TurretPos, transform.rotation);
								PlayerPrefs.SetInt ("NumIceSpawn", NumIceSP);
						}
						//PlayerPrefs.SetInt ("NumFlame", 0);
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
								foreach (GameObject bOmb in Bomber) {
										bOmb.SetActive (false);
								}
								foreach (GameObject ProJ in Projectiles) {
										ProJ.SetActive (false);
								}
								foreach (GameObject fIre in FireShots) {
										fIre.SetActive (false);
								}
								foreach (GameObject fIre in FlameThrowers) {
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
								foreach (GameObject bOmb in Bomber) {
										bOmb.SetActive (true);
								}
								foreach (GameObject ProJ in Projectiles) {
										ProJ.SetActive (true);
								}
								foreach (GameObject fIre in FireShots) {
										fIre.SetActive (true);
								}
								foreach (GameObject fIre in FlameThrowers) {
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
								Bomber = GameObject.FindGameObjectsWithTag ("Bomber");
								FlameThrowers = GameObject.FindGameObjectsWithTag ("Thrower");
								Bars = GameObject.FindGameObjectsWithTag ("Bar");
								FireShots = GameObject.FindGameObjectsWithTag ("Fire");
								Projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
								Ices = GameObject.FindGameObjectsWithTag ("Ice");
								FreezeRay = GameObject.FindGameObjectsWithTag ("Freeze");
								Bombs = GameObject.FindGameObjectsWithTag ("Bomb");
						}

						pauseY = (Screen.height / 2 - 100);
						pauseX = (Screen.width / 2 - 200);

						
						if (lives <= 0) {
								PlayerPrefs.SetInt ("Money", 500);
								PlayerPrefs.SetFloat ("Health", 20);
								PlayerPrefs.SetInt ("Round", 0);
								PlayerPrefs.SetInt ("NumFlame", 0);
								PlayerPrefs.SetInt ("MaxEnemies", 1);
								PlayerPrefs.SetInt ("currentEnemies", 1);
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
						num = 1;
						PlayerPrefs.SetInt ("NumFlame", FlameThrowers.Length);
						PlayerPrefs.SetInt ("NumSnipe", Snipers.Length);
						PlayerPrefs.SetInt ("NumIce", FreezeRay.Length);
						PlayerPrefs.SetInt ("NumBomber", Bomber.Length);
						PlayerPrefs.SetInt ("Money", storecontrol.Coins);
						PlayerPrefs.SetInt ("Round", wave);
						PlayerPrefs.SetInt ("MaxEnemies", maxEnemies);
						PlayerPrefs.SetFloat ("Health", lives);
						PlayerPrefs.SetInt ("currentEnemies", currentEnemies);
						foreach (GameObject ThRoW in FlameThrowers) {
								PlayerPrefs.SetFloat ("FlameX" + num.ToString (), ThRoW.transform.position.x);
								PlayerPrefs.SetFloat ("FlameY" + num.ToString (), ThRoW.transform.position.y);
								num++;
						}

						num = 1;
						foreach (GameObject objs in Turrets) {
								PlayerPrefs.SetFloat ("GreenX" + num.ToString (), objs.transform.position.x);
								PlayerPrefs.SetFloat ("GreenY" + num.ToString (), objs.transform.position.y);
								num++;
						}
						num = 1;
						foreach (GameObject objs in FreezeRay) {
								PlayerPrefs.SetFloat ("IceX" + num.ToString (), objs.transform.position.x);
								PlayerPrefs.SetFloat ("IceY" + num.ToString (), objs.transform.position.y);
								num++;
						}				
						num = 1;
						foreach (GameObject objs in Bomber) {
								PlayerPrefs.SetFloat ("BombX" + num.ToString (), objs.transform.position.x);
								PlayerPrefs.SetFloat ("BombY" + num.ToString (), objs.transform.position.y);
								num++;
						}
						PlayerPrefs.Save ();
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
