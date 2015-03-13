using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Level;

public class Health : MonoBehaviour
{
	public Slider health;

	//Access other scripts
	public GameObject round_level;
	public LevelControl round_accessor;


	void Start () {
		round_level = GameObject.Find ("spawner");
		round_accessor = round_level.GetComponent<LevelControl> ();
	}


	// Update is called once per frame
	void Update ()
	{
		health.value = Mathf.MoveTowards (health.value, round_accessor.lives, 1f);
	}
}

