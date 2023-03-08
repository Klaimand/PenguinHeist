using UnityEngine;

public class SplitScreen : MonoBehaviour {

	/*Reference both the transforms of the two players on screen.
	Necessary to find out their current positions.*/
	[Header("Players Transform")]
	[SerializeField] private Transform player1;
	[SerializeField] private Transform player2;
	
	// SplitScreen
	[Space][Tooltip("The distance at which the splitscreen will be activated.")][Range(5f, 15f)]
	[SerializeField] private float splitDistance = 5;
	
	//The color and width of the splitter which splits the two screens up.
	[Space][Header("Players Transform")]
	[SerializeField] private Color splitterColor;
	[SerializeField] private float splitterWidth;
	
	//The two cameras, both of which are initalized/referenced in the start function.
	[Space][Header("Cameras + Settings")]
	[SerializeField] private GameObject camera1;
	[SerializeField] private GameObject camera2;
	[SerializeField] private int twoCamsLerpSpeed = 15;
	[SerializeField] private int oneCamsLerpSpeed = 15;
	public float yOffset = 8;
	public float zOffset = 0.1f;
	public float cameraAngle = 85;
	public Vector3 player1OffSetCam;
	public Vector3 player2OffSetCam;
	[SerializeField] private float offSetByStickMultiplier;
	
	//The two quads used to draw the second screen, both of which are initalized in the start function.
	private GameObject splitter;
	private GameObject split;
	private bool isSplit;
	
	

	void Start ()
	{
		splitter = transform.GetChild(0).gameObject;
		split = splitter.transform.GetChild(0).gameObject;
		
		Camera c1 = camera1.GetComponent<Camera>();
		Camera c2 = camera2.GetComponent<Camera>();
		c2.depth = c1.depth - 1;
		c2.cullingMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));
		c1.transform.rotation = c2.transform.rotation = Quaternion.Euler(cameraAngle,0,0);
		
		AssignMaterialAndShader();
		isSplit = false;
	}
	
	void LateUpdate () 
	{
		//Gets the z axis distance between the two players and just the standard distance.
		float zDistance = player1.position.z - player2.transform.position.z;
		float distance = Vector3.Distance (player1.position, player2.transform.position);

		//Sets the angle of the player up, depending on who's leading on the x axis.
		float angle;
		if (player1.transform.position.x <= player2.transform.position.x) angle = Mathf.Rad2Deg * Mathf.Acos (zDistance / distance);
		else angle = Mathf.Rad2Deg * Mathf.Asin (zDistance / distance) - 90;
		
		//Rotates the splitter according to the new angle.
		splitter.transform.localEulerAngles = new Vector3 (0, 0, angle);

		//Gets the exact midpoint between the two players.
		Vector3 midPoint = new Vector3 ((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, (player1.position.z + player2.position.z) / 2); 

		//Waits for the two cameras to split and then calcuates a midpoint relevant to the difference in position between the two cameras.
		if (distance > splitDistance)
		{
			isSplit = true;
			Vector3 offset = midPoint - player1.position; 
			offset.x = Mathf.Clamp(offset.x,-splitDistance/2,splitDistance/2);
			//offset.y = Mathf.Clamp(offset.y,-splitDistance/2,splitDistance/2);
			offset.z = Mathf.Clamp(offset.z,-splitDistance/2,splitDistance/2);
			midPoint = player1.position + offset; 

			Vector3 offset2 = midPoint - player2.position; 
			offset2.x = Mathf.Clamp(offset.x,-splitDistance/2,splitDistance/2);
			//offset2.y = Mathf.Clamp(offset.y,-splitDistance/2,splitDistance/2);
			offset2.z = Mathf.Clamp(offset.z,-splitDistance/2,splitDistance/2);
			Vector3 midPoint2 = player2.position - offset;

			//Sets the splitter and camera to active and sets the second camera position as to avoid lerping continuity errors.
			if (!splitter.activeSelf) // Si splitter est désactivé
			{
				splitter.SetActive (true);
				camera2.SetActive (true);
				camera2.transform.position = camera1.transform.position;
				//camera2.transform.rotation = camera1.transform.rotation;
			} 
			else 
			{
				camera2.transform.position = Vector3.Lerp(camera2.transform.position,midPoint2 + new Vector3(0, yOffset, zOffset) 
					+ new Vector3(player2OffSetCam.x, 0, player2OffSetCam.z),
					Time.deltaTime * twoCamsLerpSpeed);
				
				//Lerps the second cameras position and rotation to that of the second midpoint, so relative to the second player.
				//Quaternion newRot2 = Quaternion.LookRotation(midPoint2 - camera2.transform.position);
				//camera2.transform.rotation = Quaternion.Lerp(camera2.transform.rotation, Quaternion.Euler(cameraAngle, newRot2.y, newRot2.z), Time.deltaTime * 15f);
			}
		} 
		else
		{
			isSplit = false;
			if (splitter.activeSelf) splitter.SetActive (false);
			camera2.SetActive (false);
		}

		camera1.transform.position = isSplit ? 
			Vector3.Lerp(camera1.transform.position,midPoint + new Vector3(0,yOffset,zOffset) + new Vector3(player1OffSetCam.x, 0, player1OffSetCam.z),Time.deltaTime * twoCamsLerpSpeed) : 
			Vector3.Lerp(camera1.transform.position,midPoint + new Vector3(0,yOffset,zOffset) * offSetByStickMultiplier,Time.deltaTime * oneCamsLerpSpeed);
		
		//Quaternion newRot = Quaternion.LookRotation(midPoint-camera1.transform.position);
		//camera1.transform.rotation = Quaternion.Lerp(camera1.transform.rotation, Quaternion.Euler(cameraAngle, newRot.y, newRot.z), Time.deltaTime * 15f);
		/*camera1.transform.rotation = Quaternion.Euler(cameraAngle, camera1.transform.rotation.y, camera1.transform.rotation.z);
		camera2.transform.rotation = Quaternion.Euler(cameraAngle, camera2.transform.rotation.y, camera2.transform.rotation.z);*/
		
	}
	
	private void AssignMaterialAndShader()
	{
		//Creates both temporary materials required to create the splitscreen.
		Material tempMat = new Material(Shader.Find("Unlit/Color"));
		tempMat.color = splitterColor;
		splitter.GetComponent<Renderer>().material = tempMat;
		splitter.GetComponent<Renderer>().sortingOrder = 2;
		splitter.layer = LayerMask.NameToLayer("TransparentFX");
		Material tempMat2 = new Material(Shader.Find("Mask/SplitScreen"));
		split.GetComponent<Renderer>().material = tempMat2;
		split.layer = LayerMask.NameToLayer("TransparentFX");
	}

	private void CreateSplitCameraMask()
	{
		split = GameObject.CreatePrimitive(PrimitiveType.Quad);
		split.name = $"Camera 2 texture rendered";
		split.transform.parent = splitter.transform;
		split.transform.localPosition = new Vector3(0, -(1 / (splitterWidth / 10)), 0.0001f); // Add a little bit of Z-distance to avoid clipping with splitter
		split.transform.localScale = new Vector3(1, 2 / (splitterWidth / 10), 1);
		split.transform.localEulerAngles = Vector3.zero;
	}

	private void CreateSpliterLine()
	{
		splitter = GameObject.CreatePrimitive(PrimitiveType.Quad);
		splitter.name = $"Splitter line";
		splitter.transform.parent = gameObject.transform;
		splitter.transform.localPosition = Vector3.forward;
		splitter.transform.localScale = new Vector3(2.5f, splitterWidth / 10, 1);
		splitter.transform.localEulerAngles = Vector3.zero;
		splitter.SetActive(false);
	}
}
