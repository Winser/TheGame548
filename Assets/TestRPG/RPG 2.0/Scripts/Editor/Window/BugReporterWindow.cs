using UnityEngine;
using System.Collections;
using UnityEditor;

public class BugReporterWindow : EditorWindow {
	[MenuItem("RPG Kit 2.0/Report Bug")]
	static void Init ()
	{
		BugReporterWindow window= (BugReporterWindow)EditorWindow.GetWindow (typeof(BugReporterWindow));
		window.minSize= new Vector2(300,200);
		window.reporter=(BugReporterInformation)Resources.Load("Internal/BugReporterInformation",typeof(BugReporterInformation));
		window.title="Report Bug";
	}
	
	private BugReporterInformation reporter;
	private string reportTitle=string.Empty;
	private string description=string.Empty;
	private Vector2 scroll;
	private string serverAddress="http://zerano-unity3d.com/RPG/";
	private WWW _request;
    private string _response = string.Empty;

	private void OnGUI(){
		reportTitle=EditorGUILayout.TextField("Title",reportTitle);
		reporter.email=EditorGUILayout.TextField("Email",reporter.email);
		GUILayout.Label("Description:");
		scroll = EditorGUILayout.BeginScrollView(scroll);        
		description=EditorGUILayout.TextArea(description,GUILayout.Height(position.height - 70));
		GUILayout.EndScrollView();
		if(GUILayout.Button("Report Bug")){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("title", reportTitle);
			newForm.AddField ("description", description);
			newForm.AddField("email",reporter.email);
			_request = new WWW(serverAddress + "/ReportBug.php", newForm);
		}
	}
	
	private void Update(){
        if (_request != null){
            if (_request.isDone){
                if (_request.error != null){
                    Debug.LogError("Error getting response: " + _request.error);
                }else{
                    _response += _request.text + System.Environment.NewLine; // read
                    Debug.Log("Response: " + _response);
                }
                _request = null; // reset
            }
        }
    }
}
