using UnityEngine;
using System.Collections;

/// <summary>
/// Base storage for experience points
/// </summary>
public class Exp {
	private float curValue;
	private float baseValue;
	private float barLength;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Exp"/> class.
	/// </summary>
	/// <param name='level'>
	/// Player Level.
	/// </param>
	public Exp(int level){
		barLength=InterfaceContainer.Instance.expBar.transform.localScale.x;
		InterfaceContainer.Instance.expBar.transform.localScale= new Vector3(0.001f, InterfaceContainer.Instance.expBar.transform.localScale.y, InterfaceContainer.Instance.expBar.transform.localScale.z);
		BaseValue = 100 * Mathf.Pow(2,(level-2));
		
	}
	
	/// <summary>
	/// Applies the exp to the player and raises his level
	/// </summary>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void ApplyExp(float val){
		curValue+=val;
		if(curValue>=baseValue){
			curValue=0;
			BaseValue = 100 * Mathf.Pow(2,(GameManager.Player.Level-2));
			GameManager.Player.Level++;
		}
		UpdateBar();
	}
	
	/// <summary>
	/// Updates the exp bar.
	/// </summary>
	public void UpdateBar(){
		float dx = (barLength * CurValue) / (BaseValue)+0.00001f;	
		InterfaceContainer.Instance.expBar.transform.localScale = new Vector3(dx, InterfaceContainer.Instance.expBar.transform.localScale.y, InterfaceContainer.Instance.expBar.transform.localScale.z);
	}
	
	/// <summary>
	/// Gets or sets the base value to reach for level up
	/// </summary>
	/// <value>
	/// The base value.
	/// </value>
	public float BaseValue{
		get{return baseValue;}
		set{baseValue=value;
			UpdateBar();
		}
	}
	
	/// <summary>
	/// Gets the current value.
	/// </summary>
	/// <value>
	/// The current value.
	/// </value>
	public float CurValue{
		get{return curValue;}
	}
}
