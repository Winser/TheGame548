using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{

    public Transform weapon;
    private Vector3 start;
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
            weapon.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * mouseSense);
            weapon.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSense);

        }
        else
        {
            
        }
    }

}
