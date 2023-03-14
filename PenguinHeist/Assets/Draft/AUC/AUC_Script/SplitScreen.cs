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
	[Space][Header("Splitter Section")]
	[SerializeField] private Color splitterColor;
	[SerializeField] private float splitterWidth;
	[SerializeField] private AnimationCurve splitterWidthOnDistance;
	
	//The two cameras, both of which are initalized/referenced in the start function.
	[Space][Header("Cameras + Settings")]
	[SerializeField] private GameObject camera1;
	[SerializeField] private GameObject camera2;
	public float yOffset = 8;
	public float zOffset = 0.1f;
	public float cameraAngle = 85;
	
	[SerializeField] private int twoCamsLerpSpeed = 15;
	[SerializeField] private int oneCamsLerpSpeed = 15;
	[SerializeField] private float offSetByStickMultiplier = 1.05f;
	[HideInInspector] public Vector3 player1OffSetCam;
	[HideInInspector] public Vector3 player2OffSetCam;
	
	//The two quads used to draw the second screen, both of which are initalized in the start function.
	private GameObject splitter;
	private GameObject split;
	private bool isSplit;
	public float distance;
	
	void Start ()
	{
		splitter = transform.GetChild(0).gameObject;
		split = splitter.transform.GetChild(0).gameObject;
		isSplit = false;
		
		Camera c1 = camera1.GetComponent<Camera>();
		Camera c2 = camera2.GetComponent<Camera>();
		c2.depth = c1.depth - 1;
		c2.cullingMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));
		c1.transform.rotation = c2.transform.rotation = Quaternion.Euler(cameraAngle,0,0);
		
		AssignMaterialAndShader();
	}
	
	void FixedUpdate() 
	{
		//Gets the z axis distance between the two players and just the standard distance.
		float zDistance = player1.position.z - player2.transform.position.z;
		distance = Vector3.Distance (player1.position, player2.position);

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

			player1OffSetCam = player1.forward.normalized * offSetByStickMultiplier;
			player2OffSetCam = player2.forward.normalized * offSetByStickMultiplier;
			
			if (!splitter.activeSelf) // Si splitter est désactivé
			{
				splitter.SetActive (true);
				camera2.SetActive (true);
				camera2.transform.position = camera1.transform.position;
			} 
			else // Sinon
			{
				camera2.transform.position = Vector3.Lerp(camera2.transform.position,midPoint2 + new Vector3(0, yOffset, zOffset) 
					+ new Vector3(player2OffSetCam.x, 0, player2OffSetCam.z),
					Time.deltaTime * twoCamsLerpSpeed);
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
			Vector3.Lerp(camera1.transform.position,midPoint + new Vector3(0,yOffset,zOffset),Time.deltaTime * oneCamsLerpSpeed);
		
		SetSplitterLineWidth();
	}
	
	private void AssignMaterialAndShader()
	{
		//Creates both temporary materials required to create the splitscreen.
		//Material tempMat = new Material(Shader.Find("Unlit/Color"));
		//tempMat.color = splitterColor;
		//splitter.GetComponent<Renderer>().material = tempMat;
		//Material tempMat2 = new Material(Shader.Find("Mask/SplitScreen"));
		//split.GetComponent<Renderer>().material = tempMat2;
		
		splitter.GetComponent<Renderer>().sortingOrder = 2;
		splitter.layer = LayerMask.NameToLayer("TransparentFX");
		split.layer = LayerMask.NameToLayer("TransparentFX");
	}

	private void SetSplitterLineWidth()
	{
		splitterWidth = splitterWidthOnDistance.Evaluate(distance) / 10;
		splitter.transform.localScale = new Vector3(2.5f, splitterWidth, 1);
	}

	/*private void CreateSpliterLine()
	{
		splitter = GameObject.CreatePrimitive(PrimitiveType.Quad);
		splitter.name = $"Splitter line";
		splitter.transform.parent = gameObject.transform;
		splitter.transform.localPosition = Vector3.forward;
		splitter.transform.localScale = new Vector3(2.5f, splitterWidth / 10, 1);
		splitter.transform.localEulerAngles = Vector3.zero;
		splitter.SetActive(false);
	}*/
}
