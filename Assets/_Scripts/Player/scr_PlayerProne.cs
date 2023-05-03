using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerProne : scr_PlayerBehaviour
{
    [SerializeField]
    private float proneScale;
    [SerializeField]
    private float proneSpeed;

    private Vector3 originalScale;
    public bool prone;

    private void Start()
    {
        originalScale = transform.localScale;
        prone = false;
    }

    public void OnProne(InputValue value)
    {
        if (!prone && player.playerGround.isGrounded)
            Prone();
        else
            StandUp();
    }

    public void Prone()
    {
        player.ResetStance();
        prone = true;
        transform.localScale = new Vector3(1, proneScale, 1);

    }

    public void StandUp()
    {
        prone = false;
        transform.localScale = originalScale;
    }

    public float GetStateSpeed()
    {
        return proneSpeed;
    }
}
