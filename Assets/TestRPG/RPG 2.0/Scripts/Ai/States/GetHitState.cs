using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class GetHitState : BaseState {
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		ai.StopAgent();
	}
	
	#if UNITY_EDITOR
	public GetHitState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="OnHit";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="OnHit";
	}
#endif
}
