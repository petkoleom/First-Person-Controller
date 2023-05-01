using System;
using UnityEngine;

public class scr_Player : MonoBehaviour
{
    [HideInInspector]
    public scr_PlayerLook playerLook { get; set; }
    public scr_PlayerGround playerGround { get; set; }
    public scr_PlayerMove playerMove { get; set; }
    public scr_PlayerJump playerJump { get; set; }
    public scr_PlayerSprint playerSprint { get; set; }
    public scr_PlayerCrouch playerCrouch { get; set; }


    public Rigidbody rb { get; set; }
    public Transform orientation { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerLook = GetComponent<scr_PlayerLook>();
        playerGround = GetComponent<scr_PlayerGround>();
        playerMove = GetComponent<scr_PlayerMove>();
        playerJump = GetComponent<scr_PlayerJump>();
        playerSprint = GetComponent<scr_PlayerSprint>();
        playerCrouch = GetComponent<scr_PlayerCrouch>();

        if (playerLook != null)
            playerLook.Initialize(this);
        if (playerGround != null)
            playerGround.Initialize(this);
        if (playerMove != null)
            playerMove.Initialize(this);
        if (playerJump != null)
            playerJump.Initialize(this);
        if (playerSprint != null)
            playerSprint.Initialize(this);
        if (playerCrouch != null)
            playerCrouch.Initialize(this);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.GetChild(0);
    }

    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public MovementState state { get; set; }

    private void StateHandler()
    {
        scr_UIManager.Instance.UpdateState(state);
        /*
        if (playerGrounded.isGrounded)
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
        }*/

        if (playerGround.isGrounded)
        {
            if (playerSprint.sprintingHeld && !playerMove.walkingBack && playerMove.moveDir != Vector3.zero)
            {
                state = MovementState.Sprinting;
                playerMove.SetTargetSpeed(playerSprint.GetStateSpeed());
            }
            else if (rb.velocity.magnitude > .01f)
            {
                state = MovementState.Walking;
                playerMove.SetTargetSpeed(playerMove.GetStateSpeed());
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
