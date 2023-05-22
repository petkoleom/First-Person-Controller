using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class scr_WeaponBob : scr_WeaponBehaviour
{

    private scr_Player player;

    private float xIntensity, yIntensity, movementCounter, idleCounter;
    private Vector3 origin, targetPosition;

    private void Start()
    {
        player = transform.root.GetChild(0).GetComponent<scr_Player>();
        origin = transform.parent.localPosition;

    }

    private void Update()
    {
        if (!player.Ground.IsGrounded) return;
        transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, CalculateTargetPos(player.Move.GetSpeed()), Time.deltaTime * 6);
    }


    private Vector3 CalculateTargetPos(float _velocity)
    {
        if (_velocity < 1)
            movementCounter = .1f;
        else
            movementCounter += Time.deltaTime * (_velocity + .5f);

        targetPosition = origin + new Vector3(Mathf.Cos(movementCounter) * (Weapon.state == WeaponState.ADS ? xIntensity * .1f : xIntensity) * (_velocity * .1f), Mathf.Sin(movementCounter * 2) * (Weapon.state == WeaponState.ADS ? yIntensity * .1f : yIntensity) * (_velocity * .1f), 0);
        return targetPosition;

    }
}

