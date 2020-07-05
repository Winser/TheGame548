using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_destroy : MonoBehaviour
{
    private void FixedUpdate()
    {
        Destroy(gameObject, Time.deltaTime * 5000f);
    }
}
