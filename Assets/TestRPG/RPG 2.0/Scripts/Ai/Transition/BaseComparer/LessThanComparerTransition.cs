using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class LessThanComparerTransition : BaseComparerTransition {
	
#if UNITY_EDITOR
	public LessThanComparerTransition(Vector2 position):base (position){
		this.Title="Less Than";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Title="Less Than";

	}
#endif
	
}
