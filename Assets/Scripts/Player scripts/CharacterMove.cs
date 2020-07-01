using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Transform debugMesh;
    public MovableCharacter movableTarget;
    Camera mainCamera;

    void Start()
    {
        this.mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3? mouseTargetPosition = this.getMouseTargetPosition();
        if (mouseTargetPosition != null) {
            if (this.debugMesh) {
                this.debugMesh.position = (Vector3)mouseTargetPosition;
            }

            if (Input.GetMouseButton(0)) {
                this.movableTarget.Move((Vector3)mouseTargetPosition);
            }
        }

    }

    private Vector3? getMouseTargetPosition()
    {
        Vector3 mouseOnScreenPosition = Input.mousePosition + new Vector3(0, 0, 1);
        Vector3 mouseOnWorldPosition = this.mainCamera.ScreenToWorldPoint(mouseOnScreenPosition);
        Vector3 mouseDirection = mouseOnWorldPosition - this.mainCamera.transform.position;
        Ray ray = new Ray(mainCamera.transform.position, mouseDirection);
    
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return null;
    }
}
