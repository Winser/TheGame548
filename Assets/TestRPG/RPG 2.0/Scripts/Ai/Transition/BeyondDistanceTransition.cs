using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BeyondDistanceTransition : BaseTransition {
	public float distance;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode distanceNode;
	
	public BeyondDistanceTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Beyond Distance";
		distanceNode= new StateNode("Distance",this,typeof(FloatField));
		this.Nodes.Add(distanceNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Beyond Distance";
		distanceNode= new StateNode("Distance",this,typeof(FloatField));
		this.Nodes.Add(distanceNode);
	}
	
	public override void Init ()
	{
		base.Init();
		distance= distanceNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		distance=EditorGUILayout.FloatField("Distance",distance);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		distance= distanceNode.GetFloat();
	}
#endif
}
