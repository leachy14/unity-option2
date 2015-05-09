using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Store;

public class BuyTurrets : MonoBehaviour
{

	public GameObject Store_sniper;
	public StoreControl Store_bool;
	public Text NameTur;
	public Text Price;

		// Use this for initialization
		void Start ()
		{
		Store_sniper = GameObject.Find("Store");
		Store_bool = Store_sniper.GetComponent<StoreControl> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		Price = GameObject.Find("Turret Price").GetComponent<Text>();
		NameTur = GameObject.Find("Turret Name").GetComponent<Text> ();
		}
	public void Buy_Sniper () {
		Store_bool.Sniper_toggle = true;
		Store_bool.green_toggle = false;
		Store_bool.Bomber_toggle = false;
		Store_bool.Flame_toggle = false;
		Store_bool.Ice_toggle = false;
		Price.text = ("$500");
		NameTur.text = ("Sniper");
	}
	public void Buy_Flame () {
		Store_bool.Flame_toggle = true;
		Store_bool.green_toggle = false;
		Store_bool.Bomber_toggle = false;
		Store_bool.Sniper_toggle = false;
		Store_bool.Ice_toggle = false;
		Price.text = ("$1500");
		NameTur.text = ("Flame Thrower");
	}
	public void Buy_Bomber () {
		Store_bool.Bomber_toggle = true;
		Store_bool.green_toggle = false;
		Store_bool.Sniper_toggle = false;
		Store_bool.Flame_toggle = false;
		Store_bool.Ice_toggle = false;
		Price.text = ("$1250");
		NameTur.text = ("Bomber");
	}
	public void Buy_Normal () {
		Store_bool.green_toggle = true;
		Store_bool.Sniper_toggle = false;
		Store_bool.Bomber_toggle = false;
		Store_bool.Flame_toggle = false;
		Store_bool.Ice_toggle = false;
		Price.text = ("$250");
		NameTur.text = ("Green Turret");
	}
	public void Buy_Ice () {
		Store_bool.Ice_toggle = true;
		Store_bool.green_toggle = false;
		Store_bool.Bomber_toggle = false;
		Store_bool.Flame_toggle = false;
		Store_bool.Sniper_toggle = false;
		Price.text = ("$1000");
		NameTur.text = ("Ice Turret");
	}
}

