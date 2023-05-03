using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerCamBob : scr_PlayerBehaviour
{
    [SerializeField]
    private float walkBobAmount = .035f;
    [SerializeField]
    private float sprintBobAmount = .07f;
    [SerializeField]
    private float crouchBobAmount = .015f;

    private float defaultYPos = 0;
    private float timer;

    private Transform headBob;


    private void Start()
    {
        headBob = transform.GetChild(0);
        defaultYPos = headBob.transform.localPosition.y;
    }

    private void Update()
    {
        HeadBob();
    }

    private void HeadBob()
    {
        if (!player.playerGround.isGrounded) return;
        if(player.playerMove.GetSpeed() > .1f)
        {

            timer += Time.deltaTime * player.playerMove.GetSpeed() * 1.25f;
            headBob.transform.localPosition = new Vector3(headBob.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (player.state == PlayerState.Crouching ? crouchBobAmount : player.state == PlayerState.Sprinting ? sprintBobAmount : walkBobAmount), headBob.transform.localPosition.z);
        }
    }

}
