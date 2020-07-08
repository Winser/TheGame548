using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public ControlledСharacter SelectedCharacter;
    public Camera SelectedCamera;
    public InputController InputController;

    private CharacterSelect _characterSelect;
    private CharacterMove _characterMove;

    private void Awake() {
        this.InputController = gameObject.AddComponent(typeof(InputController)) as InputController;
    }

    private void Update() {
    }
    private void FixedUpdate() {
        
    }
}
