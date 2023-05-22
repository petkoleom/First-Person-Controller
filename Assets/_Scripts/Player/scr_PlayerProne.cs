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
    public bool Prone { get; private set; }

    private void Start()
    {
        originalScale = transform.localScale;
        Prone = false;
    }

    public void OnProne(InputValue _value)
    {
        if (!Prone && Player.Ground.IsGrounded)
            LieDown();
        else
            StandUp();
    }

    public void LieDown()
    {
        Player.ResetStance();
        Prone = true;
        transform.localScale = new Vector3(1, proneScale, 1);

    }

    public void StandUp()
    {
        Prone = false;
        transform.localScale = originalScale;
    }

    public float GetStateSpeed()
    {
        return proneSpeed;
    }
}
