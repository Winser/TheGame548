using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour
{
	public UISprite icon;
	[HideInInspector]
	public float coolDown;
	[HideInInspector]
	public bool removeIcon;
	public virtual void Start ()
	{
		
	}
	
	public virtual void Update(){
		
	}
	
	public virtual bool Use ()
	{
		return true;
	}
	
	//public Transform target;
   
	/// <summary>
	/// Scale value applied to the drag delta. Set X or Y to 0 to disallow dragging in that direction.
	/// </summary>

	public Vector3 scale = Vector3.one;
	Plane mPlane;
	Vector3 mLastPos;
	UIPanel mPanel;

	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>

	public virtual void OnPress (bool pressed)
	{
		if (enabled && GetActive (gameObject) && InterfaceContainer.Instance.dragIcon != null) {
			if (pressed) {
				if((InterfaceContainer.Instance.draggingSlot = this) is ItemSlot && !GameManager.Player.Inventory.GetItem((ItemSlot)this)){
					return;
				}
				
				InterfaceContainer.Instance.dragIcon.gameObject.SetActive (true);
				InterfaceContainer.Instance.isDragging = true;
				InterfaceContainer.Instance.dragIcon.spriteName = icon.spriteName;
				

				InterfaceContainer.Instance.dragIcon.transform.position = new Vector3(transform.position.x,transform.position.y,InterfaceContainer.Instance.dragIcon.transform.position.z);
				mLastPos = UICamera.lastHit.point;
				mPlane = new Plane (Vector3.back, mLastPos);
			} else {
				InterfaceContainer.Instance.dragIcon.transform.position = new Vector3(transform.position.x,transform.position.y,InterfaceContainer.Instance.dragIcon.transform.position.z);
				InterfaceContainer.Instance.dragIcon.gameObject.SetActive (false);
				if (InterfaceContainer.Instance.isDragging && !InterfaceContainer.Instance.UIClick () ){
					if((this is ItemSlot)) {
						GameManager.Player.Inventory.DropItem ((ItemSlot)this,GameManager.Player.transform.position + Vector3.up *2 + GameManager.Player.transform.forward * 1.5f);
					}
					InterfaceContainer.Instance.isDragging = false;
				}

				
			}
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
		if (enabled && GetActive (gameObject) && InterfaceContainer.Instance.dragIcon != null) {
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

			Ray ray = UICamera.currentCamera.ScreenPointToRay (UICamera.currentTouch.pos);
			float dist = 0f;

			if (mPlane.Raycast (ray, out dist)) {
				Vector3 currentPos = ray.GetPoint (dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;

				if (offset.x != 0f || offset.y != 0f) {
					offset = InterfaceContainer.Instance.dragIcon.transform.InverseTransformDirection (offset);
					offset.Scale (scale);
					offset = InterfaceContainer.Instance.dragIcon.transform.TransformDirection (offset);
				}
				
				offset.z=0;
				InterfaceContainer.Instance.dragIcon.transform.position += offset;
			}
		}
	}
	
	static public bool GetActive (GameObject go)
	{
#if UNITY_3_5
		return go && go.active;
#else
		return go && go.activeInHierarchy;
#endif
	}
	
}
