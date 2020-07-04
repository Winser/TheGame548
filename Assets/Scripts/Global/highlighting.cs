using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class highlighting : MonoBehaviour
{
    
    private void OnMouseEnter()
    {
        if(tag == "Stuff" || tag == "Wolf")
        {
            GetComponent<Light>().enabled = true;
        }
    }
    private void OnMouseExit()
    {
        if (tag == "Stuff" || tag == "Wolf")
        {
            GetComponent<Light>().enabled = false;
        }
    }

}
