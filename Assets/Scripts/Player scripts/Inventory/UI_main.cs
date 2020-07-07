using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_main : MonoBehaviour
{
    private int MaxBullets;
    private int CurrBullets;
    private BulletCreator BulletSource;
    void Start()
    {
       BulletSource = GameObject.Find("Bullet_creator").GetComponent<BulletCreator>();
        MaxBullets = BulletSource.MaxAmmo;
    }
    void Update()
    {
        CurrBullets = BulletSource.Ammo;
        GameObject.Find("BulletsKolVo").GetComponent<Text>().text = CurrBullets + " / " + MaxBullets;
    }
}
