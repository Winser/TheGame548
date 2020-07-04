using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletVelocity = 20f;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
            this.GetComponent<ParticleSystem>().Play();
          
        }

    }
}
