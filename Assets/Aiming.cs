using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
   
    public Transform weapon;
    private float ray_lenght = 100f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(weapon.position, weapon.forward * ray_lenght,Color.red);
        Ray ray = new Ray(weapon.position, weapon.forward * ray_lenght);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit));
        Rotation();

    }

    private void Rotation ()
    {
       

        
    }
  
}
