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

    private float speed;
    private float targetSpeed;
    private float lastTargetSpeed;
    private Vector3 moveDir;

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
    [SerializeField]
    private float radius = .5f;

    #endregion

    #region - Unity Methods -

    private void Awake()
    {
        //refs
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.GetChild(0);

        //initializations
        speed = walkSpeed;

        originalScale = transform.localScale;
        camPos = transform.GetChild(2);
    }

    private void FixedUpdate()
    {
        Move();
        DragControl();
        SpeedLimiting();
        SetSpeed();
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

        if (IsGrounded() && slideHeld)
        {
            state = MovementState.Sliding;
            targetSpeed = slideSpeed;
        }
        else if (IsGrounded() && crouchHeld)
        {
            state = MovementState.Crouching;
            targetSpeed = crouchSpeed;
        }
        else if (IsGrounded() && sprintHeld && !walkingBack)
        {
            state = MovementState.Sprinting;
            targetSpeed = sprintSpeed;
        }
        else if (IsGrounded() && rb.velocity.magnitude > .1f)
        {
            state = MovementState.Walking;
            targetSpeed = walkSpeed;
        }
        else if (IsGrounded())
        {
            state = MovementState.Idle;
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
        rb.AddForce(Vector3.down * Time.deltaTime * 1500);

        Vector3 move = orientation.forward * moveDir.z + orientation.right * moveDir.x;

        if (IsGrounded())
            rb.AddForce(move.normalized * speed * 10, ForceMode.Force);
        else
            rb.AddForce(move.normalized * 20 * airMultiplier, ForceMode.Force);

        walkingBack = moveDir.z < 0;
    }

    private void SpeedLimiting()
    {
        if (!IsGrounded())
            return;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void DragControl()
    {
        rb.drag = IsGrounded() ? groundDrag : 0;
    }

    public void OnMove(InputValue value)
    {
        moveDir = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);

    }


    private void SetSpeed()
    {
        if(state == MovementState.Sliding)
            speed = targetSpeed;

        else if (targetSpeed - lastTargetSpeed > 4 && speed > 0)
        {
            //StopAllCoroutines();
            StartCoroutine(SmoothSpeed(15));
        }
        else if (targetSpeed - lastTargetSpeed < -4 && speed > 0)
        {
            //StopAllCoroutines();
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

        while(time < difference)
        {
            speed = Mathf.Lerp(startSpeed, targetSpeed, time / difference);
            time += Time.deltaTime * lerpSpeed;
            scr_UIManager.Instance.UpdateSpeed(speed);
            yield return null;
        }
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
        if (state == MovementState.Sliding)
            return;
        if(state == MovementState.Crouching)
        {
            transform.localScale = originalScale;
            crouchHeld = false;
            return;
        }
        Jump();
        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);

    }
    private void Jump()
    {
        if (IsGrounded() && readyToJump)
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
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

    public bool IsGrounded()
    {
        return Physics.OverlapSphere(transform.position, radius, ground).Length > 0 ? true : false;
    }

    #endregion
}

[Serializable]
public enum MovementState
{
    Idle,
    Walking,
    Sprinting,
    Crouching,
    Sliding,
    Air
}
