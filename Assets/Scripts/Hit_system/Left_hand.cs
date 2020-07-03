using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_hand : MonoBehaviour
{
    float hp;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().left_arm;

    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg;
        Debug.Log(hp);
    }
}
