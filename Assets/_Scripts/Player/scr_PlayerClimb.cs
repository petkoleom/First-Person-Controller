using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerClimb : scr_PlayerBehaviour
{
    RaycastHit hit;

    [SerializeField]
    private LayerMask climbable;
    [SerializeField]
    private float climbSpeed;

    public bool IsClimbing;
    private bool wasClimbing;

    private float moveDir;

    private void Update()
    {
        Player.Rb.useGravity = !IsClimbing;

        Climb();

        DismountAtTop();
    }

    private bool HitClimbable()
    {
        bool _check1 = Physics.Raycast(transform.position + Vector3.up * .2f, Player.Orientation.forward, out hit, .7f, climbable);
        bool _check2 = Physics.Raycast(transform.position + Vector3.up * 1.8f, Player.Orientation.forward, out hit, .7f, climbable);

        return _check1 || _check2;
    }

    public void OnMove(InputValue _value)
    {

        moveDir = _value.Get<Vector2>().y;

        

    }

    public void OnJump(InputValue _value)
    {
        if (IsClimbing && !Player.Ground.IsGrounded)
        {
            IsClimbing = false;
            Player.Rb.AddForce(-Player.Orientation.forward * 5, ForceMode.Impulse);
        }
    }

    private void Climb()
    {

        if (!IsClimbing && moveDir > 0 && HitClimbable())
        {
            IsClimbing = true;
        }
        else if (IsClimbing && moveDir < 0 && HitClimbable() && Player.Ground.IsGrounded)
        {
            IsClimbing = false;
        }
        else if (IsClimbing && HitClimbable())
        {
            Player.Rb.velocity = new Vector3(0, moveDir * climbSpeed, 0);
        }
        else
        {
            IsClimbing = false;
        }

        
    }

    private void DismountAtTop()
    {
        if (wasClimbing && !IsClimbing && !Player.Ground.IsGrounded)
        {
            Player.Rb.AddForce(Player.Orientation.forward * 2, ForceMode.Impulse);
        }
        wasClimbing = IsClimbing;
    }
}
