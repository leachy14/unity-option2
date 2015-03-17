using UnityEngine;
using System.Collections;
using Store;

namespace Enemy {
public class Enemy_Coll : MonoBehaviour
{
		public int hlth;

	// Use this for initialization
	void Start ()
	{
	if (this.gameObject.name == "Sanic_Raptor(Clone)") {
						hlth = 2;
				} else {
				hlth = 1;
			}
	}
	
	// Update is called once per frame
	void Update ()
	{

	if (hlth < 1) 
		{
			StoreControl.Coins += 10;
			if (this.gameObject.name == "Sanic_Raptor(Clone)") {
				StoreControl.Coins += 40;
				} else if (this.gameObject.name == "Dino_enemy(Clone)") {
					StoreControl.Coins += 10;
				}
			Destroy(gameObject);

		}
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Projectile")
		{
				Destroy(coll.gameObject);
				hlth -= 1;
			}

		}
	}
}
