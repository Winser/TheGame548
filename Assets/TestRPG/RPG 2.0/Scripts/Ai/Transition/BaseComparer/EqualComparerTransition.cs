using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class EqualComparerTransition : BaseComparerTransition {

#if UNITY_EDITOR

	public EqualComparerTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Equal";

	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Equal";
	}
#endif

}
