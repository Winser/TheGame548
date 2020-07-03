using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Enemy enemy;
    void Start()
    {
        enemy = gameObject.AddComponent<Enemy>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        EnemyHead head = collision.collider.GetComponent<EnemyHead>();
        if(head)
        {
            enemy.HitHead(50);
        }
        Destroy(gameObject);
    }
}
