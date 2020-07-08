using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Wolf_ai : MonoBehaviour
{

    private GameObject player_pos;
    public float dist;
    NavMeshAgent nav;
    private Animator animator;
    public float visibility_radius;
    void Start()
    {
        visibility_radius = 20f;
        player_pos = GameObject.Find("FGC_Male_Char_Brad_Lite");
        nav = GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(player_pos.transform.position, transform.position);
        if(dist > visibility_radius)
        {
            nav.enabled = false;
            animator.SetBool("idle01", true);
            animator.SetBool("attack", false);
            animator.SetBool("run", false);
        }
        if (dist < visibility_radius && dist> 3f)
        {
            animator.SetBool("attack", false);
            animator.SetBool("idle01", false);
            nav.enabled = true;
            nav.SetDestination(player_pos.transform.position);
            animator.SetBool("run",true);

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
}
