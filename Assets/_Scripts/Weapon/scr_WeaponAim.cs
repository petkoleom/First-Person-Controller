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


    public bool isADS;


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
        adsSpeed = weapon.data.adsSpeed;
    }


    public void OnADS(InputValue value)
    {
        adsHeld = value.isPressed;
    }

    private void SetADS()
    {
        if (canADS)
        {
            isADS = true;
            targetPos = adsPos;
            scr_UIManager.Instance.ADS();
        }
        else
        {
            isADS = false;
            targetPos = hipPos;
            scr_UIManager.Instance.NotADS();
        }
    }

    private void SmoothTransition()
    {
        var vel = Vector3.zero;
        curPos = Vector3.SmoothDamp(curPos, targetPos, ref vel, adsSpeed);
        transform.parent.localPosition = curPos;
    }

    public void ResetADS()
    {

        adsHeld = false;
    }

}
