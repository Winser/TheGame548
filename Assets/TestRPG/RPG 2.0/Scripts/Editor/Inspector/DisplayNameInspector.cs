using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DisplayName))]
public class DisplayNameInspector : Editor {
	private DisplayName displayName;
	
	private void OnEnable ()
	{
		displayName = (DisplayName)target;
	}
	
	public override void OnInspectorGUI ()
	{
		GUI.changed=false;
		if(!displayName.random){
			displayName.nameToDisplay=EditorGUILayout.TextField("Name",displayName.nameToDisplay);
		}
		displayName.random=EditorGUILayout.Toggle("Random Name",displayName.random);
		if(displayName.random){
			displayName.nameTemplate=(TextTemplate)EditorGUILayout.ObjectField("Template",displayName.nameTemplate,typeof(TextTemplate),false);
		}
		displayName.color=EditorGUILayout.ColorField("Color",displayName.color);
		displayName.showTrigger=(ShowTrigger)EditorGUILayout.EnumPopup("Show",displayName.showTrigger);
		displayName.nameLabel=(UILabel) EditorGUILayout.ObjectField("Label",displayName.nameLabel,typeof(UILabel),true);
		if (GUI.changed) {
			EditorUtility.SetDirty (target);
		}
	}
}
