using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Wolf_ai : MonoBehaviour
{

    private GameObject player_pos;
    public float dist;
    //bool random = false;
   // public float random_dist;
    NavMeshAgent nav;
    private Animator animator;
    public float visibility_radius;
    //private float random_poin;
   // private float timer;
    void Start()
    {
        visibility_radius = 40f;
        player_pos = GameObject.Find("FGC_Male_Char_Brad_Lite");
        nav = GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       // timer += Time.deltaTime;
        
        dist = Vector3.Distance(player_pos.transform.position, transform.position);
        //MoveToRandomPoint();
        if(dist > visibility_radius)
        {
            nav.enabled = false;
            animator.SetBool("idle01", true);
            animator.SetBool("attack", false);
            animator.SetBool("run", false);
            //random = true;
        }
        if (dist < visibility_radius && dist> 3f)
        {
            animator.SetBool("attack", false);
            animator.SetBool("idle01", false);
            nav.enabled = true;
            nav.SetDestination(player_pos.transform.position);
            animator.SetBool("run",true);
          //  random = false;

        }
        if(dist <= 3f)
        {
            animator.SetBool("run", false);
            nav.enabled = false;
            animator.SetBool("attack", true);
        }
        if (player_pos.tag == "Dead")
        {
            visibility_radius = 0f;
        }
    }

   /* void MoveToRandomPoint ()
    {
        if (timer > 5)
        {
            random_poin = Random.Range(15, 250);
            random_dist = Vector3.Distance(new Vector3(random_poin, random_poin), transform.position);
            timer = 0f;
        }
        if (random)
        {
            nav.enabled = true;
           
            nav.SetDestination((new Vector3(random_poin, random_poin)));
        }
    }
    */
}
