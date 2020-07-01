using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class RespawnState : BaseState {
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		ai.StopAgent();
		foreach(BaseAttribute attribute in ai.GetBaseAttributes()){
			attribute.Refresh();
		}
		ai.transform.position=ai.startPosition;
		if(ai.onRespawn != null && PhotonNetwork.isMasterClient){
			ai.onRespawn();
		}
		
	}
#if UNITY_EDITOR
	public RespawnState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Respawn";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Respawn";
	}
#endif
}
