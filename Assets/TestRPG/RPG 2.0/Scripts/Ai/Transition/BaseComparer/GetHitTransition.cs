using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class GetHitTransition : BaseComparerTransition {

#if UNITY_EDITOR
	public GetHitTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Get Hit";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Get Hit";
	}
	
	public override void Setup ()
	{
		base.Setup ();
	}
#endif
}
