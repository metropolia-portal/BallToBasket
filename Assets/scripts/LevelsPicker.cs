using UnityEngine;
using System.Collections;

public class LevelsPicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI() {
		int unit = Screen.width/ 30;
		if(GUI.Button (new Rect(0,0,unit*2, unit),"0")) 
			Application.LoadLevel(0);
		
		if(GUI.Button (new Rect(unit*3, 0,unit*2, unit),"1")) 
			Application.LoadLevel(1);
	}
}
