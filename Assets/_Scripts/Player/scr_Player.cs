using System;
using UnityEngine;

public class scr_Player : MonoBehaviour
{
    public scr_PlayerLook playerLook { get; set; }
    public scr_PlayerGround playerGround { get; set; }
    public scr_PlayerMove playerMove { get; set; }
    public scr_PlayerJump playerJump { get; set; }
    public scr_PlayerMantle playerMantle { get; set; }
    public scr_PlayerSprint playerSprint { get; set; }
    public scr_PlayerCrouch playerCrouch { get; set; }
    public scr_PlayerProne playerProne { get; set; }
    public scr_PlayerCamBob playerCamBob { get; set; }
    public scr_PlayerHealth playerHealth { get; set; }
    public scr_PlayerClimb playerClimb { get; set; }


    public Rigidbody rb { get; set; }
    public Transform orientation { get; set; }
    public Transform camHolder { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orientation = transform.GetChild(0);
        camHolder = transform.root.GetChild(1);
        rb.freezeRotation = true;

        playerLook = GetComponent<scr_PlayerLook>();
        playerGround = GetComponent<scr_PlayerGround>();
        playerMove = GetComponent<scr_PlayerMove>();
        playerJump = GetComponent<scr_PlayerJump>();
        playerMantle = GetComponent<scr_PlayerMantle>();
        playerSprint = GetComponent<scr_PlayerSprint>();
        playerCrouch = GetComponent<scr_PlayerCrouch>();
        playerProne = GetComponent<scr_PlayerProne>();
        playerCamBob = camHolder.GetComponent<scr_PlayerCamBob>();
        playerHealth = GetComponent<scr_PlayerHealth>();
        playerClimb = GetComponent<scr_PlayerClimb>();

        if (playerLook != null) playerLook.Initialize(this);
        if (playerGround != null) playerGround.Initialize(this);
        if (playerMove != null) playerMove.Initialize(this);
        if (playerJump != null) playerJump.Initialize(this);
        if (playerMantle != null) playerMantle.Initialize(this);
        if (playerSprint != null) playerSprint.Initialize(this);
        if (playerCrouch != null) playerCrouch.Initialize(this);
        if (playerProne != null) playerProne.Initialize(this);
        if (playerCamBob != null) playerCamBob.Initialize(this);
        if (playerHealth != null) playerHealth.Initialize(this);
        if(playerClimb != null) playerClimb.Initialize(this);


    }

    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public PlayerState state { get; set; }

    private void StateHandler()
    {
        scr_UIManager.Instance.UpdateState(state);

        if (playerClimb.isClimbing)
        {
            state = PlayerState.Climbing;
            playerMove.SetMovement(false);
        }
        else if (playerGround.isGrounded)
        {
            playerMove.SetMovement(true);
            if (playerProne.prone)
            {
                state = PlayerState.Prone;
                playerMove.SetTargetSpeed(playerProne.GetStateSpeed());
            }
            else if (playerCrouch.sliding)
            {
                state = PlayerState.Sliding;
                playerMove.SetTargetSpeed(playerCrouch.GetStateSpeed2());
            }
            else if (playerCrouch.crouching)
            {
                state = PlayerState.Crouching;
                playerMove.SetTargetSpeed(playerCrouch.GetStateSpeed());
            }
            else if (playerSprint.sprintingHeld && playerMove.walkingForward && playerMove.moveDir != Vector3.zero)
            {
                state = PlayerState.Sprinting;
                playerMove.SetTargetSpeed(playerSprint.GetStateSpeed());
            }
            else if (rb.velocity.magnitude > .01f)
            {
                state = PlayerState.Walking;
                playerMove.SetTargetSpeed(playerMove.GetStateSpeed());
            }
            else
            {
                state = PlayerState.Idle;
            }
        }
        else
        {
            playerMove.SetMovement(true);
            state = PlayerState.Air;
        }
    }

    public void ResetStance()
    {
        playerCrouch.StandUp();
        playerProne.StandUp();
    }


    #endregion
}

[Serializable]
public enum PlayerState
{
    Idle,
    Walking,
    Sprinting,
    Crouching,
    Prone,
    Sliding,
    Climbing,
    Air
}
