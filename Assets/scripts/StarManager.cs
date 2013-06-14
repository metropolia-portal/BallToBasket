using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "ball")
		{
			// TODO: add score			
			Destroy(this.gameObject);
		}
	}
}
