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
    public bool IsCrouching;
    public bool IsSliding;


    private void Start()
    {
        originalScale = transform.localScale;
        IsCrouching = false;
    }

    public void OnCrouch(InputValue _value)
    {
        if (Player.State == PlayerState.Sprinting)
            StartCoroutine(Slide());
        else if (!IsCrouching && Player.Ground.IsGrounded)
            Crouch();
        else
            StandUp();
    }

    public void Crouch()
    {
        Player.ResetStance();
        IsCrouching = true;
        transform.localScale = new Vector3(1, crouchScale, 1);

    }

    public void StandUp()
    {
        IsCrouching = false;
        transform.localScale = originalScale;
    }

    private IEnumerator Slide()
    {
        Vector3 _currentDir = Player.Orientation.forward;
        IsSliding = true;
        transform.localScale = new Vector3(1, crouchScale, 1);
        yield return new WaitForSeconds(slideDuration);
        IsSliding = false;
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
