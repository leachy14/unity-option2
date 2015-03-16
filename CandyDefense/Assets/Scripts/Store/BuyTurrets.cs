using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Store;

public class BuyTurrets : MonoBehaviour
{

	public GameObject Store_sniper;
	public StoreControl Store_bool;
		// Use this for initialization
		void Start ()
		{
		Store_sniper = GameObject.Find("Store");
		Store_bool = Store_sniper.GetComponent<StoreControl> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	public void Buy_Sniper () {
		//Store_bool.SniperToggle = true;
	}
	public void Buy_Flame () {
		Store_bool.Flame_toggle = true;
	}
	public void Buy_Bomber () {
		Store_bool.Bomber_toggle = true;
	}
}

