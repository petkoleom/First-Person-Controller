using UnityEngine;

public class scr_ExplosivePlace : scr_ExplosiveBehaviour
{
    private Vector3 placePos;

    public void Place()
    {
        RaycastHit hit;
        var rayDown = Physics.Raycast(explosive.transform.position, Vector3.down, out hit, 3, explosive.data.ground);
        if (rayDown)
        {
            placePos = hit.point;
            transform.position = hit.point;
            Vector3 rotation = explosive.transform.eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotation.y - 90, 0);


        }

    }
}
