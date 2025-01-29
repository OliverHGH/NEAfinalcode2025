using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float speed = 15f;
    public CharacterController playertomove;
    Vector3 velocity;
    public float gravity = -14f;
    public Transform groundcheck;
    public float gcheckradius = 0.3f;
    public LayerMask groundmask;
    bool isGround;
    bool IG2;
    public bool paused = false;
    void Update()
    {
        if(!paused)
        {
            isGround = Physics.CheckSphere(groundcheck.position, gcheckradius, groundmask);
            if (isGround && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (Input.GetButtonDown("Jump"))
            {
                if (isGround) 
                {
                    velocity.y = 7f;
                }
            }
            Vector3 movement = transform.right * x + transform.forward * z;
            playertomove.Move(movement * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            playertomove.Move(velocity * Time.deltaTime);
        }

    }
}
