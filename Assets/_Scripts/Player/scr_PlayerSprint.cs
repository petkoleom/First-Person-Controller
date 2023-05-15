using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerMove))]
public class scr_PlayerSprint : scr_PlayerBehaviour
{
    [Header("Sprinting")]
    [SerializeField]
    private float sprintSpeed = 13;

    public bool sprintingHeld;

    public void OnSprint(InputValue value)
    {
        sprintingHeld = value.isPressed;
    }

    public float GetStateSpeed()
    {
        return sprintSpeed;
    }
}
