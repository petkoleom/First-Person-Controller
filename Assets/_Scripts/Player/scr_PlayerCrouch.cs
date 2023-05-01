using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerCrouch : scr_PlayerBehaviour
{
    [SerializeField]
    private float crouchScale;
    [SerializeField]
    private float crouchSpeed;

    private Vector3 originalScale;
    private Transform camPos;

    private bool crouching;

    private scr_PlayerMove move;

    private void Start()
    {
        move = GetComponent<scr_PlayerMove>();
        originalScale = transform.localScale;
        camPos = transform.GetChild(2);
        crouching = false;
    }

    public void OnCrouch(InputValue value)
    {
        if(!crouching)
        {
            crouching = true;
            transform.localScale = new Vector3(1, crouchScale, 1);
        }
        else
        {
            crouching = false;
            transform.localScale = originalScale;

        }
    }

    public void Reset()
    {
        crouching = false;
        transform.localScale = originalScale;
    }

}
