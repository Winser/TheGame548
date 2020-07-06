using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Camera cam;
    private float x;
    public Transform weapon;
    private Vector3 VorlPos;
    private float ray_lenght = 100f;
    public float mouseSense = 5f;
    public bool isAiming = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isAiming = MovableCharacter.IsAiming;
        Rotation();
    }

    private void Rotation()
    {
        if (isAiming)
        {
            Debug.DrawRay(weapon.position, weapon.forward * ray_lenght, Color.red);
           VorlPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                x = hit.point.x;
                weapon.LookAt(hit.point);
            }

            //weapon.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSense);

        }
        else
        {
            
        }
    }

}
