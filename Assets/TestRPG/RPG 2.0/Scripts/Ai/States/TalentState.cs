using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class TalentState : BaseState {
	public string talent;
	[System.NonSerialized]
	private float delay;
	[System.NonSerialized]
	private BaseTalent mTalent;
	public BaseTalent Talent{
		get{
			if(mTalent == null){
				mTalent=(BaseTalent)ScriptableObject.Instantiate(GameManager.TalentDatabase.GetTalent(talent));
			}
			return mTalent;
		}
	}
		
	public override void HandleState (AiBehaviour ai)
	{
		if(ai.target== null){
			CheckTransition(ai);
			return;
		}

		Talent.Use(ai);

		if(!ai.GetComponent<Animation>().IsPlaying(mTalent.animation.name)){
			CheckTransition(ai);
		}
	}
	
	/*public IEnumerator InstantiateProjectile (Talent t,float d, AiBehaviour ai)
	{
		yield return new WaitForSeconds(d);
		GameObject go = PhotonNetwork.Instantiate (t.projectile.name, ai.transform.position+t.projectileOffset, ai.transform.rotation,0);
		Missle missle= go.GetComponent<Missle>();
		go.AddComponent<TimedDestroy>().time=7;
		if(missle){
			Physics.IgnoreCollision(ai.collider,go.collider);
			missle.talent=t;
		}
	}
	
	public IEnumerator InstantiateProjectile (Talent t,float d, AiBehaviour ai, Vector3 position)
	{
		yield return new WaitForSeconds(d);
		GameObject go = PhotonNetwork.Instantiate (t.projectile.name, position+t.projectileOffset, ai.transform.rotation,0);
		//Missle missle= go.GetComponent<Missle>();
		go.AddComponent<TimedDestroy>().time=7;
		if(missle){
			Physics.IgnoreCollision(ai.collider,go.collider);
			missle.talent=t;
		}
		
		switch(t.talentType){
		case Talent.TalentType.AOE:
			switch(t.castOnType){
			case Talent.CastOnType.Target:
				if(Vector3.Distance(ai.target.position, position)< t.aoeRange){
					ai.StartCoroutine(ai.ApplyPlayerDamage(t.damageAttribute,t.damage,0));
				}
				break;
			}
			break;
		}
	}*/
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode talentNode;

	public TalentState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Talent Attack";
		this.talentNode= new StateNode("Talent",this,typeof(TalentField));
		this.Nodes.Add(talentNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Talent Attack";
		this.talentNode= new StateNode("Talent",this,typeof(TalentField));
		this.Nodes.Add(talentNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		BaseTalent mTalent= talentNode.GetTalent();
		if(mTalent != null){
			talent=mTalent.talentName;
			GameManager.TalentDatabase.AddTalent(mTalent);
		}
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		mTalent=(BaseTalent)EditorGUILayout.ObjectField("Talent",mTalent, typeof(BaseTalent),false);
	}
	
	public override void Save (System.IO.FileStream fileStream, System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter)
	{
		AnimationClip animationClip= animationNode.GetAnimationClip();
		if(animationClip != null){
			animation= animationNode.GetAnimationClip().name;
		}else{
			animation=animationNode.GetString();
		}
		
		transitions= transitionNode.GetBaseTransition();
		foreach(BaseTransition transition in transitions){
			transition.Setup();
		}
		
		BaseTalent mTalent= talentNode.GetTalent();
		if(mTalent != null){
			talent=mTalent.talentName;
			GameManager.TalentDatabase.AddTalent(mTalent);
		}
		
		
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
