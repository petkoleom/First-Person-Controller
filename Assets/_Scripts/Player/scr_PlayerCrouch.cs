using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerCrouch : scr_PlayerBehaviour
{
    [Header("Crouching")]
    [SerializeField]
    private float crouchScale;
    [SerializeField]
    private float crouchSpeed;
    [Header("Sliding")]
    [SerializeField]
    private float slideDuration;
    [SerializeField]
    private float slideSpeed;

    private Vector3 originalScale;
    public bool crouching;
    public bool sliding;


    private void Start()
    {
        originalScale = transform.localScale;
        crouching = false;
    }

    public void OnCrouch(InputValue value)
    {
        if (player.state == MovementState.Sprinting)
            StartCoroutine(Slide());
        else if (!crouching && player.playerGround.isGrounded)
            Crouch();
        else
            StandUp();
    }

    public void Crouch()
    {
        player.ResetStance();
        crouching = true;
        transform.localScale = new Vector3(1, crouchScale, 1);

    }

    public void StandUp()
    {
        crouching = false;
        transform.localScale = originalScale;
    }

    private IEnumerator Slide()
    {
        Vector3 currentDir = player.orientation.forward;
        sliding = true;
        transform.localScale = new Vector3(1, crouchScale, 1);
        yield return new WaitForSeconds(slideDuration);
        sliding = false;
        transform.localScale = originalScale;
    }

    public float GetStateSpeed()
    {
        return crouchSpeed;
    }    
    public float GetStateSpeed2()
    {
        return slideSpeed;
    }

}
