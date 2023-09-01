using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float gravityVel = 2;
    bool gravityOn = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gravityOn)
        {
            rb.velocity = new Vector3(rb.velocity.x, gravityVel * Time.deltaTime, rb.velocity.z);
            gravityVel += 2f;

            // Max speed
            if(gravityVel > 80)
            {
                gravityVel = 50;
            }
        }
    }

    public void GravityOn()
    {
        gravityOn = true;

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void GravityOff()
    {
        gravityOn = false;

        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
