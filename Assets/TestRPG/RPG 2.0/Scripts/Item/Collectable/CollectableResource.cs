using UnityEngine;
using System.Collections;

public class CollectableResource : MonoBehaviour
{
	public CollectableItem item;
	public EquipmentItem tool;
	public float collectCoolDown = 3;
	public bool lookAt;
	public AnimationClip collectAnimation;
	public float maxDistance = 2.0f;
	public int stack=5;
	public float fillRate=15;
	private int maxStack;
	public bool destroyIfEmpty;
	
	private void Start(){
		maxStack=stack;	
		StartCoroutine(FillSpot());
	}
	
	private void OnMouseUp ()
	{
		if (Vector3.Distance (transform.position, GameManager.Player.transform.position) < maxDistance) {
			if(tool != null){
				EquipmentItem equipedTool=GameManager.Player.Inventory.GetEquipmentItem(EquipmentItem.Region.Hands);
				if(equipedTool== null || !equipedTool.itemName.Equals(tool.itemName)){
					MessageManager.Instance.AddMessage(GameManager.GameMessages.needTool.Replace("@ItemName",tool.itemName));
					return;
				}
				
			}
			if(stack>0){
				ProgressBar progress = InterfaceContainer.Instance.progressBar;
				if (progress.isDone) {
					GameManager.Player.transform.LookAt (new Vector3 (transform.position.x, GameManager.Player.transform.position.y, transform.position.z));
					if(collectAnimation != null){
						GameManager.Player.Movement.PlayAnimation (collectAnimation.name, collectCoolDown);
					}
					progress.StartProgress (collectCoolDown, gameObject, "OnCollect");
				}
			}else{
				MessageManager.Instance.AddMessage (GameManager.GameMessages.emptySpot.Replace("@ItemName",item.itemName));
			}
		} else {
			MessageManager.Instance.AddMessage (GameManager.GameMessages.farAway);
		}
	}
	
	private void OnCollect ()
	{
		if(GameManager.Player.Inventory.AddItem ((CollectableItem)ScriptableObject.Instantiate (item))){
			stack--;
			if(destroyIfEmpty && stack <=0 ){
				Destroy(gameObject);
			}
		}
		GameManager.Player.Movement.PlayAnimation (GameManager.Player.Character.pickUp.name, GameManager.Player.Character.pickUp.length);
	}
	
	private IEnumerator FillSpot(){
		while(true){
			yield return new WaitForSeconds(fillRate);
			if(stack< maxStack){
				stack++;
			}
		}
	}
	
	private void OnMouseEnter ()
	{
		SlotContainer.disableMouseTalent = true;
	}
	
	private void OnMouseExit ()
	{
		SlotContainer.disableMouseTalent = false;
	}
}
