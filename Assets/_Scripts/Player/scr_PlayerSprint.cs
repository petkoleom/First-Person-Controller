using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerMove))]
public class scr_PlayerSprint : scr_PlayerBehaviour
{
    [SerializeField]
    private float sprintSpeed = 13;

    public bool sprintingHeld;

    public void OnSprint(InputValue value)
    {
        if (!sprintingHeld)
        {
            sprintingHeld = true;
           
        }
        else
        {
            sprintingHeld = false;
        }
    }

    public float GetStateSpeed()
    {
        return sprintSpeed;
    }
}
