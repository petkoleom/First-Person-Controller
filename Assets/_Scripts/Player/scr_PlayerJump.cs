using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerGround))]
public class scr_PlayerJump : scr_PlayerBehaviour
{
    [SerializeField]
    private float force = 3;
    [SerializeField]
    private float jumpCooldown = .3f;

    private Vector3 normalVector = Vector3.up;
    public bool jumpingHeld;
    private bool readyToJump = true;

    private void Start()
    {
        readyToJump = true;
    }

    private void Update()
    {
        Jump();
    }

    public void OnJump(InputValue value)
    {
        jumpingHeld = !jumpingHeld;
    }
    private void Jump()
    {
        if (jumpingHeld && player.playerGround.isGrounded && readyToJump)
        {
            readyToJump = false;

            player.rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);
            player.rb.AddForce(Vector3.up * force, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
