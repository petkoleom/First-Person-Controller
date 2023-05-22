using UnityEngine;

public class scr_PlayerGround : scr_PlayerBehaviour
{
    [Header("Grounding")]
    public LayerMask Ground;
    [SerializeField]
    private float maxSlopeAngle = 40;

    public bool IsGrounded;
    private GameObject currentGroundObject;
    [HideInInspector]
    public RaycastHit SlopeHit;
    private Vector3 currentGroundNormal = Vector3.up;

    private bool IsFloor(Vector3 _v)
    {
        float _angle = Vector3.Angle(Vector3.up, _v);
        return _angle < maxSlopeAngle;
    }

    private void OnCollisionStay(Collision _collisionInfo)
    {
        foreach (ContactPoint _contact in _collisionInfo.contacts)
        {
            Vector3 _normal = _contact.normal;
            if (IsFloor(_normal))
            {
                IsGrounded = true;
                currentGroundNormal = _normal;
                currentGroundObject = _contact.otherCollider.gameObject;
                return;
            }

            else if (currentGroundObject == _contact.otherCollider.gameObject)
            {
                IsGrounded = false;
                currentGroundObject = null;
            }
        }
    }

    private void OnCollisionExit(Collision _other)
    {
        if (_other.gameObject == currentGroundObject)
        {
            IsGrounded = false;
            currentGroundObject = null;
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, .2f))
        {
            float _angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return _angle < maxSlopeAngle && _angle != 0;
        }
        return false;
    }

    public Vector3 GetCurrentGroundNormal()
    {
        return currentGroundNormal;
    }


}
