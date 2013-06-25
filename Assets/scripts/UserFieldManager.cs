using UnityEngine;
using System.Collections;
using System;

public class UserFieldManager : MonoBehaviour
{
	
	// 0 - ball object creation
	// 1 - 2nd object to create
	// -1 - second obj
	public float smoothingSpeed = 0.01f;
	
	public GameObject brushPrefub;
	int creationMode = -1;
	public GameObject[] objectsToCreate;
	GameObject currentObject = null;
	float maxDistance;
	float wholeDistance;
	float minColliderLenght;
	public Material blockMaterial;
	PhysicMaterial blockPhysicsMaterial;
	float blockHeight = 0f;
	
	
	public PhysicMaterial colliderMeshMaterial;
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
	
	Vector3 colliderSpawn;
	
	// Use this for initialization
	void Start ()
	{
		maxDistance = 100;		
		wholeDistance = 0;
		minColliderLenght = 0.05f;
		ball = GameObject.Find ("ball");
		ball.rigidbody.useGravity = false;	
	}
	
	// Update is called once per frame
	void Update ()
	{						
		if (creationMode == -1) {
			DrawLine ();
		} else {			

		}
		

	}
	
	void CreateObject ()
	{			
//		if(currentObject != null)
//		{					
//			currentObject.transform.position = MousePoint();
//		if(Input.GetMouseButton(0) && buttonNotPressed == true)
//			{
//				currentObject = null;
//				creationMode = -2;
//			}
//		}
//		else
//		{		
		currentObject = (GameObject)Instantiate (objectsToCreate [creationMode], MousePoint (), Quaternion.identity);
//		}
	}
	
	void ObjectUpdate (bool b)
	{
		if (currentObject != null) {				
			currentObject.transform.position = MousePoint ();									
			if (b) {
				currentObject = null;
				creationMode = -2;			
			}				
		}
	}
		
	void createBoxWithoutFrontSide (Vector3 p1, Vector3 p2)
	{	
		
		//TODO: recalculate border. Add one more side.
		// 
		Vector3 topLeftFront = p1;
		Vector3 topRightFront = p2;
		Vector3 topLeftBack = p1;
		Vector3 topRightBack = p2;
				
		Vector3 bottomLeftFront;
		Vector3 bottomRightFront;
		// TODO: check
		Vector3 backLeft = p1;
		Vector3 backRight = p2;
		
		
		
		topRightFront.z = 0.5f;
		topLeftFront.z = 0.5f;
		topLeftBack.z = -0.5f;
		topRightBack.z = -0.5f;
		
		float l = Vector3.Magnitude (p1 - p2);		
		float b = Mathf.Sqrt (l * l + blockHeight * blockHeight);
		float x1 = l * blockHeight / b;
		float y1 = blockHeight * blockHeight / b;		
		
		bottomLeftFront = topLeftFront;
		bottomRightFront = topRightFront;
		
		/*	
		bottomLeftFront.y -= y1;
		bottomLeftFront.x -= x1;
		bottomRightFront.y -= y1;
		bottomRightFront.x -= x1;
		*/
		
		bottomLeftFront.y -= blockHeight;
		bottomRightFront.y -= blockHeight;
		
		backLeft = bottomLeftFront;
		backLeft.z = -0.5f;		
		backRight = bottomRightFront;
		backRight.z = -0.5f;
		
		
		GameObject newLedge = new GameObject ();
		Mesh newMesh = new Mesh ();
		newLedge.AddComponent<MeshFilter> ();
		newLedge.AddComponent<MeshRenderer> ();
		
		newMesh.vertices = new Vector3[] {topLeftFront, topRightFront, topLeftBack, topRightBack,
			bottomLeftFront, bottomRightFront, backLeft, backRight};
		
		Vector2[] uvs = new Vector2[newMesh.vertices.Length];
		for (int i = 0; i < uvs.Length; i++) {
			uvs [i] = new Vector2 (newMesh.vertices [i].x, newMesh.vertices [i].z);
		}		
		
		newMesh.uv = uvs;		
		newMesh.triangles = new int[] {5,4,6, 6,7,5};
		newMesh.RecalculateNormals ();		
		
		newLedge.GetComponent<MeshFilter> ().mesh = newMesh;
		addMaterial(newLedge);		
	}
	
	void createBox (Vector3 p1, Vector3 p2)
	{	
		
		//TODO: recalculate border. Add one more side.
		// 
		Vector3 topLeftFront = p1;
		Vector3 topRightFront = p2;
		Vector3 topLeftBack = p1;
		Vector3 topRightBack = p2;
				
		Vector3 bottomLeftFront;
		Vector3 bottomRightFront;
		// TODO: check
		Vector3 backLeft = p1;
		Vector3 backRight = p2;
		
		
		
		topRightFront.z = 0.5f;
		topLeftFront.z = 0.5f;
		topLeftBack.z = -0.5f;
		topRightBack.z = -0.5f;
		
		float l = Vector3.Magnitude (p1 - p2);		
		float b = Mathf.Sqrt (l * l + blockHeight * blockHeight);
		float x1 = l * blockHeight / b;
		float y1 = blockHeight * blockHeight / b;		
		
		bottomLeftFront = topLeftFront;
		bottomRightFront = topRightFront;
		
		/*	
		bottomLeftFront.y -= y1;
		bottomLeftFront.x -= x1;
		bottomRightFront.y -= y1;
		bottomRightFront.x -= x1;
		*/
		bottomLeftFront.y -= blockHeight;
		bottomRightFront.y -= blockHeight;
		
		backLeft = bottomLeftFront;
		backLeft.z = -0.5f;		
		backRight = bottomRightFront;
		backRight.z = -0.5f;
		
		
		GameObject newLedge = new GameObject ();
		Mesh newMesh = new Mesh ();
		newLedge.AddComponent<MeshFilter> ();
		newLedge.AddComponent<MeshRenderer> ();
		
		newMesh.vertices = new Vector3[] {topLeftFront, topRightFront, topLeftBack, topRightBack,
			bottomLeftFront, bottomRightFront, backLeft, backRight};
		
		Vector2[] uvs = new Vector2[newMesh.vertices.Length];
		for (int i = 0; i < uvs.Length; i++) {
			uvs [i] = new Vector2 (newMesh.vertices [i].x, newMesh.vertices [i].z);
		}		
		
		newMesh.uv = uvs;		
		newMesh.triangles = new int[] {5,4,0, 0,1,5, 0,2,3, 3,1,0, 5,4,6, 6,7,5};
		newMesh.RecalculateNormals ();		
		
		newLedge.GetComponent<MeshFilter> ().mesh = newMesh;
		addMaterial(newLedge);
	}
	
	void  addMaterial(GameObject go)
	{
		if (blockMaterial) 
		go.renderer.material = blockMaterial;
		go.AddComponent<MeshCollider> ();
		if (blockPhysicsMaterial) 
			go.GetComponent<MeshCollider> ().material = blockPhysicsMaterial;	
		go.GetComponent<MeshCollider> ().material  = colliderMeshMaterial;
		
		go.transform.parent = GameObject.Find ("UserMeshes").transform;
	}
	
	void DrawLine ()
	{
		if (maxDistance - wholeDistance > 0) {
			if (Input.GetMouseButtonDown (0)) {			
				if (!lastPointExist) {
					newDotPosition = MousePoint ();
					lastDotPosition = MousePoint ();				
					lastPointExist = true;				
				}
				MouseDown ();
			}		
			if (Input.GetMouseButtonUp (0)) {
				MouseUp ();
			}			
			if (dragging) {	
				newDotPosition = Vector3.Lerp(colliderSpawn, MousePoint (), smoothingSpeed);	
				colliderSpawn = newDotPosition;
				
				float colliderLengthBetweenFrames = Vector3.Distance (newDotPosition, lastDotPosition);								
				
				if (newDotPosition != lastDotPosition) {	
					if (colliderLengthBetweenFrames >= minColliderLenght) {
						//
						// split collider length
						int amountOfColliders = (int)(colliderLengthBetweenFrames / minColliderLenght);						
						
						float deltaXW = (newDotPosition.x - lastDotPosition.x);
						float deltaYW = (newDotPosition.y - lastDotPosition.y);
						float deltaXT = deltaXW * (colliderLengthBetweenFrames - minColliderLenght * amountOfColliders) / colliderLengthBetweenFrames;
						float deltaYT = deltaYW * (colliderLengthBetweenFrames - minColliderLenght * amountOfColliders) / colliderLengthBetweenFrames;					
						
						
						
						float deltaXPerCollider = (deltaXW - deltaXT) / amountOfColliders;
						float deltaYPerCollider = (deltaYW - deltaYT) / amountOfColliders;						
						
						for (int i = 0; i < amountOfColliders; i++) {
							//CreateBoxCollider (new Vector3 (lastDotPosition.x + deltaXPerCollider, lastDotPosition.y + deltaYPerCollider, 0));
							Vector3 nextPoint = new Vector3 (lastDotPosition.x + deltaXPerCollider, lastDotPosition.y + deltaYPerCollider, 0);
							if (nextPoint.x < lastDotPosition.x) {
								createBox (lastDotPosition, nextPoint);
								createBoxWithoutFrontSide (nextPoint, lastDotPosition);
								lastDotPosition = nextPoint;
							} else {
								createBox ( nextPoint, lastDotPosition);
								createBoxWithoutFrontSide ( lastDotPosition,nextPoint);
								lastDotPosition = nextPoint;
							}
						}
					}
						
					currentTrailRendererObject.transform.position = lastDotPosition;
				} else {
					currentTrailRendererObject.transform.position = newDotPosition;
				}
				
								
			}
		} else {
			if (dragging) {
				MouseUp ();
				currentTrailRendererObject.transform.position = lastDotPosition;
				dragging = false;
			}
		}
	}
	
	Vector3 MousePoint ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit ();
		Physics.Raycast (ray, out hit, 1000, Physics.kDefaultRaycastLayers);
		Vector3 rtn = hit.point;
		rtn.z = 0;
		return rtn;
	}
	
	void MouseDown ()
	{		
		Vector3 mouseHit;
		// receive cur mouse position
		mouseHit = MousePoint ();	
		
		// create separate object with trail for each line
		GameObject tr = Instantiate (trailPrefab, mouseHit, Quaternion.identity) as GameObject;
		currentTrailRendererObject = tr;
		tr.transform.position = mouseHit;		
		dragging = true;
		
		colliderSpawn = MousePoint();
	}
	
	void MouseUp ()
	{	
		newDotPosition = MousePoint ();
		if (newDotPosition.x > lastDotPosition.x) {
			createBox (lastDotPosition, newDotPosition);
			createBoxWithoutFrontSide (lastDotPosition, newDotPosition);
			lastDotPosition = newDotPosition;
		} else {
			createBox (newDotPosition, lastDotPosition);
			createBoxWithoutFrontSide (newDotPosition, lastDotPosition);
			lastDotPosition = newDotPosition;
		}
		
		//CreateBoxCollider (newDotPosition);
		dragging = false;
		lastPointExist = false;	
		try {
			currentTrailRendererObject.transform.position = lastDotPosition;
		} catch (NullReferenceException e) {
		}
	}
	
	void CreateBoxCollider (Vector3 newDotPosition)
	{		       
		if (lastPointExist && wholeDistance < maxDistance) {
			//GameObject colliderKeeper = new GameObject ("collider");            
			//BoxCollider bc = colliderKeeper.AddComponent<BoxCollider> ();	
			GameObject colliderKeeper = (GameObject)GameObject.Instantiate (brushPrefub);
			//SphereCollider bc = colliderKeeper.GetComponent<SphereCollider> ();
			BoxCollider bc = colliderKeeper.GetComponent<BoxCollider> ();
				
			float distance = Vector3.Distance (newDotPosition, lastDotPosition);
			colliderKeeper.transform.position = new Vector3 ((newDotPosition.x - lastDotPosition.x) / 2 + lastDotPosition.x, (newDotPosition.y - lastDotPosition.y) / 2 + lastDotPosition.y, 0);
			colliderKeeper.transform.LookAt (newDotPosition);
			if (distance < maxDistance - wholeDistance) {
				bc.size = new Vector3 (0.3f, 0.3f, distance);			
			} else {
				bc.size = new Vector3 (0.3f, 0.3f, maxDistance - wholeDistance);
			}
			wholeDistance += bc.size.z;			
        
		
			if (wholeDistance > maxDistance) {
				wholeDistance = maxDistance;
			}
			lastDotPosition = newDotPosition;
			lastPointExist = true;
		}
	}
	
	public void ChooseObjectToCreate (int _index)
	{	
		if (_index == creationMode) {					
			Destroy (currentObject);			
			currentObject = null;
			creationMode = -2;
		} else {			
			if (currentObject == null) {
				creationMode = _index;
			} else {							
				Destroy (currentObject);
				currentObject = null;				
				creationMode = _index;				
			}
		}		
		if (creationMode >= 0) {
			CreateObject ();
		}
	}
	
	void OnGUI ()
	{
		if (GUI.Button (new Rect (50, 50, 70, 50), "Start")) {
			ball.rigidbody.useGravity = !ball.rigidbody.useGravity;
		}
		if (GUI.Button (new Rect (50, 100, 70, 50), "Restart")) {
			Application.LoadLevel (0);			
		}
		
		// bar
		float x = 100 - wholeDistance * 100 / maxDistance;
		
		GUI.TextArea (new Rect (125, 50, 50, 20), x.ToString ());		
		
		GUI.BeginGroup (new Rect (Screen.width / 2 - 100, 2, 200, 20));
		GUI.Box (new Rect (0, 0, 200, 20), statusBarBorder);
		GUI.BeginGroup (new Rect (2, 0, (int)x * 2, 16));
		GUI.DrawTexture (new Rect (0, 0, 200, 20), markerTexture, ScaleMode.ScaleAndCrop);
		GUI.EndGroup ();
		GUI.EndGroup ();
		
		
		// creating objects buttons.
		if (GUI.Button (new Rect (Screen.width - 50, 50, 50, 50), "Ball")) {
			// create ball object				
			this.ChooseObjectToCreate (0);				
		} else if (GUI.Button (new Rect (Screen.width - 50, 100, 50, 50), "cube")) {			
			// create cube object
			this.ChooseObjectToCreate (1);
			
		} else if (GUI.Button (new Rect (Screen.width - 50, 150, 50, 50), "rotCube")) {
			// create rotated cube object			
			this.ChooseObjectToCreate (2);			
		} else if (Event.current.button == 0 && Event.current.isMouse) {
			ObjectUpdate (true);
		} else {
			ObjectUpdate (false);	
		}
		if (GUI.Button (new Rect (Screen.width - 50, 200, 50, 50), "Draw Line")) {					
			this.creationMode = -1;			
		}
	}
	
	
}
