using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	UserFieldManager fieldManager;
	// Use this for initialization
	void Start () {
		fieldManager = GameObject.Find("Screen").GetComponent<UserFieldManager>();		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	

}
