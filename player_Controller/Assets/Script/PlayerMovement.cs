using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Player Movement")]
    public float moveSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    [Header("ground Check")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask groundCheck;
    public bool grounded;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldowm;
    public float airMultipler;
    public bool readyToJump;
    public int timsJump;

    public KeyCode jumpKey = KeyCode.Space;

    [Header("Animation")]
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(orientation.transform.position, Vector3.down, playerHeight * 0.3f + 0.2f, groundCheck);
        Debug.DrawRay(orientation.transform.position, Vector3.down,Color.green);
        MoveInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
            animator.SetBool("IsJump", false);

        }
        else if (!grounded)
        {
            animator.SetBool("IsJump", true);
            rb.drag = 0;
        }
        else
        {
            
            rb.drag = 0;
        }
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded && timsJump < 2)
        {
           
            readyToJump = false;  
            Jump();
            Invoke(nameof(ResetJump), jumpCooldowm);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultipler, ForceMode.Force);
        }
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limiteVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limiteVel.x, rb.velocity.y, limiteVel.z);
        }
        
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
    }

    private void ResetJump()
    {
        timsJump = 0;
        readyToJump = true;
        //animator.SetBool("IsJump", false);
    }

}
