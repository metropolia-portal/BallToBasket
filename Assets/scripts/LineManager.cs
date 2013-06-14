using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {

	
	float maxDistance;
	float wholeDistance;
	float minColliderLenght;
	 
	public Material blockMaterial;
	PhysicMaterial blockPhysicsMaterial;
	//LineRenderer lineRenderer;	
	
	public Transform DotPrefab;
	public GameObject trailPrefab;
	GameObject ball;
	GameObject currentTrailRendererObject;

	
	Vector3 mousePosition; // Vector3 because of using a raycast.
	//Vector3 mouseHit;
	Vector3 newDotPosition;
	Vector3 lastDotPosition;
	
	
	bool dragging = false;
	bool lastPointExist = false;
	
	// status bar
	public Texture2D statusBarBorder;
	public Texture2D markerTexture;
	
	
	
	TrailRenderer trailRenderer;
	
	Vector3 mousePosForTrail;
	
	// Use this for initialization
	void Start () 
	{
		maxDistance = 10;		
		wholeDistance = 0;
		minColliderLenght = 0.5f;
		ball = GameObject.Find("ball");
		ball.rigidbody.useGravity = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (maxDistance - wholeDistance > 0)
		{
			if(Input.GetMouseButtonDown(0))
			{			
				if(!lastPointExist)
				{
					newDotPosition = MousePoint();
					lastDotPosition = MousePoint();				
					lastPointExist = true;				
				}
				MouseDown();
			}		
			if(Input.GetMouseButtonUp(0))
			{
				MouseUp();
			}			
			if(dragging)
			{						
				newDotPosition = MousePoint();		
				float colliderLengthBetweenFrames = Vector3.Distance(newDotPosition, lastDotPosition);								
				
				if(newDotPosition != lastDotPosition)					
				{	
					if(colliderLengthBetweenFrames >= minColliderLenght)
					{
						//
						// split collider length
						int amountOfColliders = (int)(colliderLengthBetweenFrames/minColliderLenght);						
						
						float deltaXW = (newDotPosition.x-lastDotPosition.x);
						float deltaYW = (newDotPosition.y-lastDotPosition.y);
						float deltaXT = deltaXW*(colliderLengthBetweenFrames - minColliderLenght*amountOfColliders)/colliderLengthBetweenFrames;
						float deltaYT = deltaYW*(colliderLengthBetweenFrames - minColliderLenght*amountOfColliders)/colliderLengthBetweenFrames;					
						
						
						
						float deltaXPerCollider = (deltaXW-deltaXT)/amountOfColliders;
						float deltaYPerCollider= (deltaYW-deltaYT)/amountOfColliders;						
						
						for(int i = 0; i < amountOfColliders; i++)
						{
							CreateBoxCollider(new Vector3(lastDotPosition.x + deltaXPerCollider, lastDotPosition.y + deltaYPerCollider, 0));
						}
					}
						
					currentTrailRendererObject.transform.position = lastDotPosition;
				}
				else
				{
					currentTrailRendererObject.transform.position = newDotPosition;
				}
				
								
			}
		}
		else
		{
			if (dragging)
			{
				MouseUp();
				currentTrailRendererObject.transform.position = lastDotPosition;
				dragging = false;
			}
		}
	}
	
	Vector3 MousePoint()
	{
		Ray ray = GameObject.Find("Main Camera").camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray,out hit,1000,Physics.kDefaultRaycastLayers);
		Vector3 rtn = hit.point;
		rtn.z = 0;
		return rtn;
	}
	
	void MouseDown()
	{		
		Vector3 mouseHit;
		// receive cur mouse position
		mouseHit = MousePoint();	
		
		// create separate object with trail for each line
		GameObject tr = Instantiate(trailPrefab,mouseHit,Quaternion.identity) as GameObject;
		currentTrailRendererObject = tr;
		tr.transform.position = mouseHit;		
		dragging = true;				
	}
	
	void MouseUp()
	{		
		newDotPosition = MousePoint();
		CreateBoxCollider(newDotPosition);
		dragging = false;
		lastPointExist = false;		
		currentTrailRendererObject.transform.position = lastDotPosition;
	}
	
	void CreateBoxCollider(Vector3 newDotPosition)
	{		       
        if (lastPointExist && wholeDistance < maxDistance)
        {
            GameObject colliderKeeper = new GameObject("collider");            
			BoxCollider bc = colliderKeeper.AddComponent<BoxCollider>();	
			float distance = Vector3.Distance(newDotPosition, lastDotPosition);
			colliderKeeper.transform.position = new Vector3((newDotPosition.x-lastDotPosition.x)/2+lastDotPosition.x, (newDotPosition.y-lastDotPosition.y)/2+lastDotPosition.y, 0);
            colliderKeeper.transform.LookAt(newDotPosition);
			if(distance < maxDistance-wholeDistance)
			{
            	bc.size = new Vector3( 0.3f,0.3f,  distance);			
			}
			else
			{
				bc.size = new Vector3( 0.3f,0.3f,  maxDistance-wholeDistance);
			}
			wholeDistance  +=  bc.size.z;			
        
		
			if(wholeDistance > maxDistance)
			{
				wholeDistance = maxDistance;
			}
        	lastDotPosition = newDotPosition;
	        lastPointExist = true;
		}
    }
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(50,50,70,50),"Start"))
		{
			ball.rigidbody.useGravity = !ball.rigidbody.useGravity;
		}
		if(GUI.Button(new Rect(50,100,70,50),"Restart"))
		{
			Application.LoadLevel(0);			
		}
		
		// bar
		float x = 100-wholeDistance*100/maxDistance;
		
		GUI.TextArea(new Rect(125,50,50,20), x.ToString());		
		
		GUI.BeginGroup(new Rect(Screen.width/2-100,2,200,20));
			GUI.Box(new Rect(0,0, 200,20), statusBarBorder);
			GUI.BeginGroup(new Rect(2,0, (int)x*2, 16));
				GUI.DrawTexture(new Rect(0,0,200,20), markerTexture, ScaleMode.ScaleAndCrop);
			GUI.EndGroup();
		GUI.EndGroup();
		
		
		
	}
	
}
