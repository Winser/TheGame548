using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class ExpCondition : BaseCondition {

#if UNITY_EDITOR

	public ExpCondition(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Exp Condition";
	}
#endif
}