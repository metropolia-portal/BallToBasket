using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {
	
	bool bounce = false;
	float timeToReactOnBounce;
	
	float  yTrash;
	public float forceStrength;
	// Use this for initialization
	void Start () {
		yTrash = GameObject.Find("trash").transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		timeToReactOnBounce -= Time.deltaTime;
		if(transform.position.y < yTrash)
		{			
			Debug.Log("Lost");
		}			
	}
	
	void OnCollisionStay(Collision col)
	{			
		if (bounce)			
		{
			if(timeToReactOnBounce >= 0)
			{
				//Vector3 velocity = this.rigidbody.velocity;
				Vector3 norm = col.contacts[0].normal;
				//Vector3 force = (/*velocity +*/ norm);				
//				force.z = 0;
				rigidbody.AddForce(forceStrength*norm,ForceMode.Impulse);	
				bounce = false;
			}
		}

	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width-100,2,100,30), "Bounce"))
		{
			if(this.rigidbody.velocity == new Vector3(0f,0f,0f))
			{
				rigidbody.AddForce(forceStrength*(new Vector3(0,1,0)),ForceMode.Impulse);	
			}
			else
			{			
				bounce = true;
				timeToReactOnBounce = 0.3f;
			}
		}
	}
	
	
}
