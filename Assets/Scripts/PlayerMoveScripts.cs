using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScripts : MonoBehaviour
{
    public CharacterController characterController;

    public float speed = 12f;
    public float gravity = -9.18f * 2;
    public float jumpHeight = 10f;

    public Transform groundCheck;
    public float groundDistance = 1.0f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGround;
    bool isMoving;

    private Vector3 lastPositon = new Vector3(0f, 0f, 0f);
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        //ground check
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the default velocity
        if(isGround && velocity.y<0)
        {
            velocity.y = -2f; 
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //Actually moving the player

        characterController.Move(move * speed * Time.deltaTime);

       //Check player can jump

        if(Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }


        //Falling down

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if(lastPositon != gameObject.transform.position && isGround == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPositon = gameObject.transform.position;
    }
}
