using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletVelocity = 20f;
    public bool is_automatic = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!is_automatic)
            {

                Shoot();
            }
            else
            { 
                Rifle_shoot();
            }
        }
    }
    void Shoot ()
    {
        GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
        this.GetComponent<ParticleSystem>().Play();
        this.GetComponent<AudioSource>().Play();
    }
    void Rifle_shoot()
    {
        for (float i = 0; i < 3; i++)
        {
            GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
            this.GetComponent<ParticleSystem>().Play();
            this.GetComponent<AudioSource>().Play();
        }
    }
}
