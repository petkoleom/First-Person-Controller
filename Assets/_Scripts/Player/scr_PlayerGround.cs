using UnityEngine;

public class scr_PlayerGround : scr_PlayerBehaviour
{
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float maxSlopeAngle = 40;

    public bool isGrounded;
    private GameObject currentGroundObject;
    [HideInInspector]
    public RaycastHit slopeHit;

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Vector3 normal = contact.normal;
            if (IsFloor(normal))
            {
                isGrounded = true;
                //normalVector = normal;
                currentGroundObject = contact.otherCollider.gameObject;
                return;
            }

            else if (currentGroundObject == contact.otherCollider.gameObject)
            {
                isGrounded = false;
                currentGroundObject = null;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == currentGroundObject)
        {
            isGrounded = false;
            currentGroundObject = null;
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, .5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }


}