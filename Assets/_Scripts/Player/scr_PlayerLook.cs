using UnityEngine;
using UnityEngine.InputSystem;

public class scr_PlayerLook : scr_PlayerBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private float sensitivity = 5;

    public Transform CamHolder;

    private float mouseX, mouseY;
    private float xRot, yRot;

    private Vector3 rot;

    [SerializeField]
    private Transform camPrefab;
    [SerializeField]
    private Transform camParent;

    private GameObject mainCam;

    private void Awake()
    {
        CamHolder = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<Transform>();
        mainCam = Instantiate(camPrefab, camParent.position, camParent.rotation).gameObject;
        mainCam.transform.parent = camParent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        Look();
    }

    void Look()
    {
        rot = CamHolder.transform.localRotation.eulerAngles;
        yRot = rot.y + mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        CamHolder.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        Player.Orientation.transform.localRotation = Quaternion.Euler(0, yRot, 0);
    }

    public void OnLook(InputValue _value)
    {

        mouseX = _value.Get<Vector2>().x * sensitivity * Time.fixedDeltaTime;
        mouseY = _value.Get<Vector2>().y * sensitivity * Time.fixedDeltaTime;
    }

    public void ResetLook(float _yRot)
    {
        yRot = _yRot;
        xRot = 0;

        CamHolder.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        Player.Orientation.transform.localRotation = Quaternion.Euler(0, yRot, 0);
    }

}
