using UnityEngine;
using System.Collections;

public class ChangeMovementType : MonoBehaviour {
	private void OnControllerChange (string sel)
	{
		switch (sel) {
		case "Default":
			GameManager.Player.Movement.controllerType = ThirdPersonMovement.ControllerType.Normal;
			break;
		case "Mouse":
			GameManager.Player.Movement.controllerType = ThirdPersonMovement.ControllerType.MouseRotation;
			break;
		case "Click":
			GameManager.Player.Movement.controllerType = ThirdPersonMovement.ControllerType.ClickToMove;
			GameManager.Player.Movement.Stop();
			break;
		}
	}
}
