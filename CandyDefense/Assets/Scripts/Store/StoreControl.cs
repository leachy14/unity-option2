using UnityEngine;
using System.Collections;

public class StoreControl : MonoBehaviour
{
	public Vector2 scrollPosition = Vector2.zero;
	public Texture illuminaty_turret;
	public Texture startbutton;
	private bool illum_toggle = false;
	public int Coins;
	public GameObject illum_turret;

	// Use this for initialization
	void Start ()
	{
		Coins = 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnGUI () {
		//Store Menu
		scrollPosition = GUI.BeginScrollView(new Rect(520, 10, 140, 300), scrollPosition, new Rect(0, 0, 140, 200));
		illum_toggle = GUI.Toggle(new Rect(0, 0, 50, 50), illum_toggle, illuminaty_turret);
		GUI.Button(new Rect(55, 0, 50, 50), "Turret 2");
		GUI.Button(new Rect(0, 55, 50, 50), "Turret 3");
		GUI.Button(new Rect(55, 55, 50, 50), "Turret 4");
		GUI.EndScrollView();
		if (illum_toggle == true && Coins >= 100) {
			if(Input.GetMouseButton(0)) {
				Instantiate(illum_turret, transform.position, transform.rotation);
				Coins = (Coins - 100);
				illum_toggle = false;
			}
		}
	}
}