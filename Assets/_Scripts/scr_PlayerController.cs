using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerController : MonoBehaviour
{
    #region - Variables -

    //references
    private Rigidbody rb;
    private Transform orientation;

    [Header("Movement")]
    [SerializeField]
    private float walkSpeed = 9;
    [SerializeField]
    private float sprintSpeed = 13;
    [SerializeField]
    private float groundDrag = 5;
    [SerializeField]
    private float airMultiplier = .3f;
    [SerializeField]
    private float maxSlopeAngle = 40;

    private float speed;
    private float targetSpeed;
    private float lastTargetSpeed;
    private Vector3 moveInput;
    private Vector3 moveDir;
    private RaycastHit slopeHit;

    public bool walkingBack;
    public bool sprintHeld;

    [Header("Crouching")]
    [SerializeField]
    private float crouchScale;
    [SerializeField]
    private float crouchSpeed;

    private Vector3 originalScale;
    private Transform camPos;
    public bool crouchHeld;

    [Header("Jumping")]
    [SerializeField]
    private float force = 3;
    [SerializeField]
    private float jumpCooldown = .3f;

    private Vector3 normalVector = Vector3.up;
    public bool jumpingHeld;
    private bool readyToJump = true;


    [Header("Sliding")]
    [SerializeField]
    private float slideDuration;
    [SerializeField]
    private float slideSpeed;

    private bool slideHeld;

    [Header("Ground")]
    [SerializeField]
    private LayerMask ground;

    GameObject currentGroundObject;
    public bool isGrounded;
    private bool cancellingGrounded;

    #endregion

    #region - Unity Methods -

    private void Awake()
    {
        //refs
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.GetChild(0);

        //initializations
        originalScale = transform.localScale;
        camPos = transform.GetChild(2);

    }

    private void FixedUpdate()
    {
        Move();
        DragControl();
        SpeedLimiting();
        SetSpeed();

        Jump();
    }

    private void Update()
    {
        StateHandler();
    }

    #endregion

    #region - States -

    public MovementState state { get; private set; }

    private void StateHandler()
    {
        scr_UIManager.Instance.UpdateState(state);

        if (isGrounded)
        {
            if (slideHeld && rb.velocity.y <= 0)
            {
                state = MovementState.Sliding;
                targetSpeed = slideSpeed;
            }
            else if (crouchHeld)
            {
                state = MovementState.Crouching;
                targetSpeed = crouchSpeed;
            }
            else if (sprintHeld && !walkingBack && moveDir != Vector3.zero)
            {
                state = MovementState.Sprinting;
                targetSpeed = sprintSpeed;
            }
            else if (rb.velocity.magnitude > 0)
            {
                state = MovementState.Walking;
                targetSpeed = walkSpeed;
            }
            else
            {
                state = MovementState.Idle;
            }
        }
        else
        {
            state = MovementState.Air;
        }
    }

    #endregion

    #region - Movement -


    private void Move()
    {
        rb.useGravity = !OnSlope();
        var stopSpeed = speed * .8f;


        moveDir = orientation.forward * moveInput.z + orientation.right * moveInput.x;

        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDir(moveDir) * speed, ForceMode.Impulse);
            
        }

        else if (isGrounded)
            rb.AddForce(moveDir.normalized * speed, ForceMode.Impulse);
        else
            rb.AddForce(moveDir.normalized * speed * airMultiplier, ForceMode.Impulse);

        
        

        walkingBack = moveInput.z < 0;
        scr_UIManager.Instance.UpdateSpeed(rb.velocity.magnitude);
    }

    private void SpeedLimiting()
    {
        if (!isGrounded)
            return;

        if (OnSlope())
        {
            if (rb.velocity.magnitude > speed)
                rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void DragControl()
    {
        rb.drag = isGrounded ? groundDrag : 0;
    }

    public void OnMove(InputValue value)
    {
        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void SetSpeed()
    {
        if (state == MovementState.Sliding)
            speed = targetSpeed;

        else if (targetSpeed - lastTargetSpeed > 4 && speed > 0)
        {
            StartCoroutine(SmoothSpeed(15));
        }
        else if (targetSpeed - lastTargetSpeed < -4 && speed > 0)
        {
            StartCoroutine(SmoothSpeed(50));
        }
        else
            speed = targetSpeed;

        lastTargetSpeed = targetSpeed;
    }

    private IEnumerator SmoothSpeed(float lerpSpeed)
    {
        float time = 0;
        float difference = Mathf.Abs(targetSpeed - speed);
        float startSpeed = speed;

        while (time < difference)
        {
            speed = Mathf.Lerp(startSpeed, targetSpeed, time / difference);
            time += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }


    private Vector3 GetSlopeMoveDir(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    #endregion

    #region - Sprinting -

    public void OnSprint(InputValue value)
    {

        if (!sprintHeld)
        {
            sprintHeld = true;
        }
        else
        {
            sprintHeld = false;
        }
    }


    #endregion

    #region - Jumping -

    public void OnJump(InputValue value)
    {
        if (state == MovementState.Sliding || state == MovementState.Crouching)
            return;
        /*
        if (state == MovementState.Crouching)
        {
            transform.localScale = originalScale;
            crouchHeld = false;
            return;
        }*/
        jumpingHeld = !jumpingHeld;
    }
    private void Jump()
    {
        if (jumpingHeld && isGrounded && readyToJump)
        {
            readyToJump = false;
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            rb.AddForce(normalVector * force * 0.5f);

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    #endregion

    #region - Crouching -

    public void OnCrouch(InputValue value)
    {
        if (state == MovementState.Sprinting)
            StartCoroutine(Slide());
        else
            Crouch();
    }

    private void Crouch()
    {
        if (!isGrounded)
            return;

        if (!crouchHeld)
        {
            crouchHeld = true;
            transform.localScale = new Vector3(1, crouchScale, 1);
        }
        else
        {
            crouchHeld = false;
            transform.localScale = originalScale;
        }
    }

    #endregion

    #region - Sliding -

    private IEnumerator Slide()
    {
        Vector3 currentDir = orientation.forward;
        slideHeld = true;
        transform.localScale = new Vector3(1, crouchScale, 1);
        //rb.AddForce(currentDir * slideSpeed);
        yield return new WaitForSeconds(slideDuration);
        slideHeld = false;
        transform.localScale = originalScale;
        //Crouch();
    }

    #endregion

    #region - Ground Check -

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, .3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public bool OnStairs()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + new Vector3(0, .35f, 0), .48f, ground);
        foreach (var col in cols)
        {
            if (col.transform.tag == "Stairs")
            {
                return true;
            }
        }
        return false;
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }


    private void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Vector3 normal = contact.normal;
            if (IsFloor(normal))
            {
                isGrounded = true;
                normalVector = normal;
                currentGroundObject = contact.otherCollider.gameObject;
                return;
            }

            else if (currentGroundObject == contact.otherCollider.gameObject)
            {
                isGrounded = false;
                currentGroundObject = null;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == currentGroundObject)
        {
            isGrounded = false;
            currentGroundObject = null;
        }
    }

    #endregion

}


