using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.ObjectModel;
using System.Collections.Generic;
#endif

public class StateNode{
	
#if UNITY_EDITOR
	private const float nodeSize=10;
	public Vector2 position;
	private static StateNode selection;
	private List<StateNode> targets;
	private string label;
	public AbstractState state;
	public System.Type type;
	public StateNode(string label,AbstractState owner, System.Type type){
		this.targets= new List<StateNode>();
		this.label= label;
		this.state=owner;
		this.type=type;
	}
	
	public StateNode(string label,AbstractState owner){
		this.targets= new List<StateNode>();
		this.label= label;
		this.state=owner;
	}
	 public ReadOnlyCollection<StateNode> Targets{
        get{return targets.AsReadOnly();}
    }
	
	public static StateNode Selection{
        get{return selection;}
        set{selection = value;}
    }
	
	public void DrawNode(Rect rect){
		GUILayout.BeginHorizontal();
		GUILayout.Label(label);

		GUIStyle xButton = GUI.skin.button;
		xButton.margin.top = 5;
		
		Rect rt = GUILayoutUtility.GetRect(new GUIContent(""),xButton);
		rt.width=rt.height=nodeSize;
		rt.x= rect.width-20;
		position= new Vector2(rt.x+ rect.x+ nodeSize*0.5f,rt.y+rect.y+nodeSize*0.5f);
		if(Targets.Count>0){
			GUI.color=Color.red;
		}
		if(GUI.Button(rt,"")){
			Selection= this;
		}
		GUI.color=Color.white;
		GUILayout.EndHorizontal();
	}
	
	public void DrawNode(Vector2 pos){
		position= pos+ new Vector2(nodeSize*0.5f, nodeSize*0.5f);
		
		if(GUI.Button(new Rect(pos.x,pos.y,nodeSize,nodeSize),"")){
			if(Selection != null){
				selection.ConnectTo(this);
			}
		}
	}
	
	public void ConnectTo(StateNode target){
		if(!targets.Contains(target)){
        	targets.Add(target); 
			Selection= null;
		}
    }
	
	public void Disconnect(StateNode target){
		targets.Remove(target);
	}
	
	public static void DrawConnection(Vector2 from, Vector2 to, bool toMouse)
    {
        bool left = from.x > to.x;
        Vector3 startPoint = new Vector3(from.x + (left ? -nodeSize : nodeSize) * 0.3f, from.y, 0.0f);
        Vector3 endPoint = new Vector3(to.x + (left ? nodeSize : -nodeSize) * 0.3f, to.y, 0.0f);
        if (toMouse) {
            endPoint = new Vector3(to.x, to.y, 0.0f);
        }
        Vector3 startTangent=new Vector3(from.x, from.y, 0.0f) + Vector3.right * 50.0f * (left ? -1.0f : 1.0f);
        Vector3 endTangent = new Vector3(to.x, to.y, 0.0f) + Vector3.right * 50.0f * (left ? 1.0f : -1.0f);

        Handles.DrawBezier(
            startPoint,
            endPoint,
            startTangent,
            endTangent,
            GUI.color,
            null,
            2.0f
        );
    }

	public List<string> GetStringList(){
		List<string> list= new List<string>();
		foreach(StateNode node in targets){
			if(node.state is StringField){
				list.Add(((StringField)node.state).stringVar);
			}
		}
		return list;
	}
	
	public string GetString(){
	
		foreach(StateNode node in targets){
			if(node.state is StringField){
				return (node.state as StringField).stringVar;
			}
		}
		return null;
	}
	
	public string GetText(){
		foreach(StateNode node in targets){
			if(node.state is TextField){
				return (node.state as TextField).stringVar;
			}
		}
		return null;
	}
	
	public float GetFloat(){
		foreach(StateNode node in targets){
			if(node.state is FloatField){
				return (node.state as FloatField).floatVar; 
			}
		}
		return 0;
	}
	
	public int GetInt(){
		foreach(StateNode node in targets){
			if(node.state is IntField){
				return (node.state as IntField).intVar; 
			}
		}
		return 0;
	}
	
	public BaseItem GetItem(){
		foreach(StateNode node in targets){
			if(node.state is ItemField){
				return (node.state as ItemField).item;
			}
		}
		return null;
	}
	
	public BaseTalent GetTalent(){
		foreach(StateNode node in targets){
			if(node.state is TalentField){
				return (node.state as TalentField).talent;
			}
		}
		return null;
	}
	
	public ItemTable GetItemTable(){
		foreach(StateNode node in targets){
			if(node.state is ItemTableField){
				return (node.state as ItemTableField).itemTable;
			}
		}
		return null;
	}
	
	public BaseState GetBaseState(){
		foreach(StateNode node in targets){
			if(node.state is BaseState){
				return node.state as BaseState;
			}
		}
		return null;
	}
	
	public BaseTaskState GetBaseTask(){
		foreach(StateNode node in targets){
			if(node.state is BaseTaskState){
				return node.state as BaseTaskState;
			}
		}
		return null;
	}
	
	public AnimationClip GetAnimationClip(){
		foreach(StateNode node in targets){
			if(node.state is AnimationField){
				return (node.state as AnimationField).anim;
			}
		}
		return null;
	}
	
	public List<BaseCondition> GetBaseCondition(){
		List<BaseCondition> list= new List<BaseCondition>(); 
		foreach(StateNode node in targets){
			if(node.state is BaseCondition){
				list.Add(node.state as BaseCondition);
			}
		}
		return list;
	}
		
	public List<BaseTransition> GetBaseTransition(){
		List<BaseTransition> list= new List<BaseTransition>(); 
		foreach(StateNode node in targets){
			if(node.state is BaseTransition){
				list.Add(node.state as BaseTransition);
			}
		}
		return list;
	}
	
	public List<BaseComparerTransition> GetBaseComparerTransition(){
		List<BaseComparerTransition> list= new List<BaseComparerTransition>(); 
		foreach(StateNode node in targets){
			if(node.state is BaseComparerTransition){
				list.Add(node.state as BaseComparerTransition);
			}
		}
		return list;
	}
	
	public List<BaseAttribute> GetBaseAttribute(){
		List<BaseAttribute> list= new List<BaseAttribute>();
		foreach(StateNode node in targets){
			if(node.state is BaseAttribute){
				list.Add(node.state as BaseAttribute);
			}
		}
		return list;
	}
#endif
}
