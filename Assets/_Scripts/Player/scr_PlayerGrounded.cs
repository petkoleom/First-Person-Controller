using UnityEngine;

public class scr_PlayerGrounded : MonoBehaviour
{
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float radius = .5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 900);
    }

    public bool IsGrounded()
    {
        return Physics.OverlapSphere(transform.position, radius, ground).Length > 0 ? true : false;
    }

}
