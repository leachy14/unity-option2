using UnityEngine;
using System.Collections;
using Level;
using UnityEngine.UI;

namespace Store
{
		public class StoreControl : MonoBehaviour
		{
	

	
				public int Coins;
				public Camera main;

				//Toggles
				public bool illum_toggle = false;
				public bool StoreOpen = false;
				public bool green_toggle = false;
				public bool Flame_toggle = false;
				public bool Bomber_toggle = false;
				public bool Sniper_toggle = false;
				public bool Ice_toggle = false;
				public bool GameSpeed = false;

				//Turrets 
				public GameObject illum_turret;
				public GameObject green_tur;
				public GameObject flame_tur;
				public GameObject Bomber;
				public GameObject Sniper_tur;
				public GameObject Ice_tur;


				//Path Collider
				public GameObject path_collide;

				//Access other scripts
				public GameObject round_level;
				public LevelControl round_accessor;

				//Ui Stuff New UI
				private GameObject Health_Slider;
	

			


				// Use this for initialization
				void Start ()
				{
						

						round_level = GameObject.Find ("spawner");
						round_accessor = round_level.GetComponent<LevelControl> ();
						Coins = PlayerPrefs.GetInt ("Money");
				}
	
				// Update is called once per frame
				void Update ()
				{
						Vector3 vec = main.ScreenToWorldPoint (Input.mousePosition);
		
						transform.position = vec;
							

						if (round_accessor.current_enemy_amount > 0) {
								StoreOpen = false;
						}
			
						if (round_accessor.current_enemy_amount == 0) {
								StoreOpen = true;
						}
		
						if (illum_toggle == true && StoreOpen == true && Coins >= 100) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {	
												Instantiate (illum_turret, transform.position, transform.rotation);
												Coins = (Coins - 100);
												illum_toggle = false;
										}
								}
						}
						if (green_toggle == true && StoreOpen == true && Coins >= 250) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {
												Instantiate (green_tur, transform.position, transform.rotation);
												Coins = (Coins - 250);
												green_toggle = false;
										}
								}
						}
						if (Flame_toggle == true && StoreOpen == true && Coins >= 1500) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {
												Instantiate (flame_tur, transform.position, transform.rotation);
												Coins = (Coins - 1500);
												Flame_toggle = false;
										}
								}
						}
						if (Bomber_toggle == true && StoreOpen == true && Coins >= 1250) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {
												Instantiate (Bomber, transform.position, transform.rotation);
												Coins = (Coins - 1250);
												Bomber_toggle = false;
										}
								}
						}
						if (Sniper_toggle == true && StoreOpen == true && Coins >= 500) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {	
												Instantiate (Sniper_tur, transform.position, transform.rotation);
												Coins = (Coins - 500);
												Sniper_toggle = false;
										}
								}
						}
						if (Ice_toggle == true && StoreOpen == true && Coins >= 1000) {
								if (Input.GetMouseButton (0)) {
										if (path_collide.GetComponent<Collider2D> ().OverlapPoint (transform.position) == false && transform.position.y > -1) {	
												Instantiate (Ice_tur, transform.position, transform.rotation);
												Coins = (Coins - 1000);
												Ice_toggle = false;
										}
								}
						}



				}
			

		}
}
