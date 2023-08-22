using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovementThirdPerson : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
}
