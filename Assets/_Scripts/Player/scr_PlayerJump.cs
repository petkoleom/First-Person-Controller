using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerGround))]
public class scr_PlayerJump : scr_PlayerBehaviour
{
    [Header("Jumping")]
    [SerializeField]
    private float force = 3;
    [SerializeField]
    private float jumpCooldown = .3f;
    
    private bool jumpingHeld;
    private bool readyToJump = true;

    private void Start()
    {
        readyToJump = true;
    }

    private void Update()
    {
        Jump();
    }

    public void OnJump(InputValue _value)
    {
        //jumpingHeld = !jumpingHeld;
        jumpingHeld = _value.isPressed;
    }
    private void Jump()
    {
        if (jumpingHeld && Player.Ground.IsGrounded && readyToJump && Player.State != PlayerState.Sliding && !Player.Mantle.CanMantle)
        {
            if (Player.State == PlayerState.Crouching || Player.State == PlayerState.Prone)
            {
                Player.ResetStance();
                readyToJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else
            {
                readyToJump = false;
                Player.Rb.velocity = new Vector3(Player.Rb.velocity.x, 0f, Player.Rb.velocity.z);
                Player.Rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

    }

    public void DisableJump(float _duration)
    {
        readyToJump = false;
        Invoke(nameof(ResetJump), _duration);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    

}
