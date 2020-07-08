using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDead : MonoBehaviour
{
    public bool dead = false;
    private Animator anim;
    public GameObject Head;
    public GameObject chest;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject LeftLeg;
    public GameObject RightLeg;
    public GameObject Ragdoll;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Head.GetComponent<EnemyHead>().hp <= 0 || chest.GetComponent<Chest>().hp <= 0 || LeftArm.GetComponent<Left_hand>().hp <=-25 || RightArm.GetComponent<Right_hand>().hp <= -25 
            || LeftLeg.GetComponent<Left_leg>().hp <= -25 || RightLeg.GetComponent<Right_leg>().hp <= - 25)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }


        if (dead && tag == "Player")
        {
            anim.Play("Dying");
            tag = "Dead";
        }
        if(dead && tag == "Wolf")
        {
            anim.Play("dead");
            tag = "Dead";
            Instantiate(Ragdoll,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }


}
