using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerGrounded))]
public class scr_PlayerJump : MonoBehaviour
{

    [SerializeField]
    private float force = 3;

    [SerializeField]
    private float jumpCooldown = .3f;

    private bool readyToJump = true;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        readyToJump = true;
    }

    public void OnJump(InputValue value)
    {
        Jump();
        readyToJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);

    }

    private void Jump()
    {
        if (GetComponent<scr_PlayerGrounded>().IsGrounded() && readyToJump)
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
