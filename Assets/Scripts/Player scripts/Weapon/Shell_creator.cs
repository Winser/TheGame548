﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_creator : MonoBehaviour
{

    public GameObject ShellPrefab;
    public float Velocity = 5f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject Shell = Instantiate(ShellPrefab, transform.position, transform.rotation);
            Shell.GetComponent<Rigidbody>().velocity = transform.parent.right * Velocity;
        }
       
    }
   
}
