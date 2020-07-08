using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public float hp;
    public float Armor = 1;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().head;
       
    }
    public void OnHit(float Dmg)
    {
        hp -=  Dmg * Armor;
        Debug.Log(hp);
    }
   
}
