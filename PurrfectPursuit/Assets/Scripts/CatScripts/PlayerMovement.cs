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
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        grounded = Physics.Raycast(playerCenter.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

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
            /*//rotate orientation
            Vector3 viewDir = this.gameObject.transform.position - new Vector3(Camera.main.transform.position.x,
                                                                       this.gameObject.transform.position.y,
                                                                       Camera.main.transform.position.z);
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
            }*/
        #endregion
    }

    private void FixedUpdate()
    {
        if (locked == false)
        {
            MovePlayer();
            RotationOfPlayer();
        }
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
        // Old
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
        // Camera Direction
        Vector3 viewDir = this.gameObject.transform.position - new Vector3(Camera.main.transform.position.x,
                                                                   this.gameObject.transform.position.y,
                                                                   Camera.main.transform.position.z);

        // Orientation always looking towards camera
        orientation.forward = viewDir.normalized;

        // Get direction to move towards
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // If player is not pressing anything
        if (inputDir != Vector3.zero)
        {
            // Player object looks towards direction
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
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
