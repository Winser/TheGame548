using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    float hp;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().chest;

    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg;
        Debug.Log(hp);
    }
}
