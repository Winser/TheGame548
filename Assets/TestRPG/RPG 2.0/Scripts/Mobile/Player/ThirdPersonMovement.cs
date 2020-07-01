/// <summary>
/// This is a very messy class, i need to clean up this class...
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public enum MouseButton
{
	Left,
	Right,
	Middle,
}

public static class MouseButtonExtension
{
	public static int GetButton (this MouseButton button)
	{
		switch (button) {
		case MouseButton.Left:
			return 0;
		case MouseButton.Right:
			return 1;
		case MouseButton.Middle:
			return 2;
		default:
			return -1;
		}
	}
}

public enum CharacterState
{
	Idle,
	WalkForward,
	WalkBackward,
	RunForward,
	RunBackward,
	WalkLeft,
	WalkRight,
	RunLeft,
	RunRight,
	Attack,
	Jump,
	GetHit
}

[AddComponentMenu("RPG Kit 2.0/ThirdPersonMovement")]
public class ThirdPersonMovement : Photon.MonoBehaviour
{
	
	
	public enum ControllerType
	{
		Normal,
		ClickToMove,
		MouseRotation
		
	}
	
	public Character character;
	private CharacterController controller;
	private Vector3 downDirection = Vector3.zero;
	[HideInInspector]
	public CharacterState curState = CharacterState.Idle;
	public bool runningMode = false;
	[HideInInspector]
	public bool killInput;
	[HideInInspector]
	public bool autoAttack = false;
	//[HideInInspector]
//	public MouseButton mouseButton;
//	private bool onMouseTrigger;
	public ControllerType controllerType;
	//[HideInInspector]
	//public Vector3 moveDirection;
	public Animation playerAnimation;
	public bool canEvade;
	[HideInInspector]
	public Animation mountAnimation;
	public ClickToMove clickToMove;
	
	private void Start ()
	{
		controller = GetComponent<CharacterController> ();
//		moveDirection = transform.position;
		SetupEvadeAnimations ();
		clickToMove.Init (transform);
		character = GameManager.Player.Character;
		
		playerAnimation[character.die.name].wrapMode=WrapMode.ClampForever;
		playerAnimation[character.die.name].layer=6;
	}
	
	void Update ()
	{
		if (!photonView.isMine) {
			return;
		}
		if (killInput) {
			playerAnimation.Play (character.idle.name);
			if (!controllerType.Equals (ControllerType.ClickToMove)) {
				ApplyGravity ();
			}
			return;
		}
		
		if (levelWasLoaded) {
			levelWasLoaded = false;
			Stop ();
			GameManager.Player.Target = null;
			GameManager.Player.TargetBehaviour = null;
		}
		ToogleRunMode ();
		float curSpeed = 0.0f;
		float normalizedRotationSpeed = Input.GetAxis ("Horizontal");
		float normalizedMovementSpeed = Input.GetAxis ("Vertical");
		Vector3 direction = transform.TransformDirection (Vector3.forward);
		
		switch (controllerType) {
		case ControllerType.ClickToMove:
			#region ClickToMove
			clickToMove.HandleMovement (runningMode, ref autoAttack, HandleCharacterState);
			ApplyGravity ();
			#endregion ClickToMove
			break;
		case ControllerType.MouseRotation:
			#region MouseRotated
			
			if (canEvade) {
				Evade ();
			}
			
			if (normalizedMovementSpeed > 0.3f || normalizedMovementSpeed < -0.3f) {
				if (normalizedMovementSpeed < -0.1f) {
					curState = runningMode ? CharacterState.RunBackward : CharacterState.WalkBackward;
				} else {
					curState = runningMode ? CharacterState.RunForward : CharacterState.WalkForward;
				}
			} else {
				if (normalizedRotationSpeed > 0.3f || normalizedRotationSpeed < -0.3f) {
					if (normalizedRotationSpeed < -0.1f) {
						curState = runningMode ? CharacterState.RunLeft : CharacterState.WalkLeft;
					} else {
						curState = runningMode ? CharacterState.RunRight : CharacterState.WalkRight;
					}
					direction = transform.TransformDirection (-Vector3.left);
				} else {
					curState = CharacterState.Idle;
				}
			}
      
			Jump ();
			ApplyGravity ();
			curSpeed = HandleCharacterState (curState) * ((curState.Equals (CharacterState.WalkLeft) || curState.Equals (CharacterState.WalkRight) || curState.Equals (CharacterState.RunLeft) || curState.Equals (CharacterState.RunRight)) ? normalizedRotationSpeed : normalizedMovementSpeed);
			controller.SimpleMove (direction * curSpeed);
	
			#endregion MouseRotated
			break;
		default:
			#region Normal
			
			if (canEvade) {
				Evade ();
			}
			
			if (normalizedRotationSpeed > 0.3f || normalizedRotationSpeed < -0.3f) {
				Yaw (Time.smoothDeltaTime * character.characterMotor.rotationSpeed * normalizedRotationSpeed);
			}
		
			if (normalizedMovementSpeed > 0.3f || normalizedMovementSpeed < -0.3f) {
				if (normalizedMovementSpeed < -0.1f) {
					curState = runningMode ? CharacterState.RunBackward : CharacterState.WalkBackward;
				} else {
					curState = runningMode ? CharacterState.RunForward : CharacterState.WalkForward;
				}
			} else {
				curState = CharacterState.Idle;
			}
      
			Jump ();
			ApplyGravity ();
			curSpeed = HandleCharacterState (curState) * normalizedMovementSpeed;
			controller.SimpleMove (direction * curSpeed);  
			#endregion Normal
			break;
		}
	}
	
	public float HandleCharacterState (CharacterState state)
	{
		if(curState != state){
			curState=state;
		}
		switch (state) {
		case CharacterState.Idle:
			if (mountAnimation != null) {
				mountAnimation.CrossFade (character.idle.name);
			}
			playerAnimation.CrossFade (character.idle.name);
			return 0;
		case CharacterState.WalkForward:
			if (mountAnimation != null) {
				mountAnimation [character.walk.name].speed = 1;
				mountAnimation.CrossFade (character.walk.name);
			}
			playerAnimation [character.walk.name].speed = 1;
			playerAnimation.CrossFade (character.walk.name);
			return character.characterMotor.walkSpeed;
		case CharacterState.WalkBackward:
			if (mountAnimation != null) {
				mountAnimation [character.walk.name].speed = -1;
				mountAnimation.CrossFade (character.walk.name);
			}
			
			playerAnimation [character.walk.name].speed = -1;
			playerAnimation.CrossFade (character.walk.name);
			
			return character.characterMotor.walkBackSpeed;
		case CharacterState.WalkRight:
			playerAnimation.CrossFade (character.walkRight.name);
			
			return character.characterMotor.walkRightSpeed;
		case CharacterState.WalkLeft:
			playerAnimation.CrossFade (character.walkLeft.name);
			
			return character.characterMotor.walkLeftSpeed;
		case CharacterState.RunForward:
			if (mountAnimation != null) {
				mountAnimation [character.run.name].speed = 1;
				mountAnimation.CrossFade (character.run.name);
			}
			playerAnimation [character.run.name].speed = 1;
			playerAnimation.CrossFade (character.run.name);
		
			return character.characterMotor.runSpeed;
		case CharacterState.RunBackward:
			if (mountAnimation != null) {
				mountAnimation [character.run.name].speed = -1;
				mountAnimation.CrossFade (character.run.name);
			}
			playerAnimation [character.run.name].speed = -1;
			playerAnimation.CrossFade (character.run.name);
			return character.characterMotor.runBackSpeed;
		case CharacterState.RunLeft:
			playerAnimation.CrossFade (character.runLeft.name);
			return character.characterMotor.runLeftSpeed;
		case CharacterState.RunRight:
			playerAnimation.CrossFade (character.runRight.name);
			return character.characterMotor.runRightSpeed;
		case CharacterState.Jump:
			downDirection.y = character.characterMotor.jumpHeight;
			if (mountAnimation != null) {
				mountAnimation [character.jump.name].layer = 3;
				mountAnimation.CrossFade (character.jump.name);
			}
			
			playerAnimation [character.jump.name].layer = 3;
			playerAnimation.CrossFade (character.jump.name);
			if (!PhotonNetwork.offlineMode) {
				photonView.RPC ("PlayAnimation", PhotonTargets.Others, character.jump.name);
			}
			return character.characterMotor.jumpHeight;
		case CharacterState.Attack:
			return 0;
		case CharacterState.GetHit:
			if (mountAnimation != null) {
				mountAnimation.CrossFade (character.getHit.name);
			}
			playerAnimation.CrossFade (character.getHit.name);
			
			return 0;
		default:
			return 0;
		}
	}

	private void Jump ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !playerAnimation.IsPlaying (character.jump.name)) {
			curState = CharacterState.Jump;
		}
	}
   
	public void ApplyGravity ()
	{
		if (!controller.isGrounded) {
			downDirection.y -= (character.characterMotor.gravity) * Time.deltaTime;
		}
		downDirection.y = Mathf.Clamp (downDirection.y, -5, character.characterMotor.jumpHeight);
		controller.Move (downDirection * Time.deltaTime);
	}
	
	public void Yaw (float angles)
	{
		transform.RotateAround (Vector3.up, angles);
	}
	
	private void ToogleRunMode ()
	{
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			runningMode = !runningMode; 
		}
	}
	
	public void SetAnimationLayer(string anim, int layer){
		playerAnimation[anim].layer=layer;
		if (!PhotonNetwork.offlineMode) {
			photonView.RPC ("SetAnimationLayer", PhotonTargets.Others, anim, layer);
		}
	}
	
	public void PlayAnimation (string anim, float killInput)
	{
		StopCoroutine ("KillInput");
		StartCoroutine ("KillInput", killInput);
		playerAnimation.CrossFade (character.idle.name); // We play idle first to get a smooth transition
		if (playerAnimation [anim] != null) {
			
			playerAnimation [anim].layer = 3;
			playerAnimation.CrossFade (anim);
			if (!PhotonNetwork.offlineMode) {
				photonView.RPC ("PlayAnimation", PhotonTargets.Others, anim);
			}
		}
	}
	
	
	public IEnumerator PlayAnimation (List<AnimationClip> anim, float delay, float killInput, float animationSpeed)
	{
		yield return new WaitForSeconds(delay);
		StopCoroutine ("KillInput");
		StartCoroutine ("KillInput", killInput);	
		playerAnimation.CrossFade (character.idle.name); // We play idle first to get a smooth transition
		
		for (int i=0; i<anim.Count; i++) {
			
			playerAnimation [anim [i].name].speed = animationSpeed;
			playerAnimation [anim [i].name].layer = 3;
			playerAnimation.CrossFade (anim[i].name);
			if (!PhotonNetwork.offlineMode) {
				photonView.RPC ("PlayAnimation", PhotonTargets.Others, anim[i].name);
			}
			yield return new WaitForSeconds(anim[i].length-anim[i].length*0.1f);
		}
	}
	
	public void PlayAnimation (string anim, float killInput, float animationSpeed)
	{
		StopCoroutine ("KillInput");
		StartCoroutine ("KillInput", killInput);
		playerAnimation.CrossFade (character.idle.name); // We play idle first to get a smooth transition
		if (playerAnimation [anim] != null) {
			playerAnimation [anim].speed = animationSpeed;
			playerAnimation [anim].layer = 3;
			playerAnimation.CrossFade (anim);
			if (!PhotonNetwork.offlineMode) {
				photonView.RPC ("PlayAnimation", PhotonTargets.Others, anim, animationSpeed);
			}
		}
	}
	
	public void PlayAnimation (string anim, float killInput, int layer)
	{
		StopCoroutine ("KillInput");
		StartCoroutine ("KillInput", killInput);
		playerAnimation.CrossFade (character.idle.name); // We play idle first to get a smooth transition
		if (playerAnimation [anim] != null) {
			
			playerAnimation [anim].layer = layer;
			playerAnimation.CrossFade (anim);
			if (!PhotonNetwork.offlineMode) {
				photonView.RPC ("PlayAnimation", PhotonTargets.Others, anim);
			}
		}
	}
	
	private float dodgeBackTapTime = 0.0f;
	private float dodgeBackTapSpeed = 0.5f;
	private float dodgeLeftTapTime = 0.0f;
	private float dodgeLeftTapSpeed = 0.5f;
	private float dodgeRightTapTime = 0.0f;
	private float dodgeRightTapSpeed = 0.5f;

	private void Evade ()
	{
		if (Input.GetKeyDown ("s")) {
			if ((Time.time - dodgeBackTapTime) < dodgeBackTapSpeed && character.dodgeBack != null) {
				playerAnimation.CrossFade (character.dodgeBack.name);
				if (!PhotonNetwork.offlineMode) {
					photonView.RPC ("PlayAnimation", PhotonTargets.Others, character.dodgeBack.name);
				}
				StartCoroutine (KillInput (character.dodgeBack.length));
			}
			dodgeBackTapTime = Time.time;
		}
		
		if (Input.GetKeyDown ("a")) {
			if ((Time.time - dodgeLeftTapTime) < dodgeLeftTapSpeed && character.dodgeLeft != null) {
				playerAnimation.CrossFade (character.dodgeLeft.name);
				if (!PhotonNetwork.offlineMode) {
					photonView.RPC ("PlayAnimation", PhotonTargets.Others, character.dodgeLeft.name);
				}
				StartCoroutine (KillInput (character.dodgeLeft.length));
			}
			dodgeLeftTapTime = Time.time;
		}
	
		if (Input.GetKeyDown ("d")) {
			if ((Time.time - dodgeRightTapTime) < dodgeRightTapSpeed && character.dodgeRight != null) {
				playerAnimation.CrossFade (character.dodgeRight.name);
				if (!PhotonNetwork.offlineMode) {
					photonView.RPC ("PlayAnimation", PhotonTargets.Others, character.dodgeRight.name);
				}
				StartCoroutine (KillInput (character.dodgeRight.length));
			}
			dodgeRightTapTime = Time.time;
		}
		
	}

	public void SetupEvadeAnimations ()
	{
		if (character.dodgeBack != null) {
			playerAnimation [character.dodgeBack.name].layer = 1;
			playerAnimation [character.dodgeBack.name].wrapMode = WrapMode.Once;
		}
		
		if (character.dodgeLeft != null) {
			playerAnimation [character.dodgeLeft.name].layer = 1;
			playerAnimation [character.dodgeLeft.name].wrapMode = WrapMode.Once;
		}
		
		if (character.dodgeRight != null) {
			playerAnimation [character.dodgeRight.name].layer = 1;
			playerAnimation [character.dodgeRight.name].wrapMode = WrapMode.Once;
		}
	}
	
	private IEnumerator KillInput (float delay)
	{
		killInput = true;
		yield return new WaitForSeconds(delay);
		killInput = false;
	}
	
	public void Stop ()
	{
		clickToMove.moveDirection = transform.position;
		autoAttack = false;
	}
		
	private bool levelWasLoaded = false;

	private void OnLevelWasLoaded (int level)
	{
		levelWasLoaded = true;
		Stop ();
	}
}

[System.Serializable]
public class ClickToMove
{
	public bool usePathFinder;
	public MouseButton button;
	private Transform transform;
	private float searchInterval = 0.4f;
	private float nextSearch;
	private Seeker seeker;
	private CharacterController controller;
	public Path path;
	public float nextWaypointDistance = 0.5f;
	private int currentWaypoint = 0;
	[HideInInspector]
	public Vector3 moveDirection;
	
	public void Init (Transform trans)
	{
		this.transform = trans;
		moveDirection = trans.position;
		seeker = trans.GetComponent<Seeker> ();
		controller = trans.GetComponent<CharacterController> ();
	}
	
	public void HandleMovement (bool running, ref bool autoAttack, System.Func<CharacterState, float> handleCharacterState)
	{
		
		if (Input.GetMouseButton (button.GetButton ()) && Time.time > nextSearch) {
			if (InterfaceContainer.Instance.UIClick () || InterfaceContainer.Instance.isDragging) {
				handleCharacterState (CharacterState.Idle);
				return;
			}
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity)) {
				GameManager.Player.Target = hit.transform;
				if (GameManager.Player.Target.GetComponent<AiBehaviour> () == null) {
					autoAttack = false;
				}
				moveDirection = hit.point;
				if (usePathFinder) {
					seeker.StartPath (transform.position, hit.point, OnPathComplete);
				}
			}
			nextSearch = Time.time + searchInterval;
			
		}
		
		if (autoAttack) {
			AiBehaviour ai = GameManager.Player.TargetBehaviour;
			if (ai != null && !ai.Dead) {
				if (Vector3.Distance (ai.transform.position, transform.position) < GameManager.PlayerSettings.mouseTalent.maxDistance) {
					GameManager.PlayerSettings.mouseTalent.Use ();
				} else {
					if (usePathFinder) {
						if (Time.time > nextSearch) {
							seeker.StartPath (transform.position, ai.transform.position, OnPathComplete);
							nextSearch = Time.time + searchInterval;
						}
					} else {
						moveDirection = ai.transform.position;
					}
				}
			}
		}
		
		float dist = Vector3.Distance (transform.position, moveDirection);
		if (dist < 2 && GameManager.Player.Target != null) {
			handleCharacterState (CharacterState.Idle);
			
			Shop shop = GameManager.Player.Target.GetComponent<Shop> ();
			if (shop != null) {
				shop.OnMouseUp ();
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
			Quest quest = GameManager.Player.Target.GetComponent<Quest> ();
			if (quest != null) {
				quest.OnMouseUp ();
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
			ItemContainer container = GameManager.Player.Target.GetComponent<ItemContainer> ();
			if (container != null) {
				container.OnMouseUp ();
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
			Lootable loot = GameManager.Player.Target.GetComponent<Lootable> ();
			if (loot != null) {
				loot.OnMouseUp ();
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
			QuestParameter param = GameManager.Player.Target.GetComponent<QuestParameter> ();
			if (param != null) {
				param.OnMouseUp ();
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
			DisplayName name = GameManager.Player.Target.GetComponent<DisplayName> ();
			if (name != null) {
				GameManager.Player.Target = null;
				path = null;
				moveDirection = transform.position;
				return;
			}
			
		}
		
		if (usePathFinder) {
			if (path == null) {
				return;
			}
		
			if (currentWaypoint >= path.vectorPath.Count) {
				handleCharacterState (CharacterState.Idle);
				return;
			}
		
		
			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position);
			if (dir != Vector3.zero) {
				dir.y = 0;
				Quaternion wantedRotation = Quaternion.LookRotation (dir);
				transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * 15);
			}
			dir = dir.normalized;
			dir *= (running ? handleCharacterState (CharacterState.RunForward) : handleCharacterState (CharacterState.WalkForward));
			controller.SimpleMove (dir);
        
			if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
				currentWaypoint++;
				return;
			}
		} else {
			if (dist > 0.1f) {
				moveDirection.y = transform.position.y;
				Vector3 dif = moveDirection - transform.position;
				if (dif != Vector3.zero) {
					Quaternion targetRotation = Quaternion.LookRotation (dif, Vector3.up);
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 10.0f); 
					
					if (dist < 0.5f) {
						transform.rotation = targetRotation;
					}
				}
				controller.SimpleMove (transform.TransformDirection (Vector3.forward) * (running ? handleCharacterState (CharacterState.RunForward) : handleCharacterState (CharacterState.WalkForward)));
				
			} else {
				handleCharacterState (CharacterState.Idle);
			}
		}
		
		
	}

	public void OnPathComplete (Path p)
	{
		if (!p.error) {
			path = p;
			currentWaypoint = 1;
		}
	}
}