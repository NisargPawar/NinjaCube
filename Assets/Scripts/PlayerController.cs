using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player's RigidBody refrence
    private Rigidbody rb;

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
    private float BASE_VELOCITY_JUMP_MULTIPLIER = 3f;
    [SerializeField]
    private float BASE_VELOCITY_RUN_MULTIPLIER = 2f;
    [SerializeField]
    private float BASE_VELOCITY_DASH_MULTIPLIER = 4f;

    //general purpose variables
    private Vector3 PlayerDirection = new Vector3(0, 0, 0);
    private float playerSpeed = 0;
    private bool isDash = false;
    private float dashTime = 0.2f;
    private float dashCounter = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   //movement controls start
        PlayerDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //movement controls end

        //dash start usig jump button instead
        if (Input.GetButtonDown("Jump"))
        {
            isDash = true && ALLOW_DASH;
        }
        //dash end
    }

    private void FixedUpdate()
    {
        //movement control start
        if (ALLOW_MOVEMENT)
        {
            if (PlayerDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerDirection), Time.deltaTime * 10);

            playerSpeed = BASE_VELOCITY_PLAYER_MOVEMENT * (Input.GetButton("Run") ? BASE_VELOCITY_RUN_MULTIPLIER : 1);
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                rb.velocity = PlayerDirection * playerSpeed * Time.deltaTime;
            }
            else
            {
                rb.velocity = PlayerDirection * 0;
            }
        }
        if (isDash)
        {
            Dash();
        }
    }
    //movement control end

    //dash start
    void Dash()
    {
        float dash_speed = BASE_VELOCITY_PLAYER_MOVEMENT * BASE_VELOCITY_DASH_MULTIPLIER;
        if (dashTime == dashCounter)
        {
            rb.velocity = rb.transform.forward.normalized * dash_speed * Time.deltaTime;
            Debug.Log("Dash Velocity: " + dash_speed);
            ALLOW_MOVEMENT = false;
            dashTime -= Time.deltaTime;
        }
        else if (dashTime <= 0)
        {
            dashTime = dashCounter;
            ALLOW_MOVEMENT = true;
            rb.velocity = new Vector2(0, 0);
            isDash = false;
        }
        else
        {
            dashTime -= Time.deltaTime;
            rb.velocity = rb.transform.forward.normalized * dash_speed * Time.deltaTime;
        }
    }
    //dash end

}
