using System;
using UnityEngine;

public class scr_Player : MonoBehaviour
{

    public scr_PlayerManager PlayerManager { get; set; }

    public int PlayerID;

    public scr_PlayerLook Look { get; set; }
    public scr_PlayerGround Ground { get; set; }
    public scr_PlayerMove Move { get; set; }
    public scr_PlayerJump Jump { get; set; }
    public scr_PlayerMantle Mantle { get; set; }
    public scr_PlayerSprint Sprint { get; set; }
    public scr_PlayerCrouch Crouch { get; set; }
    public scr_PlayerProne Prone { get; set; }
    public scr_PlayerCamBob CamBob { get; set; }
    public scr_PlayerHealth Health { get; set; }
    public scr_PlayerClimb Climb { get; set; }


    public Rigidbody Rb { get; set; }
    public Transform Orientation { get; set; }
    public Transform CamHolder { get; set; }

    public void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Orientation = transform.GetChild(0);
        CamHolder = transform.root.GetChild(1);
        Rb.freezeRotation = true;

        Look = GetComponent<scr_PlayerLook>();
        Ground = GetComponent<scr_PlayerGround>();
        Move = GetComponent<scr_PlayerMove>();
        Jump = GetComponent<scr_PlayerJump>();
        Mantle = GetComponent<scr_PlayerMantle>();
        Sprint = GetComponent<scr_PlayerSprint>();
        Crouch = GetComponent<scr_PlayerCrouch>();
        Prone = GetComponent<scr_PlayerProne>();
        CamBob = CamHolder.GetComponent<scr_PlayerCamBob>();
        Health = GetComponent<scr_PlayerHealth>();
        Climb = GetComponent<scr_PlayerClimb>();

        if (Look != null) Look.Initialize(this);
        if (Ground != null) Ground.Initialize(this);
        if (Move != null)
        {
            Move.Initialize(this);
            Move.SetTargetSpeed(1);
        }
        if (Jump != null) Jump.Initialize(this);
        if (Mantle != null) Mantle.Initialize(this);
        if (Sprint != null) Sprint.Initialize(this);
        if (Crouch != null) Crouch.Initialize(this);
        if (Prone != null) Prone.Initialize(this);
        if (CamBob != null) CamBob.Initialize(this);
        if (Health != null) Health.Initialize(this);
        if (Climb != null) Climb.Initialize(this);
    }

    public void InitializePlayer(scr_PlayerManager _playerManager, int _id)
    {
        PlayerManager = _playerManager;
        PlayerID = _id;
    }

    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public PlayerState State { get; set; }

    private void StateHandler()
    {
        scr_UIManager.Instance.UpdateState(State);

        if (Climb.IsClimbing)
        {
            State = PlayerState.Climbing;
            Move.SetMovement(false);
        }
        else if (Ground.IsGrounded)
        {
            Move.SetMovement(true);
            if (Prone.Prone)
            {
                State = PlayerState.Prone;
                Move.SetTargetSpeed(Prone.GetStateSpeed());
            }
            else if (Crouch.IsSliding)
            {
                State = PlayerState.Sliding;
                Move.SetTargetSpeed(Crouch.GetStateSpeed2());
            }
            else if (Crouch.IsCrouching)
            {
                State = PlayerState.Crouching;
                Move.SetTargetSpeed(Crouch.GetStateSpeed());
            }
            else if (Sprint.SprintingHeld && Move.walkingForward && Move.moveDir != Vector3.zero)
            {
                State = PlayerState.Sprinting;
                Move.SetTargetSpeed(Sprint.GetStateSpeed());
            }
            else if (Rb.velocity.magnitude > .02f)
            {
                State = PlayerState.Walking;
                Move.SetTargetSpeed(Move.GetStateSpeed());
            }
            else
            {
                State = PlayerState.Idle;

            }


        }
        else
        {
            Move.SetMovement(true);
            State = PlayerState.Air;
        }

        VelocityChange();
    }

    private void VelocityChange()
    {
        if (State == PlayerState.Air)
            scr_UIManager.Instance.SetVelocity(15);
        else if (State == PlayerState.Idle)
            scr_UIManager.Instance.SetVelocity(0);
        else if (State == PlayerState.Crouching && Move.GetSpeed() < .1f)
            scr_UIManager.Instance.SetVelocity(-5);
        else if (State == PlayerState.Prone && Move.GetSpeed() < .1f)
            scr_UIManager.Instance.SetVelocity(-10);
        else
            scr_UIManager.Instance.SetVelocity(Move.GetTargetSpeed());

    }

    public void ResetStance()
    {
        Crouch.StandUp();
        Prone.StandUp();
    }

    public void Respawn(Transform _spawnpoint)
    {
        transform.SetPositionAndRotation(_spawnpoint.position, Quaternion.identity);
        Look.ResetLook(_spawnpoint.localEulerAngles.y);
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
