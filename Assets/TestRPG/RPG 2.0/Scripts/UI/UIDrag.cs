using UnityEngine;
using System.Collections;

public class UIDrag : MonoBehaviour {
	
	private static Transform draggingWindow;
	
	public Vector3 scale = Vector3.one;
	Plane mPlane;
	Vector3 mLastPos;
	UIPanel mPanel;
	
	public static Transform DraggingWindow{
		get{return draggingWindow;}
		set{
			if(draggingWindow != null){
				draggingWindow.localPosition=new Vector3(draggingWindow.localPosition.x,draggingWindow.localPosition.y,0);
			}
			draggingWindow=value;
			draggingWindow.localPosition=new Vector3(draggingWindow.localPosition.x,draggingWindow.localPosition.y,-5);
		}
	}
	
	
	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>
	
	public virtual void OnPress (bool pressed)
	{
		if (enabled && GetActive (gameObject) ) {
			if (pressed) {
				mLastPos = UICamera.lastHit.point;
				mPlane = new Plane (Vector3.back, mLastPos);
				DraggingWindow=transform;
			} 
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
		if (enabled && GetActive (gameObject)) {
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

			Ray ray = UICamera.currentCamera.ScreenPointToRay (UICamera.currentTouch.pos);
			float dist = 0f;

			if (mPlane.Raycast (ray, out dist)) {
				Vector3 currentPos = ray.GetPoint (dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;

				if (offset.x != 0f || offset.y != 0f) {
					offset = transform.InverseTransformDirection (offset);
					offset.Scale (scale);
					offset = transform.TransformDirection (offset);
				}
				
				offset.z=0;
				transform.position += offset;
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
