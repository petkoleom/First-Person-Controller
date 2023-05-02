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
    public scr_PlayerCamBob playerCamBob { get; set; }


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
        playerSprint = GetComponent<scr_PlayerSprint>();
        playerCrouch = GetComponent<scr_PlayerCrouch>();
        playerCamBob = camHolder.GetComponent<scr_PlayerCamBob>();

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
        if(playerCamBob != null)
            playerCamBob.Initialize(this);

        
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


        if (playerGround.isGrounded)
        {
            if (playerCrouch.sliding)
            {
                state = MovementState.Sliding;
                playerMove.SetTargetSpeed(playerCrouch.GetStateSpeed2());
            }
            else if (playerCrouch.crouching)
            {
                state = MovementState.Crouching;
                playerMove.SetTargetSpeed(playerCrouch.GetStateSpeed());
            }
            else if (playerSprint.sprintingHeld && !playerMove.walkingBack && playerMove.moveDir != Vector3.zero)
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
