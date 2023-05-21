using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerLook : scr_PlayerBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private float sensitivity = 5;

    public Transform camHolder;

    private float mouseX, mouseY;
    public float xRot, yRot;

    private Vector3 rot;

    private void Awake()
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
        rot = camHolder.transform.localRotation.eulerAngles;
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

    public void ResetLook(float _yRot)
    {
        yRot = _yRot;
        xRot = 0;

        camHolder.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        player.orientation.transform.localRotation = Quaternion.Euler(0, yRot, 0);
    }

}
