using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_creator : MonoBehaviour
{

    public GameObject ShellPrefab;
    public float Velocity = 5f;
    public bool IsAiming = false;
    public float CD;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CD > 3f)
        {
            if (Input.GetKeyDown("1"))
            {

                IsAiming = true;
            }
            if (IsAiming)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject Shell = Instantiate(ShellPrefab, transform.position, transform.rotation);
                    Shell.GetComponent<Rigidbody>().velocity = transform.parent.right * Velocity;
                    IsAiming = false;
                    CD = 0;
                }
            }
        }
        CD += Time.deltaTime;
    }
}
