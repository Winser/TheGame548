using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health_sys hs;
    void Start()
    {
        hs = gameObject.AddComponent<Health_sys>();
        hs.Params(100,100,100,100,100,100);
    }

    public void HitHead(float Dmg)
    {
        this.hs.head -= Dmg;
        Debug.Log(this.hs.head);
    }
}
