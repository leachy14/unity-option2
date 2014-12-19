using UnityEngine;
using System.Collections;

public class ParticleControl : MonoBehaviour {

	private ParticleSystem pSystem;
	private float startLifeTime;
	private bool looping;

	private ParticleEmitter pEmitter;
	private float minE=0;
	private float maxE=0;
	
	private float duration;
	
	public bool oneShot=false;
	

	
	private GameObject thisObj;
	//private Transform thisT;
	
	void Awake(){
		thisObj=gameObject;
		//thisT=transform;
		
		pEmitter=thisObj.GetComponent<ParticleEmitter>();
		if(pEmitter!=null){
			minE=pEmitter.minEmission;
			maxE=pEmitter.maxEmission;
			
			pEmitter.minEmission=0;
			pEmitter.maxEmission=0;
			
			duration=pEmitter.maxEnergy;
		}
		
		pSystem=thisObj.GetComponent<ParticleSystem>();
		if(pSystem!=null){
			duration=pSystem.duration+pSystem.startLifetime;
			looping=pSystem.loop;
		}
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnEnable(){
		if(pEmitter!=null){
			if(oneShot){
				pEmitter.Emit((int)Random.Range(minE, maxE));
				StartCoroutine(Disable());
			}
			else{
				pEmitter.minEmission=minE;
				pEmitter.maxEmission=maxE;
			}
		}
		else if(pSystem!=null){
			if(!looping){
				StartCoroutine(Disable());
			}
			else{
				pSystem.loop=true;
			}
		}
	}
	
	public void OnDisable(){
		if(pEmitter!=null){
			if(!oneShot){
				pEmitter.minEmission=0;
				pEmitter.maxEmission=0;
			}
		}
		else if(pSystem!=null){
			if(looping){
				pSystem.loop=false;
			}
		}
	}
	
	public IEnumerator Disable(){
		yield return new WaitForSeconds(duration);
		
		ObjectPoolManager.Unspawn(thisObj);
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
