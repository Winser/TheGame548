using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    float hp;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().head;
       
    }
    public void OnHit(float Dmg)
    {
        hp -=  Dmg;
        Debug.Log(hp);
    }
}
