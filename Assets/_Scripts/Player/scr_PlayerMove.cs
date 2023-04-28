using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerGrounded))]
public class scr_PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 9;
    [SerializeField]
    private float groundDrag = 5;
    [SerializeField]
    private float airMultiplier = .3f;

    private float speed;
    Vector3 moveDir;

    private Rigidbody rb;
    private scr_PlayerGrounded grounded;
    private Transform orientation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        grounded = GetComponent<scr_PlayerGrounded>();
        orientation = transform.GetChild(0);

        speed = walkSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        DragControl();
        SpeedControl();
    }

    private void Move()
    {
        Vector3 move = orientation.forward * moveDir.z + orientation.right * moveDir.x;

        if(grounded.IsGrounded())
            rb.AddForce(move.normalized * speed * 10, ForceMode.Force);
        else
            rb.AddForce(move.normalized * 10 * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (!grounded.IsGrounded())
            return;
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void DragControl()
    {
        rb.drag = GetComponent<scr_PlayerGrounded>().IsGrounded() ? groundDrag : 0;
    }

    public void OnMove(InputValue value)
    {
        moveDir = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    public void SetMoveSpeed(float speed)
    {
        
        if (speed == -1)
            this.speed = walkSpeed;
        else
            this.speed = speed;
    }

}
