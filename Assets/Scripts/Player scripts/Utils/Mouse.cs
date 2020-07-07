using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse
{
    public static Vector3 GetMousePositionOnScreen()
    {
        return Input.mousePosition + new Vector3(0, 0, 1);
    }
    public static Vector3 GetMousePositionOnWorld(Camera camera)
    {
        return camera.ScreenToWorldPoint(Mouse.GetMousePositionOnScreen());
    }

    public static RaycastHit? GetHit(Camera camera)
    {
        
        Vector3 mouseOnScreenPosition = Mouse.GetMousePositionOnScreen();
        Vector3 mouseOnWorldPosition = camera.ScreenToWorldPoint(mouseOnScreenPosition);
        Vector3 mouseDirection = mouseOnWorldPosition - camera.transform.position;
        Ray ray = new Ray(camera.transform.position, mouseDirection);
    
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit;
        }

        return null;
    }

    public static GameObject GetTargetObject(Camera camera) {
        RaycastHit? hit = Mouse.GetHit(camera);
        if (hit != null) {
            return ((RaycastHit)hit).collider.gameObject;
        }

        return null;
    }

    public static Vector3? GetTargetPoint(Camera camera) {
        RaycastHit? hit = Mouse.GetHit(camera);
        if (hit != null) {
            return ((RaycastHit)hit).point;
        }

        return null;
    }
}
