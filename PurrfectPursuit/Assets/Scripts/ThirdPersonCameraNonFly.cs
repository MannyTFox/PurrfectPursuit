 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCameraNonFly : MonoBehaviour
{

    [Header("References")]
    public float movementSpeed = 5;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public bool locked;

    private void Start()
    {
        locked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!locked)
        {
            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            //rotate player object
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

                // Apply movement to the rigidbody
                rb.velocity = inputDir.normalized * movementSpeed;

            }
            else
            {
                // Apply break to the rigidbody
                rb.velocity = new Vector3(0, 0, 0);
            }

        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }


    }
}
