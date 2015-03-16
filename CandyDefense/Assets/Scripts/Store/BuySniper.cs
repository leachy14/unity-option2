using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Store;

public class BuySniper : MonoBehaviour
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
		//Store_bool.
	}
}

