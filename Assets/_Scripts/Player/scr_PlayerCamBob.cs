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
        if (!Player.Ground.IsGrounded) return;
        if(Player.Move.GetSpeed() > .1f)
        {

            timer += Time.deltaTime * Player.Move.GetSpeed() * 1.25f;
            headBob.transform.localPosition = new Vector3(headBob.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (Player.State == PlayerState.Crouching ? crouchBobAmount : Player.State == PlayerState.Sprinting ? sprintBobAmount : walkBobAmount), headBob.transform.localPosition.z);
        }
    }

}
