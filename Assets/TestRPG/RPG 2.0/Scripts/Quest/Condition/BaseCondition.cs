using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class BaseCondition : AbstractState {

#if UNITY_EDITOR
	public BaseCondition(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Condition";
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Condition";
	}
	
	public virtual void Setup(){
		x = Position.x;
		y = Position.y;
	}
#endif
}
