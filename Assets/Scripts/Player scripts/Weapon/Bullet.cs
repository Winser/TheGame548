using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Health_sys enemy;
    public float Bullet_dmg;
    void Start()
    {
        enemy = gameObject.AddComponent<Health_sys>();
        
    }
    private void FixedUpdate()
    {
        Destroy(gameObject, Time.deltaTime * 5000f);
        if (tag == "Tera")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        EnemyHead head = collision.collider.GetComponent<EnemyHead>();
        Chest chest = collision.collider.GetComponent<Chest>();
        Right_hand right_Hand = collision.collider.GetComponent<Right_hand>();
        Left_hand left_Hand = collision.collider.GetComponent<Left_hand>();
        Right_leg right_Leg = collision.collider.GetComponent<Right_leg>();
        Left_leg left_Leg = collision.collider.GetComponent<Left_leg>();

        if(head)
        {
            head.OnHit(Bullet_dmg*2);
            Debug.Log(collision.gameObject.name);
            //enemy.Head(Bullet_dmg * 2);
            
        }
        if(chest)
        {
            chest.OnHit(Bullet_dmg);
            Debug.Log(collision.gameObject.name);
        }
        if(right_Hand)
        {
            right_Hand.OnHit(Bullet_dmg * 0.8f);
            Debug.Log(collision.gameObject.name);
        }
        if (left_Hand)
        {
            left_Hand.OnHit(Bullet_dmg * 0.8f);
            Debug.Log(collision.gameObject.name);
        }
        if (right_Leg)
        {
            right_Leg.OnHit(Bullet_dmg * 0.8f);
            Debug.Log(collision.gameObject.name);
        }
        if (left_Leg)
        {
            left_Leg.OnHit(Bullet_dmg * 0.8f);
            Debug.Log(collision.gameObject.name);
        }
        Destroy(gameObject);
        
    }
}
