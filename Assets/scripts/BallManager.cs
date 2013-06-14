using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {
	
	float  yTrash;
	public float forceStrength = 50;
	// Use this for initialization
	void Start () {
		yTrash = GameObject.Find("trash").transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < yTrash)
		{			
			Debug.Log("Lost");
		}	
	}
	
	void OnCollisionEnter(Collision col)
	{			
		/*Vector3 velocity = this.rigidbody.velocity;
		Vector3 norm = col.contacts[0].normal;
		Vector3 force = velocity + forceStrength*norm;
		force.z = 0;
		rigidbody.AddForce(force);*/		
	}
}
