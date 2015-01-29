using UnityEngine;
using System.Collections;


namespace Store {
public class StoreControl : MonoBehaviour
{
	public Vector2 scrollPosition = Vector2.zero;
	public Texture illuminaty_turret;
	public Texture startbutton;
	private bool illum_toggle = false;
	public static int Coins = 100;
	public GameObject illum_turret;
	public string Money;
	public RaycastHit hit;
	public Camera main;
	public int Storeposition;
	public int MoneyHeight;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 vec = main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = vec;
		Money =	"<color=#ff0000ff><size=30>Coins: " + Coins + "</size></color>";
			Storeposition = (Screen.width - 150);
			MoneyHeight = (Screen.height - 100);
	}
	void OnGUI () {
		//Store Menu
		scrollPosition = GUI.BeginScrollView(new Rect(Storeposition, 10, 140, 300), scrollPosition, new Rect(0, 0, 140, 200));
		illum_toggle = GUI.Toggle(new Rect(0, 0, 50, 50), illum_toggle, illuminaty_turret);
		GUI.Button(new Rect(55, 0, 50, 50), "Turret 2");
		GUI.Button(new Rect(0, 55, 50, 50), "Turret 3");
		GUI.Button(new Rect(55, 55, 50, 50), "Turret 4");
		GUI.EndScrollView();
		GUI.Label(new Rect(10, MoneyHeight, 200, 300), Money);
		if (illum_toggle == true && Coins >= 100) {
			if(Input.GetMouseButton(0)) {
				Instantiate(illum_turret, transform.position, transform.rotation);
				Coins = (Coins - 100);
				illum_toggle = false;
				}
			}
		}
	public void BuyIllumTurret () { 
		if (Coins >= 100) {
			if(Input.GetMouseButton(0)) {
				Instantiate(illum_turret, transform.position, transform.rotation);
				Coins = (Coins - 100);
				illum_toggle = false;
				}	
			}
		}
	public void ShowStore() {
		Storeposition = (Screen.width - 100);
		}
	public void HideStore() {
		Storeposition = Screen.width;
		}
	}
}
