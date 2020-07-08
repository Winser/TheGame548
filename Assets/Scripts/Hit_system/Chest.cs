using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float hp;
    public float Armor = 1;
    public GameObject unit;
    private float DMG = 30;
    private void Start()
    {
        hp = unit.GetComponent<Health_sys>().chest;

    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg * Armor;
        Debug.Log(hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WolfAttack")
        {
            OnHit(DMG);
        }
    }
}
