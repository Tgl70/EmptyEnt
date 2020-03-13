using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    static Vector3 respawnPosition;

    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isColliding;

    private void Start()
    {
        respawnPosition = transform.position;
    }

    private void Update()
    {
        isColliding = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isColliding)
        {
            controller.Move(respawnPosition-transform.position);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            transform.position = respawnPosition;
        }
        else if(collision.gameObject.tag == "CheckPoint")
        {
            respawnPosition = collision.gameObject.transform.position;
        }
    }
}
