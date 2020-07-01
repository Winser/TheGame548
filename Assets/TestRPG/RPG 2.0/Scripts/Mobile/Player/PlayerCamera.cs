using UnityEngine;
using System.Collections;

/// <summary>
/// Player camera.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
	/// <summary>
	/// The instance of the camera.
	/// </summary>
	private static PlayerCamera instance;
	
	/// <summary>
	/// Gets the instance of the camera. We have only one PlayerCamera per scene
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>
	public static PlayerCamera Instance {
		get{ return instance;}
	}

	private Transform target;
	private float desiredDistance;
	
	/// <summary>
	/// The player tag to find the transform and set as target.
	/// </summary>
	public string playerTag = "Player";
	/// <summary>
	/// We can toogle first person camera and third person camera with this key
	/// </summary>
	public KeyCode switchKey;
	/// <summary>
	/// The distance at which we switch from third person camera to first person camera or the other way arround.
	/// </summary>
	public float switchDistance = 1.5f;
	/// <summary>
	/// Should we switch the camera on zoom?
	/// </summary>
	public bool switchOnZoom;
	[HideInInspector]
	public bool isInFirstPersonView;
	/// <summary>
	/// The first person camera data
	/// </summary>
	public FirstPersonCamera firstPersonCamera;
	/// <summary>
	/// The third person camera data.
	/// </summary>
	public ThirdPersonCamera thirdPersonCamera;
	
	private void Awake ()
	{
		//Initiaize this camera instance
		instance = this;
	}
	
	private void Start ()
	{
		//Try to find Target
		FindTarget ();
		
		//If camera has a rigidbody, freeze the rotation
		if (GetComponent<Rigidbody>()) {
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		
		//Initializes the thid person camera data
		thirdPersonCamera.Init (transform);
		//Initializes the first person camera data
		firstPersonCamera.Init (transform);
	}
	
	private void LateUpdate ()
	{
		//If our target is null, try to find it
		if (target == null) {
			FindTarget ();
			return;
		}
		
		if (isInFirstPersonView) {
			firstPersonCamera.Handle (target);
		} else {
			thirdPersonCamera.Handle (target);
		}
		
		SwitchCameraView ();
		
		if (shakeIntensity > 0) {
			transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;
			transform.rotation = new Quaternion (
                        originRotation.x + Random.Range (-shakeIntensity, shakeIntensity) * 0.2f,
                        originRotation.y + Random.Range (-shakeIntensity, shakeIntensity) * 0.2f,
                        originRotation.z + Random.Range (-shakeIntensity, shakeIntensity) * 0.2f,
                        originRotation.w + Random.Range (-shakeIntensity, shakeIntensity) * 0.2f);
			shakeIntensity -= shakeDecay;
		}
	}
	
	private void SwitchCameraView ()
	{
		
		#region MeshChange
		MeshHolder holder = target.GetComponent<MeshHolder> ();
		if (holder) {
			if (isInFirstPersonView) {
				holder.EnableFirstPerson ();
			} else {
				holder.EnableThirdPerson ();
			}
		}
		#endregion MeshChange
		
		#region KeySwitch
		if (Input.GetKeyUp (switchKey)) {
			SwitchCamera ();
			return;
		}
		#endregion KeySwitch
		
		if (!switchOnZoom) {
			return;
		}
		
		if (isInFirstPersonView) {
			desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * firstPersonCamera.zoomRate;
			desiredDistance = Mathf.Clamp (desiredDistance, 0, thirdPersonCamera.maxDistance);
			if (desiredDistance > 1.0f) {
				isInFirstPersonView = false;
				transform.parent = null;
				target.rotation = Quaternion.Euler (0, target.rotation.eulerAngles.y, 0);
				thirdPersonCamera.Reset (switchDistance + 1.2f, target);
				desiredDistance = 0;
				return;
			}
		}
		
		if (Vector3.Distance (transform.position, target.position + firstPersonCamera.firstPersonOffset) < switchDistance && !isInFirstPersonView) {
			//Debug.Log ("Switch because of distance " + Vector3.Distance (transform.position, target.position));
			isInFirstPersonView = true;
			transform.position = target.position + firstPersonCamera.firstPersonOffset;
			transform.parent = target.transform;
			transform.localRotation = Quaternion.Euler (Vector3.zero);
			
			//desiredDistance=0;
		}	
	}
	
	/// <summary>
	/// Toogle camera view
	/// </summary>
	public void SwitchCamera ()
	{
		isInFirstPersonView = !isInFirstPersonView;
		if (isInFirstPersonView) {
			transform.position = target.position + firstPersonCamera.firstPersonOffset;
			transform.parent = target.transform;
			transform.localRotation = Quaternion.Euler (Vector3.zero);

		} else {	
			transform.parent = null;
			target.rotation = Quaternion.Euler (0, target.rotation.eulerAngles.y, 0);
			thirdPersonCamera.Reset (switchDistance + 1.2f, target);
		}
		desiredDistance = 0;
	}
	
	/// <summary>
	/// Finds and sets the target if it exists
	/// </summary>
	public void FindTarget ()
	{
		//Find the gameObject with the player tag
		GameObject go = GameObject.FindGameObjectWithTag (playerTag);
		if (go != null) {
			//Set the gameObject with playerTag as out target
			target = go.transform;
		}
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
	
	private Vector3 originPosition;
	private Quaternion originRotation;
	private float shakeDecay;
	private float shakeIntensity;
	
	public void Shake (float intensity, float decay)
	{
		originPosition = transform.position;
		originRotation = transform.rotation;
		shakeIntensity = intensity;
		shakeDecay = decay;
	}

}

[System.Serializable]
public class FirstPersonCamera
{
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15;
	public float sensitivityY = 15;
	public float minimumX = -360;
	public float maximumX = 360;
	public float minimumY = -60;
	public float maximumY = 60;
	private float rotationY = 0;
	public float zoomRate = 40;
	public Vector3 firstPersonOffset;
	public bool hideCursor;
	private Transform mCamera;
	private float desiredDistance;
	
	public void Init (Transform mTransform)
	{
		mCamera = mTransform;
	}
	
	public void Handle (Transform target)
	{
		if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt) || Input.GetKey (KeyCode.AltGr)) {
			Screen.lockCursor = false;
			Cursor.visible = true;	
			return;
		} else {
			Screen.lockCursor = true;
			Cursor.visible = false;
		}
		
		if (axes == RotationAxes.MouseXAndY) {
			rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			target.localEulerAngles = new Vector3 (-rotationY, target.localEulerAngles.y, 0);
			target.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
			
		} else if (axes == RotationAxes.MouseX) {
			target.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
		} else {
			rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			mCamera.localEulerAngles = new Vector3 (-rotationY, mCamera.localEulerAngles.y, 0);
		}
	}
}

[System.Serializable]
public class ThirdPersonCamera
{
	public float targetHeight = 1.7f;
	public float distance = 5.0f;
	public float offsetFromWall = 0.1f;
	public float maxDistance = 20;
	public float minDistance = .6f;
	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;
	public int yMinLimit = -80;
	public int yMaxLimit = 80;
	public bool smoothRotation = true;
	public float smoothRotationSpeed = 6.0f;
	public bool autoRotation = true;
	public bool alwaysBehindPlayer;
	public float autoRotationSpeed = 3.0f;
	public bool smoothZoom = true;
	public float smoothZoomSpeed = 5.0f;
	public int zoomRate = 40;
	public LayerMask collisionLayers = -1;
	public float xDeg = 0.0f;
	public float yDeg = 0.0f;
	private float currentDistance;
	private float desiredDistance;
	private float correctedDistance;
	private float currentXDeg;
	private float currentYDeg;
	public int rotateButton = 0;
	[HideInInspector]
	public bool rotateWithoutPress;
	private Transform mCamera;
	public bool hideCursor;
	
	public void Init (Transform mTransform)
	{
		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;

		currentXDeg = xDeg;
		currentYDeg = yDeg;
		mCamera = mTransform;
	}
	
	public void Handle (Transform target)
	{
		
		if (GameManager.Player.Movement.controllerType.Equals (ThirdPersonMovement.ControllerType.MouseRotation)) {
			rotateWithoutPress = true;
		} else {
			rotateWithoutPress = false;
		}
		
		if (hideCursor) {
			Screen.lockCursor = true;
			Cursor.visible = false;
		} else {
			Screen.lockCursor = false;
			Cursor.visible = true;
		}
		Vector3 vTargetOffset;
		bool userRotate = false;
		if (Input.GetMouseButton (rotateButton) && !rotateWithoutPress) {
			currentXDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			currentYDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			userRotate = true;
		}
		
		if (rotateWithoutPress) {
			if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt) || Input.GetKey (KeyCode.AltGr)) {
				Screen.lockCursor = false;
				Cursor.visible = true;
				
			} else {
				if (!InterfaceContainer.Instance.aoeTarget.gameObject.activeSelf) {
					currentXDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
					currentYDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			
					if (GameManager.Player.IsMounted) {
						GameManager.Player.MountTransform.rotation = Quaternion.Lerp (GameManager.Player.MountTransform.rotation, Quaternion.Euler (new Vector3 (0, mCamera.eulerAngles.y, 0)), Time.deltaTime * 7);
					} else {
						GameManager.Player.transform.rotation = Quaternion.Lerp (GameManager.Player.transform.rotation, Quaternion.Euler (new Vector3 (0, mCamera.eulerAngles.y, 0)), Time.deltaTime * 7);
					}
					userRotate = true;
					Screen.lockCursor = true;
					Cursor.visible = false;
				} else {
					Screen.lockCursor = false;
					Cursor.visible = true;
				}
			}
		}
		
		
		if (Input.GetMouseButtonUp (2)) {
			alwaysBehindPlayer = !alwaysBehindPlayer;
		}
		
		if (autoRotation) {
			if (Input.GetAxis ("Vertical") != 0 && !userRotate) {
				currentXDeg = Mathf.LerpAngle (currentXDeg, target.eulerAngles.y, Time.smoothDeltaTime * autoRotationSpeed);
			}
		}
		
		if (alwaysBehindPlayer && !userRotate) {
			currentXDeg = Mathf.LerpAngle (currentXDeg, target.eulerAngles.y, Time.smoothDeltaTime * autoRotationSpeed);
		}
		
		if (smoothRotation) {
			xDeg = Mathf.Lerp (xDeg, currentXDeg, Time.deltaTime * smoothRotationSpeed);
			yDeg = Mathf.Lerp (yDeg, currentYDeg, Time.deltaTime * smoothRotationSpeed);
		} else {
			xDeg = currentXDeg;
			yDeg = currentYDeg;
		}
		
		yDeg = PlayerCamera.ClampAngle (yDeg, yMinLimit, yMaxLimit);
		
		Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0);
		desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
		desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);

		if (smoothZoom) {
			correctedDistance = Mathf.Lerp (correctedDistance, desiredDistance, Time.deltaTime * smoothZoomSpeed);
		} else {
			correctedDistance = desiredDistance;
		}

		vTargetOffset = new Vector3 (0, -targetHeight, 0);
		Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

		RaycastHit collisionHit;
		Vector3 trueTargetPosition = new Vector3 (target.position.x, target.position.y + targetHeight, target.position.z);

		bool isCorrected = false;
		if (Physics.Linecast (trueTargetPosition, position, out collisionHit, collisionLayers.value)) {
			correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall;
			isCorrected = true;
		}

       
		currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * smoothZoomSpeed) : correctedDistance;
		currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance);

		position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

		mCamera.rotation = rotation;
		mCamera.position = position;
	}
	
	public void Reset ()
	{
		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;
		currentXDeg = xDeg;
		currentYDeg = yDeg;
	}
	
	public void Reset (float dist, Transform target)
	{
		currentDistance = dist;
		desiredDistance = dist;
		correctedDistance = dist;
		xDeg = target.eulerAngles.y;
		yDeg = 9;
		currentXDeg = xDeg;
		currentYDeg = yDeg;
	}
}