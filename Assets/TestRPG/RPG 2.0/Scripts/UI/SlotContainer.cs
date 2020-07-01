using UnityEngine;
using System.Collections;

public class SlotContainer : MonoBehaviour {
	[SerializeField]
	private UISprite icon;
	public KeyCode key;
	[HideInInspector]
	public Slot slot;
	[SerializeField]
	private Renderer overlay;
	private float _initTime;
	private float _perc;
	[HideInInspector]
	public bool isCoolDown;
	private bool start;
	private float duration = 5;
	public bool defaultAttackContainer;
	public static bool disableMouseTalent;
		
	private void Start(){
		if(defaultAttackContainer && GameManager.PlayerSettings.mouseTalent){
			icon.gameObject.SetActive(true);
			icon.spriteName=GameManager.PlayerSettings.mouseTalent.icon;
			GameManager.PlayerSettings.mouseTalent.spentPoints=1;
		}
	}
	
	private void Update() {
		
		if (Time.time - _initTime < duration && start == true) {
			isCoolDown = true;
			_perc = 1 - ((Time.time - _initTime) / duration);
			overlay.material.SetFloat ("_Cutoff", _perc);
		} else {
			overlay.material.SetFloat ("_Cutoff", 0);
			isCoolDown = false;
			if(slot != null && slot.removeIcon){
				slot.removeIcon=false;
				icon.gameObject.SetActive(false);
			}
		}
		
        if (Input.GetMouseButtonUp(0) && InterfaceContainer.Instance.isDragging)
        {
            Ray ray = UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.Equals(GetComponent<Collider>()))
                {
                   slot=InterfaceContainer.Instance.draggingSlot;
				   icon.gameObject.SetActive(true);
				   icon.spriteName=slot.icon.spriteName;
				   InterfaceContainer.Instance.isDragging=false;
                }
            }
        }
		
		if(Input.GetKeyUp(key)&& !isCoolDown && slot && slot.Use() && !ChatManager.Instance.chatInput.gameObject.activeSelf){
			StartCoolDown(slot.coolDown);
		}
		
		if(!isCoolDown && Input.GetMouseButtonDown(0)&& !disableMouseTalent && GameManager.PlayerSettings.mouseTalent && defaultAttackContainer && GameManager.Player.Movement.controllerType!= ThirdPersonMovement.ControllerType.ClickToMove && !InterfaceContainer.Instance.UIClick()){
			if(slot && slot.Use()){
				StartCoolDown(slot.coolDown);
			}else{
				if(GameManager.PlayerSettings.mouseTalent.Use()){
					StartCoolDown(GameManager.PlayerSettings.mouseTalent.coolDown);
				}
			}
		}
		
    }
	
	public void OnClick(){
		if(!isCoolDown && slot && slot.Use()){
			StartCoolDown(slot.coolDown);
		}else if(!slot && GameManager.PlayerSettings.mouseTalent && GameManager.PlayerSettings.mouseTalent.Use()){
			StartCoolDown(GameManager.PlayerSettings.mouseTalent.coolDown);
		}
	}
	
	public void Set(Slot s, string sprite,float coolDown){
		slot=s;	
		icon.gameObject.SetActive(true);
		icon.spriteName=sprite;
		s.coolDown=coolDown;
	}
	
	public void Set(TalentSlot s){
		slot=s;	
		icon.gameObject.SetActive(true);
		icon.spriteName=s.talent.icon;
		s.coolDown=s.talent.coolDown;
	}
	
	public void StartCoolDown (float delay)
	{
		duration = delay;
		start = true;
		if (!isCoolDown) {
			_initTime = Time.time;
		}
	}
}
