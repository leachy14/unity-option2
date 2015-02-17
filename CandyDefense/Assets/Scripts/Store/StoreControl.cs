using UnityEngine;
using System.Collections;
using Level;

namespace Store {
public class StoreControl : MonoBehaviour
{
	

	
	public static int Coins = 100;
	
	public string Money;
	public string Lives;
	public Camera main;
	
	
	//Textures
	public Texture illuminaty_turret;
	public Texture startbutton;
	public Texture green_turret;
	public Texture flamethrower;
	public Texture sniper;

	//Toggles
	private bool illum_toggle = false;
	public bool StoreOpen = false;


	//Positions
	public int Storeposition;
	public int MoneyHeight;
	public int StoreOpenButtonPos;
	public int StorePlaneH;
	public int test;

	//Turrets 
	public GameObject illum_turret;
	public GameObject green_tur;
	public GameObject flame_tur;
	public GameObject sniper_turret;

	//Path Collider
	public GameObject path_collide;

	//Access other scripts
	public GameObject round_level;
	public LevelControl round_accessor;


	//Toggles
		private bool green_toggle = false;
		private bool flame_toggle = false;
		private bool sniper_toggle = false;
	//Positions
		public Vector2 scrollPosition = Vector2.zero;


	// Use this for initialization
	void Start ()
	{
			StorePlaneH = Screen.height;

			round_level = GameObject.Find ("spawner");
			round_accessor = round_level.GetComponent<LevelControl> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 vec = main.ScreenToWorldPoint(Input.mousePosition);
		//transform.position.z = 0;
		transform.position = vec;
		
		Money =	"<color=#ff0000ff><size=30>Coins: " + Coins + "</size></color>";
		Lives = "<color=#ff0000ff><size=30>Lives left: " + round_accessor.lives + "</size></color>";

			if (StoreOpen == true) {
				Storeposition = (Screen.width - 180);
				StoreOpenButtonPos = (Screen.width - 200);
				}
			if (StoreOpen == false) {
				Storeposition = Screen.width;
				StoreOpenButtonPos = (Screen.width - 35);
				}

			MoneyHeight = (Screen.height - 100);
			test = Screen.height;




	}
	void OnGUI () {
						//Store Menu
						scrollPosition = GUI.BeginScrollView (new Rect (Storeposition, 10, 170, Screen.height), scrollPosition, new Rect (0, 0, 140, 1000));
						if (GUI.Button (new Rect (0, 0, 64, 64), illuminaty_turret)) {
								illum_toggle = true;
								green_toggle = false;
								flame_toggle = false;
						}
						if (GUI.Button (new Rect (70, 0, 65, 65), green_turret)) {
								green_toggle = true;
								illum_toggle = false;
								flame_toggle = false;
						}
						if (GUI.Button (new Rect (0, 70, 65, 65), flamethrower)) {
								flame_toggle = true;
								illum_toggle = false;
								green_toggle = false;
						}
						
						if (GUI.Button (new Rect (70, 70, 65, 65), sniper)) {
								flame_toggle = false;
								illum_toggle = false;
								green_toggle = false;
								sniper_toggle = true;
						}	
						GUI.EndScrollView ();

						if (round_accessor.current_enemy_amount > 0) {
								StoreOpen = false;
						}

						if (round_accessor.current_enemy_amount == 0) {
								StoreOpen = true;
						}


						//show the store
						StoreOpen = GUI.Toggle (new Rect (StoreOpenButtonPos, 20, 20, Screen.height), StoreOpen, "");

						//You've got money
						GUI.Label (new Rect (10, MoneyHeight, 200, 300), Money);
						GUI.Label (new Rect (10, (Screen.height - 125), 200, 300), Lives);
		
						//Place the damn turret
						if (illum_toggle == true && Coins >= 100) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.collider2D.OverlapPoint (transform.position) == false) {
												Instantiate (illum_turret, transform.position, transform.rotation);
												Coins = (Coins - 100);
												illum_toggle = false;
										}
								}
						}
						if (green_toggle == true && Coins >= 100) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.collider2D.OverlapPoint (transform.position) == false) {
												Instantiate (green_tur, transform.position, transform.rotation);
												Coins = (Coins - 100);
												green_toggle = false;
										}
								}
						}
						if (flame_toggle == true && Coins >= 100) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.collider2D.OverlapPoint (transform.position) == false) {
												Instantiate (flame_tur, transform.position, transform.rotation);
												Coins = (Coins - 100);
												flame_toggle = false;
										}
								}
						}
						if (sniper_toggle == true && Coins >= 100) {
							if (Input.GetMouseButton (0)) {
								if (path_collide.collider2D.OverlapPoint (transform.position) == false) {
									Instantiate (sniper_turret, transform.position, transform.rotation);
									Coins = (Coins - 100);
									sniper_toggle = false;
					}
				}
			}
				}
}
}
