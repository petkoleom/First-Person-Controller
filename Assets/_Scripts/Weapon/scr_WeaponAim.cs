using UnityEngine;
using UnityEngine.InputSystem;

public class scr_WeaponAim : scr_WeaponBehaviour
{
    [SerializeField]
    private float adsSpeed;
    [SerializeField]
    private Vector3 hipPos;
    [SerializeField]
    private Vector3 adsPos;

    public bool IsADS;

    private bool adsHeld;
    private bool canADS { get { return adsHeld; } }

    private Vector3 curPos;
    private Vector3 targetPos;

    private void Update()
    {
        SetADS();
        SmoothTransition();
    }

    private void Start()
    {
        adsSpeed = Weapon.Data.ADSSpeed;
    }


    public void OnADS(InputValue _value)
    {
        adsHeld = _value.isPressed;
    }

    private void SetADS()
    {
        if (canADS)
        {
            IsADS = true;
            targetPos = adsPos;
            scr_UIManager.Instance.ADS();
        }
        else
        {
            IsADS = false;
            targetPos = hipPos;
            scr_UIManager.Instance.NotADS();
        }
    }

    private void SmoothTransition()
    {
        var _vel = Vector3.zero;
        curPos = Vector3.SmoothDamp(curPos, targetPos, ref _vel, adsSpeed);
        transform.parent.localPosition = curPos;
    }

    public void ResetADS()
    {

        adsHeld = false;
    }

}
