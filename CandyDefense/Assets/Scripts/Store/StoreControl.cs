using UnityEngine;
using System.Collections;
using Level;
using UnityEngine.UI;


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
	public Texture Bomber_text;


	//Toggles
	public bool illum_toggle = false;
	public bool StoreOpen = false;
	public bool green_toggle = false;
	public bool Flame_toggle = false;
	public bool Bomber_toggle = false;


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
	public GameObject Bomber;

	//Path Collider
	public GameObject path_collide;

	//Access other scripts
	public GameObject round_level;
	public LevelControl round_accessor;

	//Ui Stuff New UI
	private GameObject Health_Slider;
	

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


			if (round_accessor.current_enemy_amount > 0) {
				StoreOpen = false;
			}
			
			if (round_accessor.current_enemy_amount == 0) {
				StoreOpen = true;
			}
		
			if (illum_toggle == true && StoreOpen == true && Coins >= 100) {
				if (Input.GetMouseButton (0)) {
					if (path_collide.GetComponent<Collider2D>().OverlapPoint (transform.position) == false) {	
						Instantiate (illum_turret, transform.position, transform.rotation);
						Coins = (Coins - 100);
						illum_toggle = false;
					}
				}
			}
			if (green_toggle == true && StoreOpen == true && Coins >= 100) {
				if (Input.GetMouseButton (0)) {
					if (path_collide.GetComponent<Collider2D>().OverlapPoint (transform.position) == false) {
						Instantiate (green_tur, transform.position, transform.rotation);
						Coins = (Coins - 100);
						green_toggle = false;
					}
				}
			}
			if (Flame_toggle == true && StoreOpen == true && Coins >= 100) {
				if (Input.GetMouseButton (0)) {
					if (path_collide.GetComponent<Collider2D>().OverlapPoint (transform.position) == false) {
						Instantiate (flame_tur, transform.position, transform.rotation);
						Coins = (Coins - 100);
						Flame_toggle = false;
					}
				}
			}
			if (Bomber_toggle == true && StoreOpen == true && Coins >= 100) {
				if (Input.GetMouseButton (0)) {
					if (path_collide.GetComponent<Collider2D>().OverlapPoint (transform.position) == false) {
						Instantiate (Bomber, transform.position, transform.rotation);
						Coins = (Coins - 100);
						Bomber_toggle = false;
					}
				}
			}



		}
			

}
}
