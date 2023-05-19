using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerClimb : scr_PlayerBehaviour
{
    RaycastHit hit;

    [SerializeField]
    private LayerMask climbable;
    [SerializeField]
    private float climbSpeed;

    public bool isClimbing;
    private bool wasClimbing;

    private float moveDir;

    private void Update()
    {
        player.rb.useGravity = !isClimbing;

        Climb();

        DismountAtTop();
    }

    private bool HitClimbable()
    {
        bool check1 = Physics.Raycast(transform.position + Vector3.up * .2f, player.orientation.forward, out hit, .7f, climbable);
        bool check2 = Physics.Raycast(transform.position + Vector3.up * 1.8f, player.orientation.forward, out hit, .7f, climbable);

        return check1 || check2;
    }

    public void OnMove(InputValue value)
    {

        moveDir = value.Get<Vector2>().y;

        

    }

    public void OnJump(InputValue value)
    {
        if (isClimbing && !player.playerGround.isGrounded)
        {
            isClimbing = false;
            player.rb.AddForce(-player.orientation.forward * 5, ForceMode.Impulse);
        }
    }

    private void Climb()
    {

        if (!isClimbing && moveDir > 0 && HitClimbable())
        {
            isClimbing = true;
        }
        else if (isClimbing && moveDir < 0 && HitClimbable() && player.playerGround.isGrounded)
        {
            isClimbing = false;
        }
        else if (isClimbing && HitClimbable())
        {
            player.rb.velocity = new Vector3(0, moveDir * climbSpeed, 0);
        }
        else
        {
            isClimbing = false;
        }

        
    }

    private void DismountAtTop()
    {
        if (wasClimbing && !isClimbing && !player.playerGround.isGrounded)
        {
            player.rb.AddForce(player.orientation.forward * 2, ForceMode.Impulse);
        }
        wasClimbing = isClimbing;
    }
}
