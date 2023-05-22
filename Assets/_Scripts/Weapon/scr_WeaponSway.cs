using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_WeaponSway : scr_WeaponBehaviour
{
    [SerializeField]
    private float intensity, smooth;

    private float x, y;

    private Quaternion originRot;

    private void Start()
    {
        originRot = transform.localRotation;
    }

    public void OnLook(InputValue _value)
    {
        x = _value.Get<Vector2>().x;
        y = _value.Get<Vector2>().y;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        Quaternion _xAdj = Quaternion.AngleAxis(-(Weapon.state == WeaponState.ADS ? intensity * .1f : intensity) * x, Vector3.up);
        Quaternion _yAdj = Quaternion.AngleAxis((Weapon.state == WeaponState.ADS ? intensity * .1f : intensity) * y, Vector3.right);
        Quaternion _targetRot = originRot * _xAdj * _yAdj;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRot, Time.deltaTime * smooth);
    }
}
