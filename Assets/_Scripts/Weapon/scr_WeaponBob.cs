using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class scr_WeaponBob : scr_WeaponBehaviour
{

    public float xIntensity;
    public float yIntensity;


    private float movementCounter;
    private float idleCounter;
    private Vector3 origin;
    private Vector3 targetPosition;
    private float velocity;


    private scr_Player player;
    private void Start()
    {
        player = transform.root.GetChild(0).GetComponent<scr_Player>();
        origin = transform.parent.localPosition;

    }

    private void Update()
    {
        if (!player.playerGround.isGrounded) return;
        transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, CalculateTargetPos(player.playerMove.GetSpeed()), Time.deltaTime * 6);
    }


    private Vector3 CalculateTargetPos(float velocity)
    {
        if (velocity < 1)
            movementCounter = .1f;
        else
            movementCounter += Time.deltaTime * (velocity + .5f);

        targetPosition = origin + new Vector3(Mathf.Cos(movementCounter) * (weapon.state == WeaponState.ADS ? xIntensity * .1f : xIntensity) * (velocity * .1f), Mathf.Sin(movementCounter * 2) * (weapon.state == WeaponState.ADS ? yIntensity * .1f : yIntensity) * (velocity * .1f), 0);
        return targetPosition;

    }
}

