using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_non_control : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator myAnimator;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetInteger("CurrentAction", 2);
    }
}
