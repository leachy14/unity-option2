using UnityEngine;
using System.Collections;
using Store;

namespace Enemy {
public class Enemy_Coll : MonoBehaviour
{
	public int hlth = 1;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	if (hlth < 1) {
			
			StoreControl.Coins += 100;
			Debug.Log ("Got coins?");
				Destroy(gameObject);

		}
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Projectile")
		{
			
			hlth -= 1;
				Destroy(coll.gameObject);
		}
	}
}

}
