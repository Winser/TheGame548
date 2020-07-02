using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Transform debugMesh;

    private PlayerData playerData;

    void Start()
    {
        this.playerData = GetComponent<PlayerData>();
    }

    void Update()
    {
        MovableCharacter character;
        playerData.selectedCharacter.TryGetComponent<MovableCharacter>(out character);
        if (character) {
            Vector3? mouseTargetPosition = this.getMouseTargetPosition();
            if (mouseTargetPosition != null) {
                if (this.debugMesh) {
                    this.debugMesh.position = (Vector3)mouseTargetPosition;
                }

                if (Input.GetMouseButton(0)) {
                    character.Move((Vector3)mouseTargetPosition);
                }
            }
        }

    }

    private Vector3? getMouseTargetPosition()
    {
        Camera camera = this.playerData.selectedCamera;
        if (camera) {
            Vector3 mouseOnScreenPosition = Input.mousePosition + new Vector3(0, 0, 1);
            Vector3 mouseOnWorldPosition = camera.ScreenToWorldPoint(mouseOnScreenPosition);
            Vector3 mouseDirection = mouseOnWorldPosition - camera.transform.position;
            Ray ray = new Ray(camera.transform.position, mouseDirection);
        
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
        }

        return null;
    }
}
