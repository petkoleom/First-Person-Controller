using UnityEngine;

public class scr_ExplosivePlace : scr_ExplosiveBehaviour
{
    private Vector3 placePos;

    public void Place()
    {
        RaycastHit _hit;
        var _rayDown = Physics.Raycast(Explosive.transform.position, Vector3.down, out _hit, 3, Explosive.Data.Ground);
        if (_rayDown)
        {
            placePos = _hit.point;
            transform.position = _hit.point;
            Vector3 _rotation = Explosive.transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0, _rotation.y - 90, 0);


        }

    }
}
