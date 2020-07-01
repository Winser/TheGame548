using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class GreaterThanComparerTransition : BaseComparerTransition {
	
#if UNITY_EDITOR
	public GreaterThanComparerTransition(Vector2 position):base (position){
		this.Title="Greater Than";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Title="Greater Than";

	}
#endif
}
