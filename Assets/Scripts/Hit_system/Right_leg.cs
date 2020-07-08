using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right_leg : MonoBehaviour
{
    public float hp;
    public float Armor = 1;
    private float DMG = 15;
    public GameObject unit;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().right_leg;

    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg * Armor;
        Debug.Log(hp);
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "WolfAttack")
        {
            OnHit(DMG);
        }
    }
}
