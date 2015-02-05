using UnityEngine;
using System.Collections;


namespace Store {
public class StoreControl : MonoBehaviour
{
	public Vector2 scrollPosition = Vector2.zero;

	
	public static int Coins = 100;
	
	public string Money;
	public RaycastHit hit;
	public Camera main;
	
	
	//Textures
	public Texture illuminaty_turret;
	public Texture startbutton;


	//Toggles
	private bool illum_toggle = false;
	private bool StoreOpen = false;


	//Positions
	public int Storeposition;
	public int MoneyHeight;
	public int StoreOpenButtonPos;
	public int StorePlaneH;
	public int test;

	//Turrets 
	public GameObject illum_turret;
	
	//Path Collider
	public GameObject path_collide;

	//Access other scripts
	public GameObject round_level;
	public LevelControl round_accessor;

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

			if (StoreOpen == true) {
				Storeposition = (Screen.width - 160);
				StoreOpenButtonPos = (Screen.width - 180);
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
		scrollPosition = GUI.BeginScrollView(new Rect(Storeposition, 10, 156, Screen.height), scrollPosition, new Rect(0, 0, 140, 1000));
		illum_toggle = GUI.Toggle(new Rect(0, 0, 50, 50), illum_toggle, illuminaty_turret);
		GUI.Button(new Rect(55, 0, 50, 50), "Turret");
		GUI.Button(new Rect(0, 55, 50, 50), "Turret");
		GUI.Button(new Rect(55, 55, 50, 50), "Turret");
		GUI.Button(new Rect(0, 110, 50, 50), "Turret");
		GUI.Button(new Rect(55, 110, 50, 50), "Turret");
		GUI.Button(new Rect(0, 165, 50, 50), "Turret");
		GUI.Button(new Rect(55, 165, 50, 50), "Turret");
		GUI.Button(new Rect(0, 220, 50, 50), "Turret");
		GUI.Button(new Rect(55, 220, 50, 50), "Turret");
		GUI.Button(new Rect(0, 275, 50, 50), "Turret");
		GUI.Button(new Rect(55, 275, 50, 50), "Turret");
		GUI.Button(new Rect(0, 330, 50, 50), "Turret");
		GUI.Button(new Rect(55, 330, 50, 50), "Turret");
		GUI.Button(new Rect(0, 385, 50, 50), "Turret");
		GUI.Button(new Rect(55, 385, 50, 50), "Turret");
		GUI.EndScrollView();

		if (round_accessor.current_enemy_amount > 0) {
				StoreOpen = false;
			}

		if (round_accessor.current_enemy_amount == 0) {
				StoreOpen = true;
			}


		//show the store

			StoreOpen = GUI.Toggle (new Rect (StoreOpenButtonPos, 20, 20, Screen.height), StoreOpen, "");

			//You've got money
		GUI.Label(new Rect(10, MoneyHeight, 200, 300), Money);
		
			//Place the damn turret
		if (illum_toggle == true && Coins >= 100) {
			if(Input.GetMouseButton(0)) {
					if (path_collide.collider2D.OverlapPoint(transform.position) == false){
					Instantiate(illum_turret, transform.position, transform.rotation);
					Coins = (Coins - 100);
					illum_toggle = false;
				}
				}
			}
		}

	}
}
