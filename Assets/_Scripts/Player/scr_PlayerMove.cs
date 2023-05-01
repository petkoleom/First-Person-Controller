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

    public bool walkingBack;
    public bool sprintHeld;

    private void FixedUpdate()
    {
        Move();
        DragControl();
        SpeedLimiting();
        SetSpeed();
    }

    public void OnMove(InputValue value)
    {
        moveInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void Move()
    {
        moveDir = player.orientation.forward * moveInput.z + player.orientation.right * moveInput.x;
        walkingBack = moveInput.z < 0;

        if (player.playerGround.isGrounded)
            player.rb.AddForce(moveDir.normalized * speed * 10, ForceMode.Acceleration);
        else if (player.playerGround.OnSlope() && player.playerGround.isGrounded)
            player.rb.AddForce(GetSlopeMoveDir(moveDir) * speed * 10, ForceMode.Acceleration);
        else
            player.rb.AddForce(moveDir.normalized * speed * 10 * airMultiplier, ForceMode.Acceleration);

        scr_UIManager.Instance.UpdateSpeed(player.rb.velocity.magnitude);
    }

    private Vector3 GetSlopeMoveDir(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, player.playerGround.slopeHit.normal).normalized;
    }

    private void SpeedLimiting()
    {
        Vector3 flatVel = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
        if (player.playerGround.isGrounded)
        {
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                player.rb.velocity = new Vector3(limitedVel.x, player.rb.velocity.y, limitedVel.z);
            }
        } 
        else if(speed > walkSpeed)
        {
            if (flatVel.magnitude > speed * .8f)
            {
                Vector3 limitedVel = flatVel.normalized * speed * .8f;
                player.rb.velocity = new Vector3(limitedVel.x, player.rb.velocity.y, limitedVel.z);
            }
        }
        else
        {
            if (flatVel.magnitude > speed * airMultiplier * 2)
            {
                Vector3 limitedVel = flatVel.normalized * speed * airMultiplier * 2.5f;
                player.rb.velocity = new Vector3(limitedVel.x, player.rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void DragControl()
    {
        player.rb.useGravity = !player.playerGround.OnSlope();
        player.rb.drag = player.playerGround.isGrounded ? groundDrag : 0;
    }

    private void SetSpeed()
    {

        if (targetSpeed - lastTargetSpeed > 4 && speed > 0)
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

    public void SetTargetSpeed(float speed)
    {
        targetSpeed = speed;
    }

    public float GetStateSpeed()
    {
        return walkSpeed;
    }


}

