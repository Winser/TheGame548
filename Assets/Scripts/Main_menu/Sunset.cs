using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunset : MonoBehaviour
{
    // Start is called before the first frame update
   
    private float r = 0.5f;
    private float g = 0.5f;
    private float b = 0.5f;
    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= Time.deltaTime/2;
        transform.rotation = Quaternion.Euler(rotation);

        this.GetComponent<Light>().color = new Color(r, g, b);
        r += Time.deltaTime/100;
        g -= Time.deltaTime / 100;
    }
}
