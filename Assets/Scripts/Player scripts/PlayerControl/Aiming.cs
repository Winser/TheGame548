using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Camera cam;
    private float x;
    public Transform weapon;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                x = hit.point.x;
                weapon.LookAt(hit.point);
                Vector3 bullet_creator_pos = GameObject.Find("Bullet_creator").GetComponent<Transform>().position;
                GameObject.Find("Bullet_creator").GetComponent<LineRenderer>().SetPosition(0, bullet_creator_pos); ;
                GameObject.Find("Bullet_creator").GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }

        }
        else
        {
            GameObject.Find("Bullet_creator").GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            GameObject.Find("Bullet_creator").GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
        }
    }

}
