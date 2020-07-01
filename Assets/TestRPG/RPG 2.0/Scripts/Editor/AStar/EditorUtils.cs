using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorUtils
{
	public static LayerMask LayerMaskField (string label, LayerMask selected)
	{
		return LayerMaskField (label, selected, true);
	}

	public static LayerMask LayerMaskField (string label, LayerMask selected, bool showSpecial)
	{

		List<string> layers = new List<string> ();
		List<int> layerNumbers = new List<int> ();

		string selectedLayers = "";

		for (int i=0; i<32; i++) {

			string layerName = LayerMask.LayerToName (i);

			if (layerName != "") {
				if (selected == (selected | (1 << i))) {

					if (selectedLayers == "") {
						selectedLayers = layerName;
					} else {
						selectedLayers = "Mixed";
					}
				}
			}
		}

		EventType lastEvent = Event.current.type;

		if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.ExecuteCommand) {
			if (selected.value == 0) {
				layers.Add ("Nothing");
			} else if (selected.value == -1) {
				layers.Add ("Everything");
			} else {
				layers.Add (selectedLayers);
			}
			layerNumbers.Add (-1);
		}

		if (showSpecial) {
			layers.Add ((selected.value == 0 ? "[X] " : "     ") + "Nothing");
			layerNumbers.Add (-2);

			layers.Add ((selected.value == -1 ? "[X] " : "     ") + "Everything");
			layerNumbers.Add (-3);
		}

		for (int i=0; i<32; i++) {

			string layerName = LayerMask.LayerToName (i);

			if (layerName != "") {
				if (selected == (selected | (1 << i))) {
					layers.Add ("[X] " + layerName);
				} else {
					layers.Add ("     " + layerName);
				}
				layerNumbers.Add (i);
			}
		}

		bool preChange = GUI.changed;

		GUI.changed = false;

		int newSelected = 0;

		if (Event.current.type == EventType.MouseDown) {
			newSelected = -1;
		}

		newSelected = EditorGUILayout.Popup (label, newSelected, layers.ToArray (), EditorStyles.layerMaskField);

		if (GUI.changed && newSelected >= 0) {
			//newSelected -= 1;

			Debug.Log (lastEvent + " " + newSelected + " " + layerNumbers [newSelected]);

			if (showSpecial && newSelected == 0) {
				selected = 0;
			} else if (showSpecial && newSelected == 1) {
				selected = -1;
			} else {

				if (selected == (selected | (1 << layerNumbers [newSelected]))) {
					selected &= ~(1 << layerNumbers [newSelected]);
					//Debug.Log ("Set Layer "+LayerMask.LayerToName (LayerNumbers[newSelected]) + " To False "+selected.value);
				} else {
					//Debug.Log ("Set Layer "+LayerMask.LayerToName (LayerNumbers[newSelected]) + " To True "+selected.value);
					selected = selected | (1 << layerNumbers [newSelected]);
				}
			}
		} else {
			GUI.changed = preChange;
		}

		return selected;
	}

}
