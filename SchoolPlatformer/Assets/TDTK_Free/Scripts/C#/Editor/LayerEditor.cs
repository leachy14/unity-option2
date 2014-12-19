using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor(typeof(LayerManager))]
public class LayerEditor : Editor {

	
	//~ [MenuItem ("TDTK/LayerEditor")]
    //~ static void Init(){
        //~ // Get existing open window or if none, make a new one:
        //~ window = (LayerEditor)EditorWindow.GetWindow(typeof (LayerEditor));
		//~ window.minSize=new Vector2(300, 300);
		
		//~ layerCreep=PlayerPrefs .GetInt("CreepLayer", 31);
		//~ layerCreepF=PlayerPrefs .GetInt("CreepFLayer", 30);
			
		//~ layerTower=PlayerPrefs .GetInt("TowerLayer", 29);
		//~ layerPlatform=PlayerPrefs .GetInt("PlatformLayer", 28);
		
		//~ GetSpawnManager();
    //~ }
	
	public override void OnInspectorGUI(){
		LayerManager lm = (LayerManager)target;
		
		GUI.changed = false;
		
		lm.layerCreep=EditorGUILayout.LayerField("Creep Layer:", lm.layerCreep);
		lm.layerCreepF=EditorGUILayout.LayerField("CreepF Layer:", lm.layerCreepF);
		lm.layerTower=EditorGUILayout.LayerField("Tower Layer:", lm.layerTower);
		lm.layerPlatform=EditorGUILayout.LayerField("Platform Layer:", lm.layerPlatform);
		lm.layerOverlay=EditorGUILayout.LayerField("Overlay Layer:", lm.layerOverlay);
		lm.layerMiscUI=EditorGUILayout.LayerField("Misc UI Layer:", lm.layerMiscUI);
	
		//~ lm.layerCreep=Mathf.Clamp(lm.layerCreep, 0, 31);
		//~ lm.layerCreepF=Mathf.Clamp(lm.layerCreepF, 0, 31);
		//~ lm.layerTower=Mathf.Clamp(lm.layerTower, 0, 31);
		//~ lm.layerPlatform=Mathf.Clamp(lm.layerPlatform, 0, 31);
		
		if(GUI.changed) EditorUtility.SetDirty(lm);
	}
}
