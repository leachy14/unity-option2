using UnityEngine;
using System.Collections;
using Store;
using Walk;


namespace Enemy {
public class Enemy_Coll : MonoBehaviour
{
		public float hlth;
		public GameObject thisEnemy;
		public Dino_walk Speedgetter;
		public GameObject store_accessor;
		public StoreControl storecontrol;
		public bool Froze = false;
		private double nextFire = 0.0;
		public double Unfreeze = 3;
		public int SanicSp = 4;
		public int RaptorSp = 2;
		public int SalidSp = 1;


	// Use this for initialization
	void Start ()
	{

			thisEnemy = this.gameObject;
			Speedgetter = thisEnemy.GetComponent<Dino_walk> ();
			store_accessor = GameObject.Find ("Store");
			storecontrol = store_accessor.GetComponent <StoreControl> ();
			if (this.gameObject.name == "Sanic_Raptor(Clone)") {
						hlth = 2f;
				} else if (this.gameObject.name == "Dino_enemy(Clone)") {
				hlth = 1f;
			}else {
				hlth = 6f;
			}
	}
	
	// Update is called once per frame
	void Update ()
	{

	if (hlth <= 0) 
		{
				storecontrol.Coins = storecontrol.Coins + 10;
			if (this.gameObject.name == "Sanic_Raptor(Clone)") {
					storecontrol.Coins = storecontrol.Coins + 40;
				} else if (this.gameObject.name == "Dino_enemy(Clone)") {
					storecontrol.Coins = storecontrol.Coins + 10;
				} else if (this.gameObject.name == "Salid_snake(Clone)") {
					storecontrol.Coins = storecontrol.Coins + 60;
				}
			Destroy(gameObject);

		}
			if (Froze == true){

				if (Time.time >  nextFire) {
					Froze = false;
				}
			} else if (Froze == false) {
				if (this.gameObject.name == "Sanic_Raptor(Clone)") {
					Speedgetter.Speed = SanicSp;
				} else if (this.gameObject.name == "Salid_snake(Clone)") {
					Speedgetter.Speed = SalidSp;
				} else if (this.gameObject.name == "Dino_enemy(Clone)") {
					Speedgetter.Speed = RaptorSp;
				}
			}
	}
		void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.gameObject.tag == "Projectile")
		{
				Destroy(coll.gameObject);
				hlth -= 1f;
			}
		if (coll.gameObject.tag == "Bomb")
		{
				hlth = 0f;
			}
			if(coll.gameObject.tag == "Ice") {
				Destroy(coll.gameObject);
				//hlth -= 1;
				if (Froze == false){
					Speedgetter.Speed = Speedgetter.Speed / 1.5f;
					Froze = true;
					nextFire = (Time.time + Unfreeze);
				}
			}
		if(coll.gameObject.tag == "Fire") {
				Destroy(coll.gameObject);
				hlth -= 0.10f;

			}
		}
	
	}
}
