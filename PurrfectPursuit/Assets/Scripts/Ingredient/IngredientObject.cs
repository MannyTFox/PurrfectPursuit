using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    bool gravityOn = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (gravityOn)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * Time.deltaTime, rb.velocity.z);
        }
    }

    public void ObjectInDecorativeMode(bool isObjectInDecorativeMode)
    {
        // When in cats mouth
        if (isObjectInDecorativeMode)
        {
            GravityOff();
        }
        // When its falling in the cauldron and when it is dropped on the ground
        else
        {
            GravityOn();
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
