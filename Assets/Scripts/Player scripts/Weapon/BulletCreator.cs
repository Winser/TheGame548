using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject Shell;
    public float BulletVelocity = 20f;
    public bool is_automatic = false;
    public bool IsAiming = false;
    public int Ammo = 25;
    //cool donw timer
    public float CD;
    void Update()
    {
        CoolDown();
        Reload();
        if (CD > 3)
        {
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
    }
    void Reload()
    {
        if (Input.GetKeyDown("r"))
        {
            Ammo = 25;
            Shell.gameObject.SetActive(true);
            GameObject.Find("Reload_sound").GetComponent<AudioSource>().Play();
            CD = 0;
        }
    }
    void Shoot()
    {
        if (Ammo > 0)
        {
            GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * BulletVelocity;
            this.GetComponent<ParticleSystem>().Play();
            this.GetComponent<AudioSource>().Play();
            CD = 0f;
            IsAiming = false;
            Ammo -= 1;
        }
        else
        {

            Shell.gameObject.SetActive(false);
            GameObject.Find("Empty_sound").GetComponent<AudioSource>().Play();
            IsAiming = false;
        }

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
