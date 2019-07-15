using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player's RigidBody refrence
    private Rigidbody rb;
    private Animator anim;

    //controller variables
    [SerializeField]
    private bool ALLOW_JUMP = true;
    [SerializeField]
    private bool ALLOW_MOVEMENT = true;
    [SerializeField]
    private bool ALLOW_RUN = true;
    [SerializeField]
    private bool ALLOW_DASH = true;
    [SerializeField]
    private bool IS_GROUNDED = false;
    //base motion values
    [SerializeField]
    private float BASE_VELOCITY_PLAYER_MOVEMENT = 500f;
    [SerializeField]
    private float BASE_VELOCITY_JUMP_MULTIPLIER = 1f;
    [SerializeField]
    private float BASE_VELOCITY_RUN_MULTIPLIER = 2f;
    [SerializeField]
    private float BASE_VELOCITY_DASH_MULTIPLIER = 8f;
    [SerializeField]
    private float BASE_FALL_MULTIPLIER = 4f;

    //general purpose variables
    private Vector3 PlayerDirection = new Vector3(0, 0, 0);
    private float playerSpeed = 0;
    private bool isDash = false;
    private bool isJump = false;
    private float dashTime = 0.15f;
    private float dashCounter = 0.15f;
    private float jumpTime = 0.08f;
    private float jumpCounter = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   //movement controls start
        PlayerDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //movement controls end

        //dash start usig jump button instead
        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash Pressed");
            isDash = true && ALLOW_DASH;
        }
        //dash end
        //jump logic start
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true && ALLOW_JUMP && IS_GROUNDED;
        }
        //jump logic end
        /*Debug.Log(anim);*/
    }

    private void FixedUpdate()
    {
        if (IS_GROUNDED) //only for animation sake
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            if (rb.velocity.normalized.y > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
            else if (rb.velocity.normalized.y < 0)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
            }
            else
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
            }
        }


        //movement control start
        if (ALLOW_MOVEMENT)
        {
            if (PlayerDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerDirection), Time.deltaTime * 10);

            if (IS_GROUNDED)
            {
                playerSpeed = BASE_VELOCITY_PLAYER_MOVEMENT * (Input.GetButton("Run") ? BASE_VELOCITY_RUN_MULTIPLIER : 1);
            }
            else
            {
                playerSpeed = BASE_VELOCITY_PLAYER_MOVEMENT * (Input.GetButton("Run") ? BASE_VELOCITY_RUN_MULTIPLIER * 0.75f : 1 * 0.5f);
            }

            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                rb.velocity = PlayerDirection * playerSpeed * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
                anim.SetBool("move", true);
            }
            else
            {
                anim.SetBool("move", false);
            }
        }

        if (isDash)
        {
            Dash();
        }

        if (isJump)
        {
            Jump();
        }
        //jump fall and amplified gravity
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * BASE_FALL_MULTIPLIER * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (BASE_FALL_MULTIPLIER + 1) * Time.deltaTime;
        }
        else
        {
            rb.velocity += Vector3.up * Physics.gravity.y * Time.deltaTime * BASE_FALL_MULTIPLIER;
        }
        //jump fall and amplified gravity
    }
    //movement control end

    //dash start
    void Dash()
    {
        float dash_speed = BASE_VELOCITY_PLAYER_MOVEMENT * BASE_VELOCITY_DASH_MULTIPLIER;
        if (dashTime == dashCounter)
        {
            anim.SetBool("isDashing", true);
            rb.velocity = rb.transform.forward.normalized * dash_speed * Time.deltaTime;
            Debug.Log("Dash Velocity: " + dash_speed);

            ALLOW_MOVEMENT = false;
            ALLOW_JUMP = false;
            dashTime -= Time.deltaTime;
        }
        else if (dashTime <= 0)
        {
            anim.SetBool("isDashing", false);
            dashTime = dashCounter;
            ALLOW_MOVEMENT = true;
            ALLOW_JUMP = true;
            rb.velocity = new Vector3(0, 0, 0);
            isDash = false;
        }
        else
        {
            dashTime -= Time.deltaTime;
            rb.velocity = rb.transform.forward.normalized * dash_speed * Time.deltaTime;
        }
    }
    //dash end

    //jump start
    void Jump()
    {
        Debug.Log("inside jump");
        /*rb.velocity += Vector3.up * BASE_VELOCITY_PLAYER_MOVEMENT * BASE_VELOCITY_JUMP_MULTIPLIER * Time.deltaTime;*/
        /*rb.AddForce(Vector3.up * BASE_VELOCITY_PLAYER_MOVEMENT * BASE_VELOCITY_JUMP_MULTIPLIER * Time.deltaTime, ForceMode.Impulse);*/

        float jump_time = BASE_VELOCITY_PLAYER_MOVEMENT * BASE_VELOCITY_JUMP_MULTIPLIER;
        if (jumpTime == jumpCounter)
        {
            rb.velocity += Vector3.up * jump_time * Time.deltaTime;
            Debug.Log("Dash Velocity: " + jump_time);
            /*ALLOW_MOVEMENT = false;*/
            jumpTime -= Time.deltaTime;
        }
        else if (jumpTime <= 0)
        {
            jumpTime = jumpCounter;
            /*ALLOW_MOVEMENT = true;*/
            /*rb.velocity -= new Vector3(0, rb.velocity.y, 0);*/
            rb.AddForce(0, Physics.gravity.y * 2, 0, ForceMode.Impulse);
            isJump = false;
        }
        else
        {
            jumpTime -= Time.deltaTime;
            rb.velocity += Vector3.up * jump_time * Time.deltaTime;
        }
    }

    //jump end





    //setters and getters
    public void setGrounded()
    {
        IS_GROUNDED = true;
    }

    public void setNotGrounded()
    {
        IS_GROUNDED = false;
    }

    public bool isGrounded()
    {
        return IS_GROUNDED;
    }

}
