using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private float rnd;
    private float turbo_rnd;
    private float pulce_rnd;
    // Update is called once per frame
    void Update()
    {
            rnd = Random.Range((float)-3, (float)3);
            turbo_rnd = Random.Range((float)0, (float)7);
            pulce_rnd = Random.Range((float)0.5, (float)4);
            this.gameObject.GetComponent<WindZone>().windMain = rnd;
            this.gameObject.GetComponent<WindZone>().windTurbulence = turbo_rnd;
            this.gameObject.GetComponent<WindZone>().windPulseMagnitude = pulce_rnd;

    }
}
