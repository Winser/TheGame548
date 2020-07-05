using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletVelocity = 20f;
    public bool is_automatic = false;
    public bool IsAiming = false;
    //cool donw timer
    public float CD;
    void Update()
    {
        CoolDown();
        if (Input.GetKeyDown("1"))
        {
            if (IsAiming)
            {
                IsAiming = false;
            }
            else
            {
                IsAiming = true;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsAiming)
            {
                if (!is_automatic)
                {
                    Shoot();
                }
                else
                {
                   // Rifle_shoot();
                }
            }
        }
    }
    void Shoot ()
    {
        
            GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
            this.GetComponent<ParticleSystem>().Play();
            this.GetComponent<AudioSource>().Play();
            CD = 0f;
            IsAiming = false;
        

    }
    private void CoolDown()
    {
        CD += Time.deltaTime;
    }
    
   /* void Rifle_shoot()
    {
        
            GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
            this.GetComponent<ParticleSystem>().Play();
            this.GetComponent<AudioSource>().Play();
    }*/
}
