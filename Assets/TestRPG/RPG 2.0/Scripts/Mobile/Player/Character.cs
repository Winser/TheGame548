using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Character:ScriptableObject
{
	public string characterClass;
	public GameObject prefab;
	public AnimationClip idle;
	public AnimationClip walk;
	public AnimationClip walkLeft;
	public AnimationClip walkRight;
	public AnimationClip run;
	public AnimationClip runLeft;
	public AnimationClip runRight;
	public AnimationClip dodgeRight;
	public AnimationClip dodgeLeft;
	public AnimationClip dodgeBack;
	public AnimationClip jump;
	public AnimationClip getHit;
	public AnimationClip pickUp;
	public AnimationClip die;
	public AudioClip getHitSound;
	
	public CharacterMotor characterMotor = new CharacterMotor ();
	public List<BaseTalent> talents = new List<BaseTalent> ();
	public List<PlayerAttribute> attributes = new List<PlayerAttribute> ();

	
	#if UNITY_EDITOR
	private bool animationFoldOut;
	private bool motorFoldOut;
	private bool attributeFoldOut;
	private bool talentFoldOut;
	private bool soundFoldOut;
	
	public void OnGUI(){
		GUILayout.BeginVertical ("box");
		characterClass=EditorGUILayout.TextField("Character Class",characterClass);
		prefab = (GameObject)EditorGUILayout.ObjectField ("Prefab",prefab, typeof(GameObject), true);
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		animationFoldOut = EditorGUILayout.Foldout (animationFoldOut, "Player Animation");
		if (animationFoldOut) {
			idle = (AnimationClip)EditorGUILayout.ObjectField ("Idle", idle, typeof(AnimationClip), false);
			walk = (AnimationClip)EditorGUILayout.ObjectField ("Walk", walk, typeof(AnimationClip), false);
			walkLeft = (AnimationClip)EditorGUILayout.ObjectField ("Walk Left", walkLeft, typeof(AnimationClip), false);
			walkRight = (AnimationClip)EditorGUILayout.ObjectField ("Walk Right", walkRight, typeof(AnimationClip), false);
			
			dodgeBack = (AnimationClip)EditorGUILayout.ObjectField ("Dodge Back", dodgeBack, typeof(AnimationClip), false);
			dodgeLeft = (AnimationClip)EditorGUILayout.ObjectField ("Dodge Left", dodgeLeft, typeof(AnimationClip), false);
			dodgeRight = (AnimationClip)EditorGUILayout.ObjectField ("Dodge Right", dodgeRight, typeof(AnimationClip), false);

			run = (AnimationClip)EditorGUILayout.ObjectField ("Run", run, typeof(AnimationClip), false);
			runLeft = (AnimationClip)EditorGUILayout.ObjectField ("Run Left", runLeft, typeof(AnimationClip), false);
			runRight = (AnimationClip)EditorGUILayout.ObjectField ("Run Right", runRight, typeof(AnimationClip), false);
			jump = (AnimationClip)EditorGUILayout.ObjectField ("Jump", jump, typeof(AnimationClip), false);
			getHit = (AnimationClip)EditorGUILayout.ObjectField ("Get Hit", getHit, typeof(AnimationClip), false);
			die = (AnimationClip)EditorGUILayout.ObjectField ("Die", die, typeof(AnimationClip), false);
		}
		GUILayout.EndVertical();
			
		GUILayout.BeginVertical("box");
		soundFoldOut=EditorGUILayout.Foldout(soundFoldOut,"Sounds");
		if(soundFoldOut){
			getHitSound=(AudioClip) EditorGUILayout.ObjectField("Get Hit",getHitSound,typeof(AudioClip),false);
		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		motorFoldOut = EditorGUILayout.Foldout (motorFoldOut, "Player Motor");
		if (motorFoldOut) {
			characterMotor.walkSpeed = EditorGUILayout.FloatField ("Walk Speed", characterMotor.walkSpeed);
			characterMotor.walkLeftSpeed = EditorGUILayout.FloatField ("Walk Left Speed", characterMotor.walkLeftSpeed);
			characterMotor.walkRightSpeed = EditorGUILayout.FloatField ("Walk Right Speed", characterMotor.walkRightSpeed);
			characterMotor.walkBackSpeed = EditorGUILayout.FloatField ("Walk Backward Speed", characterMotor.walkBackSpeed);
			characterMotor.runSpeed = EditorGUILayout.FloatField ("Run Speed", characterMotor.runSpeed);
			characterMotor.runLeftSpeed = EditorGUILayout.FloatField ("Run Left Speed", characterMotor.runLeftSpeed);
			characterMotor.runRightSpeed = EditorGUILayout.FloatField ("Run Right Speed", characterMotor.runRightSpeed);
			characterMotor.runBackSpeed = EditorGUILayout.FloatField ("Run Backward Speed", characterMotor.runBackSpeed);
			characterMotor.rotationSpeed = EditorGUILayout.FloatField ("Rotation Speed", characterMotor.rotationSpeed);
			characterMotor.jumpHeight = EditorGUILayout.FloatField ("Jump Height", characterMotor.jumpHeight);
			characterMotor.gravity = EditorGUILayout.FloatField ("Gravity", characterMotor.gravity);
		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		attributeFoldOut=EditorGUILayout.Foldout(attributeFoldOut,"Attribute");
		if(attributeFoldOut){
			int index=-1;
			if(GUILayout.Button("Add")){
				attributes.Add(null);
			}
			
			for(int i=0; i< attributes.Count;i++){
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("X", GUILayout.Width(25))){
					index=i;
				}
				attributes[i]=(PlayerAttribute)EditorGUILayout.ObjectField(attributes[i],typeof(PlayerAttribute),false);
				GUILayout.EndHorizontal();
			}
			
			if(index != -1){
				attributes.RemoveAt(index);
			}

		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		talentFoldOut=EditorGUILayout.Foldout(talentFoldOut,"Talent");
		if(talentFoldOut){
			if(GUILayout.Button("Add")){
				talents.Add(null);
			}
			
			int index=-1;
			for(int i=0; i< talents.Count;i++){
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("X", GUILayout.Width(25))){
					index=i;
				}
				talents[i]=(BaseTalent)EditorGUILayout.ObjectField(talents[i],typeof(BaseTalent),false);
				GUILayout.EndHorizontal();
			}
			
			if(index != -1){
				talents.RemoveAt(index);
			}
		}
		GUILayout.EndVertical();
		
	}
	#endif
}
