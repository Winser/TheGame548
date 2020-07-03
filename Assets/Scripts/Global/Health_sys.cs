using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_sys : MonoBehaviour
{
  
    public float head = 150f;
    public float chest = 100f;
    public float right_arm = 100f;
    public float left_arm = 100f;
    public float right_leg = 100f;
    public float left_leg = 100f;

    public void Health_set(float h, float c, float ra, float la, float rl, float ll)
    {
        head = h;
        chest = c;
        right_arm = ra;
        left_arm = la;
        right_leg = rl;
        left_leg = ll;
    }

}   


