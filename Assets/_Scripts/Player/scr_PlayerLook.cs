using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerLook : scr_PlayerBehaviour
{
    [SerializeField]
    private float sensitivity = 5;

    private Transform camHolder;

    private float mouseX, mouseY, xRot, yRot;

    private void Start()
    {
        camHolder = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
    }

    void Look()
    {
        Vector3 rot = camHolder.transform.localRotation.eulerAngles;
        yRot = rot.y + mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        camHolder.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        player.orientation.transform.localRotation = Quaternion.Euler(0, yRot, 0);
    }

    public void OnLook(InputValue value)
    {
        mouseX = value.Get<Vector2>().x * sensitivity * Time.fixedDeltaTime;
        mouseY = value.Get<Vector2>().y * sensitivity * Time.fixedDeltaTime;
    }
}
