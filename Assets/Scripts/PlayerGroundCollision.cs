using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollision : MonoBehaviour
{
    public PlayerController pc;

    void Start()
    {
        pc = GetComponent<PlayerController>();
    }
    //disabled all checks as only expecting collision
    //what to do when you hit something
    void OnCollisionEnter(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag == "Ground")
        {
            Debug.Log("We hit ground");
            pc.setGrounded();
        }
    }
    //what to do when you are in contact
    void OnCollisionStay(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag == "Ground")
        {
            /*Debug.Log("We are grounded");*/
            pc.setGrounded();
        }
    }
    //what to do when leave contact
    void OnCollisionExit(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag == "Ground")
        {
            Debug.Log("We are not grounded");
            pc.setNotGrounded();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        pc.setGrounded();
    }

    void OnTriggerStay(Collider other)
    {
        pc.setGrounded();
    }

    void OnTriggerExit(Collider other)
    {
        pc.setNotGrounded();
    }
}
