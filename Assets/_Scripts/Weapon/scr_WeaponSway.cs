using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_WeaponSway : scr_WeaponBehaviour
{
    [SerializeField]
    private float intensity;
    [SerializeField]
    private float smooth;

    private float x, y;

    private Quaternion originRot;

    private void Start()
    {
        originRot = transform.localRotation;
    }

    public void OnLook(InputValue value)
    {
        x = value.Get<Vector2>().x;
        y = value.Get<Vector2>().y;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        Quaternion xAdj = Quaternion.AngleAxis(intensity * x, Vector3.up);
        Quaternion yAdj = Quaternion.AngleAxis(-intensity * y, Vector3.right);
        Quaternion targetRot = originRot * xAdj * yAdj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smooth);
    }
}
