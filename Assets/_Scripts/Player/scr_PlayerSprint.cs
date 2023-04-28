using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_PlayerMove))]
public class scr_PlayerSprint : MonoBehaviour
{
    [SerializeField]
    private float sprintSpeed = 13;

    bool sprinting;

    public void OnSprint(InputValue value)
    {
        if (sprinting)
        {
            GetComponent<scr_PlayerMove>().SetMoveSpeed(-1);
            sprinting = false;
        }
        else
        {
            GetComponent<scr_PlayerMove>().SetMoveSpeed(sprintSpeed);
            sprinting = true;
        }
    }
}
