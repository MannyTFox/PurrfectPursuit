 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public float movementSpeed = 5;
    public Transform orientation;
    public Transform playerObj;
    public Transform playerCenter;
    public Rigidbody rb;
    public PhysicMaterial playerPhysicsMat;
    float physicsMatFrictionValue = 0.5f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public float rotationSpeed;

    bool locked;

    private void Start()
    {
        locked = true;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        grounded = Physics.Raycast(playerCenter.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (locked == false)
        {
            MovePlayer();

            if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0)
            {
                RotationOfPlayer();
            }
        }

        // Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;

            // Set cat physics mat to normal friction so it doesnt slide off ramps
            playerPhysicsMat.dynamicFriction = physicsMatFrictionValue;
            playerPhysicsMat.staticFriction = 1;
            playerPhysicsMat.frictionCombine = PhysicMaterialCombine.Maximum;
        }
        else
        {
            rb.drag = groundDrag/2;

            // So cat wont stick to walls
            playerPhysicsMat.dynamicFriction = 0;
            playerPhysicsMat.staticFriction = 0;
            playerPhysicsMat.frictionCombine = PhysicMaterialCombine.Minimum;
        }

        #region Old Movement
        /*if (!locked)
        {
            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            //rotate player object
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
        }*/
        #endregion
    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            // Set delay for next jump
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public bool IsPlayerPressingMovementInput()
    {
        if(horizontalInput == 0 && verticalInput == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void RotationOfPlayer()
    {
        // Getting cam direction
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        move = Camera.main.transform.TransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);

        Vector3 camDir = Camera.main.transform.forward;
        camDir = Vector3.ProjectOnPlane(camDir, Vector3.up);

        // Rotate orientation
        orientation.forward = camDir;

        // Player object also rotates accordinly
        playerObj.forward = camDir;
    }

    public void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // If needed, limit velocity
        if(flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;

            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    public void LockPlayer()
    {
        locked = true;
    }

    public void UnlockPlayer()
    {
        locked = false;
    }
}
