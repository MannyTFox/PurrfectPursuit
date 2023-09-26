using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationScript : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    PlayerMovement catMovScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        catMovScript = transform.parent.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x > 0.1f && catMovScript.IsPlayerPressingMovementInput() || 
            rb.velocity.x < -0.1f && catMovScript.IsPlayerPressingMovementInput() || 
            rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
        {
            anim.SetBool("running", true);

            // Start cat step loop
            if (AudioManager.audioManagerInstance.IsCatStepCoroutinePlaying() == false)
            {
                StartCoroutine(AudioManager.audioManagerInstance.CatWalkSoundLoop());
            }
        }
        else
        {
            anim.SetBool("running", false);

            // Stop cat step loop
            AudioManager.audioManagerInstance.CatWalkLoopBoolLever(false);
        }
    }
}
