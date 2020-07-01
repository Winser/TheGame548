using UnityEngine;
using System.Collections;
using UnityEditor;

public class AiPopupWindow : EditorWindow {
	private static AiPopupWindow window;
	private AbstractState state;
	
	public static void Show(Vector2 pos,AbstractState state){
		window = EditorWindow.GetWindow<AiPopupWindow>();
		window.title="Properties";
		window.state=state;
		window.minSize= new Vector2(300,200);
		window.position = new Rect(pos.x-window.minSize.x/2,pos.y-window.minSize.y/2, window.minSize.x,window.minSize.y);
		state.Init();
	}
	
	private void OnGUI(){
		state.OnGUI();
	}
}
