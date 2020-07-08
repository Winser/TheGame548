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
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 25), "Left click - move");
        GUI.Label(new Rect(0, 25, 200, 25), "1 - aiming");
        GUI.Label(new Rect(0, 50, 300, 25), "Aiming state + Left click - shoot");
        GUI.Label(new Rect(0, 75, 300, 25), "Right click - pick up item");
        GUI.Label(new Rect(0, 100, 300, 25), "Right mouse button hold - camera rotation");
        GUI.Label(new Rect(0, 125, 300, 25), "Scroll down/up - approximation/estrangement");
        GUI.Label(new Rect(0, 150, 200, 25), "Q - inventory");
        GUI.Label(new Rect(0, 175, 200, 25), "2 - run");
    }
}
