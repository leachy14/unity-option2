using UnityEngine;
using UnityEditor;

using System.Collections;

public class Link : EditorWindow {
	
    [MenuItem ("TDTK/TowerEditor", false, 10)]
    static void OpenTowerEditor () {
    	TowerEditor.Init();
    }
    
    [MenuItem ("TDTK/SpawnEditor", false, 10)]
    static void OpenSpawnEditor () {
    	SpawnEditor.Init();
    }
    

    [MenuItem ("TDTK/Support Forum", false, 100)]
    static void OpenForumLink () {
    	Application.OpenURL("http://goo.gl/NqSua");
    }
	
}
