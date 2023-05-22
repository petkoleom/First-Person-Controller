using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerGround))]
public class scr_PlayerMove : scr_PlayerBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed = 9;
    [SerializeField]
    private float groundDrag = 5;
    [SerializeField]
    private float airMultiplier = .3f;

    private float speed;
    private float targetSpeed;
    private float lastTargetSpeed;

    private Vector3 moveInput;
    public Vector3 moveDir { get; set; }

    public bool walkingForward;

    private bool movementEnabled = true;

    private void FixedUpdate()
    {


        if (movementEnabled)
            Move();

        CounterMovement();
        SpeedLimiting();
        SetSpeed();
    }

    public void OnMove(InputValue value)
    {
        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void Move()
    {
        var dragMod = groundDrag / 10;
        moveDir = Player.Orientation.forward * moveInput.z + Player.Orientation.right * moveInput.x;
        walkingForward = moveInput.z > 0;
        if (Player.Ground.OnSlope())
            Player.Rb.AddForce(GetSlopeMoveDir(moveDir) * speed * 10 * dragMod, ForceMode.Acceleration);
        else if (Player.Ground.IsGrounded)
            Player.Rb.AddForce(moveDir.normalized * speed * 10 * dragMod, ForceMode.Acceleration);
        else
            Player.Rb.AddForce(moveDir.normalized * speed * 10 * dragMod * airMultiplier, ForceMode.Acceleration);

        scr_UIManager.Instance.UpdateSpeed(Player.Rb.velocity.magnitude);
    }

    private Vector3 GetSlopeMoveDir(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, Player.Ground.SlopeHit.normal).normalized;
    }

    void CounterMovement()
    {
        if (Player.State != PlayerState.Climbing)
            Player.Rb.useGravity = !Player.Ground.OnSlope();

        Vector3 vel = Player.Rb.velocity;

        float coefficientOfFriction = (speed * groundDrag) / targetSpeed;

        if (Player.Ground.IsGrounded)
            Player.Rb.AddForce(-vel * coefficientOfFriction, ForceMode.Acceleration);
    }

    private void SpeedLimiting()
    {

        if (Player.Ground.OnSlope())
        {
            if (Player.Rb.velocity.magnitude > speed)
                Player.Rb.velocity = Player.Rb.velocity.normalized * speed;
        }

        Vector3 flatVel = new Vector3(Player.Rb.velocity.x, 0, Player.Rb.velocity.z);
        if (Player.Ground.IsGrounded)
        {
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                Player.Rb.velocity = new Vector3(limitedVel.x, Player.Rb.velocity.y, limitedVel.z);
            }
        }
        else if (speed > walkSpeed)
        {
            if (flatVel.magnitude > speed * .8f)
            {
                Vector3 limitedVel = flatVel.normalized * speed * .8f;
                Player.Rb.velocity = new Vector3(limitedVel.x, Player.Rb.velocity.y, limitedVel.z);
            }
        }
        else
        {
            if (flatVel.magnitude > speed * airMultiplier * 2)
            {
                Vector3 limitedVel = flatVel.normalized * speed * airMultiplier * 2.5f;
                Player.Rb.velocity = new Vector3(limitedVel.x, Player.Rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void SetSpeed()
    {
        if (Player.State == PlayerState.Sliding)
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

    public float GetSpeed()
    {
        return Player.Rb.velocity.magnitude;
    }

    public float GetTargetSpeed()
    {
        return targetSpeed;
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

    public void SetTargetSpeed(float speed)
    {
        targetSpeed = speed;
    }

    public float GetStateSpeed()
    {
        return walkSpeed;
    }

    public void SetMovement(bool value)
    {
        movementEnabled = value;
    }


}

