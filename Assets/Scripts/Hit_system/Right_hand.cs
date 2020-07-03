using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_hand : MonoBehaviour
{
    float hp;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().right_arm;

    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg;
        Debug.Log(hp);
    }
}
