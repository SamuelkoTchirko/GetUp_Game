using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum State
    {
        Walking,
        OnRope,
        OnRopeAvailable,
        Grabbing,
        GrabFlowerAvailable,
        Falling,
    }

    private State state;

    public Rigidbody2D rb;

    public Joystick joystick;

    private float runSpeed = 400f;
    private float airSpeed = 350f;

    float horizontalMove = 0f;

    public Animator animator;

    GameObject actionObject;
    GameObject grabbedRope;
    GameObject grabbedFlower;

    //Joystick handle
    GameObject handle;

    int jumpTotal = 1;

    private Vector2 finalDir = Vector2.zero;

    float isGroundedRayLength = 0.1f;

    LayerMask layerMaskForGrounded;

    // Start is called before the first frame update
    void Start()
    {
        layerMaskForGrounded = LayerMask.GetMask("Ground");

        handle = GameObject.Find("Handle");

        state = State.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Walking:
                if(grabbedRope != null)
                {
                    grabbedRope.GetComponent<HingeJoint2D>().connectedBody = null;
                    grabbedRope.GetComponent<HingeJoint2D>().enabled = false;
                    grabbedRope = null;
                }
                handleControls();
                break;

            case State.Falling:
                if (grabbedRope != null)
                {
                    grabbedRope.GetComponent<HingeJoint2D>().connectedBody = null;
                    grabbedRope.GetComponent<HingeJoint2D>().enabled = false;
                    grabbedRope = null;
                }
                handleControls();
                break;

            case State.OnRope:
                handleControlsRope();
                break;
            case State.OnRopeAvailable:
                handleControls();
                break;
            case State.Grabbing:
                handleControlsGrabbing();
                break;
            case State.GrabFlowerAvailable:
                handleControls();
                break;
        }
        //Debug.Log(state);

        animator.SetFloat("horizontal", horizontalMove);
        animator.SetFloat("speed", rb.velocity.sqrMagnitude);
        animator.SetFloat("jump", rb.velocity.y);
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Walking:
                handleMovement(false, false);
                break;
            case State.Falling:
                handleMovement(false, true);
                break;
            case State.OnRopeAvailable:
                handleMovement(false, true);
                break;
            case State.OnRope:
                handleMovement(true, false);
                break;
            case State.GrabFlowerAvailable:
                handleMovement(false, true);
                break;
            case State.Grabbing:
                handleMovement(false, false);
                break;
        }
    }

    void handleMovement(bool onRope, bool falling)
    {
        if (onRope == false)
        {
            if (joystick.Vertical != 0 && joystick.Horizontal != 0)
            {
                rb.velocity = new Vector2(horizontalMove * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
            else if (joystick.Vertical == 0 && joystick.Horizontal == 0)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
        else if(onRope == true)
        {
            if (joystick.Vertical != 0 && joystick.Horizontal != 0)
            {
                rb.velocity = new Vector2(horizontalMove * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
            else if (joystick.Vertical == 0 && joystick.Horizontal == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }
        else if(falling == true)
        {
            if (joystick.Vertical != 0 && joystick.Horizontal != 0)
            {
                rb.velocity = new Vector2(horizontalMove * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
            else if (joystick.Vertical == 0 && joystick.Horizontal == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }
    }

    void handleControls()
    {
        if (joystick.Horizontal >= .2f)
        {
            horizontalMove = 1f;
        }
        else if (joystick.Horizontal <= -.2f)
        {
            horizontalMove = -1f;
        }
        else
        {
            horizontalMove = 0f;
        }

        if (isGrounded)
        {
            state = State.Walking;
            jumpTotal = 1;
        }
    }

    void handleControlsRope()
    {
        if (joystick.Horizontal >= .2f)
        {
            horizontalMove = 1f;
        }
        else if (joystick.Horizontal <= -.2f)
        {
            horizontalMove = -1f;
        }
        else
        {
            //horizontalMove = 0f;
        }

        if (isGrounded)
        {
            state = State.Walking;
            jumpTotal = 1;
        }
    }

    void handleControlsGrabbing()
    {
        if(joystick.Vertical != 0 && joystick.Horizontal != 0)
        {
            //Vector2 aimDir = new Vector2(joystick.transform.position.x, joystick.transform.position.y);
            Vector2 aimDir = joystick.transform.position - handle.transform.position;
            finalDir = -(aimDir - rb.position);
            Debug.DrawLine(rb.position,finalDir);
        }
        

        if (isGrounded)
        {
            if(state == State.Grabbing)
            {

            }
            else
            {
                state = State.Walking;
                jumpTotal = 1;
            }
        }
    }

    public bool isGrounded
    {
        get
        {
            Vector3 position = transform.position;
            position.y = GetComponent<Collider2D>().bounds.min.y + 0.1f;
            float length = isGroundedRayLength + 0.1f;
            Debug.DrawRay(position, Vector3.down * length);
            bool grounded = Physics2D.Raycast(position, Vector3.down, length, layerMaskForGrounded.value);

            return grounded;
        }
    }

    public void actionBtnClick()
    {
        if(state == State.OnRopeAvailable)
        {
            state = State.OnRope;
            jumpTotal = 1;
            grabbedRope.GetComponent<HingeJoint2D>().enabled = true;
            grabbedRope.GetComponent<HingeJoint2D>().connectedBody = transform.GetComponent<Rigidbody2D>();
        }
        if (state == State.GrabFlowerAvailable)
        {
            state = State.Grabbing;
            jumpTotal = 1;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void jump()
    {
        if(jumpTotal != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1 * airSpeed * Time.fixedDeltaTime);
            jumpTotal--;
        }

        if(state == State.OnRope)
        {
            state = State.Falling;
        }
        if (state == State.Grabbing)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(finalDir * 200);
            state = State.Falling;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        actionObject = collider2D.transform.gameObject;


        //Handle Rope Enter
        if (actionObject.name == "Rope" || actionObject.name == "RopeEnd")
        {
            grabbedRope = collider2D.transform.parent.Find("RopeEnd").gameObject;
            if (state != State.OnRope)
            {
                state = State.OnRopeAvailable;
            }
        }

        //Handle Grabbing
        else if (actionObject.name == "GrabFlower")
        {
            grabbedFlower = actionObject;
            if (state != State.Grabbing)
            {
                state = State.GrabFlowerAvailable;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(state == State.OnRopeAvailable)
        {
            if(state != State.OnRope)
            {
                state = State.Walking;
            }
        }
        if (state == State.GrabFlowerAvailable)
        {
            if (state != State.Grabbing)
            {
                state = State.Walking;
            }
        }
    }
}
