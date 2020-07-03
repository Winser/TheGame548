using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_sys : MonoBehaviour
{
    public float head = 100f;
    public float chest = 100f;
    public float right_arm = 100f;
    public float left_arm = 100f;
    public float right_leg = 100f;
    public float left_leg = 100f;

    public void Params(float h, float c, float r_a, float l_a, float r_l, float l_l)
    {
        head = h;
        chest = c;
        right_arm = r_a;
        left_arm = l_a;
        right_leg = r_l;
        left_leg = l_l;
    }

}
