using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Store;

public class BuyFlame : MonoBehaviour
{

	public GameObject Store_thing;
	public StoreControl Store_bool;
		// Use this for initialization
		void Start ()
		{
		Store_thing = GameObject.Find("Store");
		Store_bool = Store_thing.GetComponent<StoreControl> ();
		}
	
		// Update is called once per frame
		public void Buy_Flame () 
		{
			Store_bool.Flame_toggle = true;
		}
}

