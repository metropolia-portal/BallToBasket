using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {

	
	public float maxDistance = 200;
	float wholeDistance = 0f;
	 
	public Material blockMaterial;
	PhysicMaterial blockPhysicsMaterial;
	//LineRenderer lineRenderer;	
	
	public Transform DotPrefab;
	public GameObject trailPrefab;
	GameObject ball;
	GameObject currentTrailRendererObject;

	
	Vector3 mousePosition; // Vector3 because of using a raycast.
	Vector3 mouseHit;
	Vector3 newDotPosition;
	Vector3 lastDotPosition;
	
	
	bool dragging = false;
	bool lastPointExist = false;
	
	
	
	TrailRenderer trailRenderer;
	
	Vector3 mousePosForTrail;
	
	// Use this for initialization
	void Start () 
	{
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
				mousePosForTrail = MousePoint();			
				currentTrailRendererObject.transform.position = mousePosForTrail;		
				newDotPosition = mousePosForTrail;
				if(newDotPosition != lastDotPosition && Vector3.SqrMagnitude(newDotPosition - lastDotPosition) > 0.5f)
				{
					wholeDistance  +=  Vector3.SqrMagnitude(newDotPosition - lastDotPosition);				
					CreateBoxCollider(newDotPosition);				
				}			
			}
		}
		else
		{
			if (dragging)
			{
				MouseUp();
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
		wholeDistance  +=  Vector3.SqrMagnitude(newDotPosition - lastDotPosition);				
		CreateBoxCollider(newDotPosition);
		dragging = false;
		lastPointExist = false;		
	}
	
	void CreateBoxCollider(Vector3 newDotPosition)
	{
	
        Transform dot =(Transform) Instantiate(DotPrefab, newDotPosition, Quaternion.identity); //use random identity to make dots looks more different
        if (lastPointExist)
        {
            GameObject colliderKeeper = new GameObject("collider");
            BoxCollider bc = colliderKeeper.AddComponent<BoxCollider>();	
			
			colliderKeeper.transform.position = new Vector3((newDotPosition.x-lastDotPosition.x)/2+lastDotPosition.x, (newDotPosition.y-lastDotPosition.y)/2+lastDotPosition.y, 0);
            colliderKeeper.transform.LookAt(newDotPosition);			
            bc.size = new Vector3( 0.3f,0.3f, Vector3.Distance(newDotPosition, lastDotPosition) );
        }
        lastDotPosition = newDotPosition;
        lastPointExist = true;
    }
	
	/*void createBoxWithoutFrontSide(Vector3 p1, Vector3 p2)
	{	
		
		//TODO: recalculate border. Add one more side.
		
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
		
//		float l = Vector3.Magnitude(p1-p2);		
//		float b = Mathf.Sqrt(l*l + blockHeight*blockHeight);
//		float x1 = l*blockHeight/b;
//		float y1 = blockHeight*blockHeight/b;		
		
		bottomLeftFront = topLeftFront;
		bottomRightFront = topRightFront;
		
		
		bottomLeftFront.y -= y1;
		bottomLeftFront.x -= x1;
		bottomRightFront.y -= y1;
		bottomRightFront.x -= x1;
		
		
		bottomLeftFront.y -= blockHeight;
		bottomRightFront.y -= blockHeight;
		
		backLeft = bottomLeftFront;
		backLeft.z = -0.5f;		
		backRight = bottomRightFront;
		backRight.z = -0.5f;
		
		
		GameObject newLedge = new GameObject();
		Mesh newMesh = new Mesh();
		newLedge.AddComponent<MeshFilter>();
		newLedge.AddComponent<MeshRenderer>();
		
		newMesh.vertices = new Vector3[] {topLeftFront, topRightFront, topLeftBack, topRightBack,
			bottomLeftFront, bottomRightFront, backLeft, backRight};
		
		Vector2[] uvs = new Vector2[newMesh.vertices.Length];
		for(int i = 0; i < uvs.Length; i++)
		{
			uvs[i] = new Vector2(newMesh.vertices[i].x, newMesh.vertices[i].z);
		}		
		
		newMesh.uv = uvs;		
		newMesh.triangles = new int[] {5,4,6, 6,7,5};
		newMesh.RecalculateNormals();		
		
		newLedge.GetComponent<MeshFilter>().mesh = newMesh;
		if(blockMaterial) 
			newLedge.renderer.material = blockMaterial;
		newLedge.AddComponent<MeshCollider>();
		if(blockPhysicsMaterial) 
			newLedge.GetComponent<MeshCollider>().material=blockPhysicsMaterial;
		
	}
	
	
	void createBox(Vector3 p1, Vector3 p2)
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
		
//		float l = Vector3.Magnitude(p1-p2);		
//		float b = Mathf.Sqrt(l*l + blockHeight*blockHeight);
//		float x1 = l*blockHeight/b;
//		float y1 = blockHeight*blockHeight/b;		
		
		bottomLeftFront = topLeftFront;
		bottomRightFront = topRightFront;
		
			
		bottomLeftFront.y -= y1;
		bottomLeftFront.x -= x1;
		bottomRightFront.y -= y1;
		bottomRightFront.x -= x1;
		
		bottomLeftFront.y -= blockHeight;
		bottomRightFront.y -= blockHeight;
		
		backLeft = bottomLeftFront;
		backLeft.z = -0.5f;		
		backRight = bottomRightFront;
		backRight.z = -0.5f;
		
		
		GameObject newLedge = new GameObject();
		Mesh newMesh = new Mesh();
		newLedge.AddComponent<MeshFilter>();
		newLedge.AddComponent<MeshRenderer>();
		
		newMesh.vertices = new Vector3[] {topLeftFront, topRightFront, topLeftBack, topRightBack,
			bottomLeftFront, bottomRightFront, backLeft, backRight};
		
		Vector2[] uvs = new Vector2[newMesh.vertices.Length];
		for(int i = 0; i < uvs.Length; i++)
		{
			uvs[i] = new Vector2(newMesh.vertices[i].x, newMesh.vertices[i].z);
		}		
		
		newMesh.uv = uvs;		
		newMesh.triangles = new int[] {5,4,0, 0,1,5, 0,2,3, 3,1,0, 5,4,6, 6,7,5};
		newMesh.RecalculateNormals();		
		
		newLedge.GetComponent<MeshFilter>().mesh = newMesh;
		if(blockMaterial) 
			newLedge.renderer.material = blockMaterial;
		newLedge.AddComponent<MeshCollider>();
		if(blockPhysicsMaterial) 
			newLedge.GetComponent<MeshCollider>().material=blockPhysicsMaterial;
		
	}	
	*/
	
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
		float x = 100-wholeDistance*100/maxDistance;
		
		GUI.TextArea(new Rect(125,50,50,20), x.ToString());		
		
	}
	
}
