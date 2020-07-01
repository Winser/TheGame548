using UnityEngine;
using System.Collections;
using UnityEditor;

public class SetupWizard : EditorWindow {
	[MenuItem ("RPG Kit 2.0/Setup Wizard")]
	public static void Init ()
	{
		EditorWindow.GetWindow (typeof(SetupWizard));
		
	}
	private GameObject prefab;
	private bool asEnemy;
	private bool asShop;
	private bool asQuest;
	
	#region AsEnemy
	private TextAsset aiFile;
	private string questParameter=string.Empty;
	#endregion AsEnemy
	
	#region AsShop
	private ItemTable itemTable;
	#endregion AsShop
	
	#region AsQuest
	private TextAsset questFile;
	#endregion AsQuest
	
	private void OnGUI(){
		prefab=(GameObject)EditorGUILayout.ObjectField("Scene GameObject",prefab,typeof(GameObject),true);
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		GUILayout.Label("Setup prefab as:");
	 	GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
		GUILayout.BeginHorizontal();
		asEnemy=EditorGUILayout.Toggle("Enemy",asEnemy);
		if(asEnemy){
			if(GUILayout.Button("Apply")){
				AddEnemyComponents();
			}
		}
		GUILayout.EndHorizontal();
		if(asEnemy){
			aiFile=(TextAsset)EditorGUILayout.ObjectField("Ai File",aiFile,typeof(TextAsset),true);
			questParameter=EditorGUILayout.TextField("Parameter",questParameter);
		}
		
		GUILayout.BeginHorizontal();
		asShop=EditorGUILayout.Toggle("Shop",asShop);
		if(asShop){
			if(GUILayout.Button("Apply")){
				AddShopComponents();
			}
		}
		GUILayout.EndHorizontal();
		if(asShop){
			itemTable= (ItemTable)EditorGUILayout.ObjectField("Item Table", itemTable, typeof(ItemTable),true);
		}
		
		GUILayout.BeginHorizontal();
		asQuest=EditorGUILayout.Toggle("Quest",asQuest);
		if(asQuest){
			if(GUILayout.Button("Apply")){
				AddQuestComponents();
			}
		}
		GUILayout.EndHorizontal();
		
		if(asQuest){
			questFile=(TextAsset)EditorGUILayout.ObjectField("Quest File",questFile,typeof(TextAsset),true);
		}
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		
	}
	
	private void AddQuestComponents(){
		if(prefab.GetComponent<DisableMouseTalent>()== null){
			prefab.AddComponent<DisableMouseTalent>();
		}
		
		if(prefab.GetComponent<CapsuleCollider>()== null){
			prefab.AddComponent<CapsuleCollider>();
		}
		
		if(prefab.GetComponent<DisplayName>()== null){
			prefab.AddComponent<DisplayName>();
		}
		
		Quest quest= null;
		
		if(prefab.GetComponent<Quest>()== null){
			quest= prefab.AddComponent<Quest>();
		}else{
			quest= prefab.GetComponent<Quest>();
		}
		
		quest.file=questFile;
		
	}
	
	private void AddShopComponents(){
		Shop shop= null;
		if(prefab.GetComponent<Shop>()== null){
			prefab.AddComponent<Shop>();
		}else{
			shop=prefab.GetComponent<Shop>();
		}
		
		shop.itemTable=itemTable;
		
		if(prefab.GetComponent<DisableMouseTalent>()== null){
			prefab.AddComponent<DisableMouseTalent>();
		}
		
		if(prefab.GetComponent<CapsuleCollider>()== null){
			prefab.AddComponent<CapsuleCollider>();
		}
		
		if(prefab.GetComponent<DisplayName>()== null){
			prefab.AddComponent<DisplayName>();
		}
		
		prefab.layer=11;
		
	}
	
	private void AddEnemyComponents(){
		AiBehaviour behaviour=null;
		if(prefab.GetComponent<AiBehaviour>() == null){
			behaviour= prefab.AddComponent<AiBehaviour>();
		}else{
			behaviour= prefab.AddComponent<AiBehaviour>();
		}
		behaviour.file=aiFile;
		
		bool hasPro = UnityEditorInternal.InternalEditorUtility.HasPro();
		if(hasPro){
			UnityEngine.AI.NavMeshAgent agent= null;
			if(prefab.GetComponent<UnityEngine.AI.NavMeshAgent>()== null){
				agent= prefab.AddComponent<UnityEngine.AI.NavMeshAgent>();
			}else{
				agent=prefab.GetComponent<UnityEngine.AI.NavMeshAgent>();
			}
			
			agent.radius=0.5f;
			agent.speed=4;
			agent.acceleration=30;
			agent.angularSpeed=250;
			agent.stoppingDistance=0;
			agent.autoTraverseOffMeshLink=true;
			agent.autoBraking=true;
			agent.autoRepath=true;
			agent.height=2;
			agent.baseOffset=0;
			agent.obstacleAvoidanceType= UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
			agent.avoidancePriority=99;
		}else{
			if(prefab.GetComponent<CustomAStarAgent>()== null){
				prefab.AddComponent<CustomAStarAgent>();
			}
			
		}
		
		if(prefab.GetComponent<DisplayName>()== null){
			prefab.AddComponent<DisplayName>();
		}
		
		if(prefab.GetComponent<PhotonView>()== null){
			PhotonView view= prefab.AddComponent<PhotonView>();
			view.observed=behaviour;
		}
		
		if(prefab.GetComponent<CapsuleCollider>()== null){
			prefab.AddComponent<CapsuleCollider>();
		}
		
		if(!questParameter.Equals(string.Empty)){
			QuestParameter param= null;
			if(prefab.GetComponent<QuestParameter>()== null){
				param= prefab.AddComponent<QuestParameter>();
			}else{
				param=prefab.GetComponent<QuestParameter>();
			}
			param.parameter=questParameter;
		}
		
		prefab.layer=11;
	}
}
