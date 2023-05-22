using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_Weapon : MonoBehaviour
{
    public sco_WeaponData Data;

    public sco_WeaponData[] Loadout = new sco_WeaponData[2];
    private int currentWeapon = 0;

    private Transform currentWeaponChild;

    public scr_WeaponShoot Shoot { get; set; }
    public scr_WeaponReload Reload { get; set; }
    public scr_WeaponAim Aim { get; set; }
    public scr_WeaponRecoil Recoil { get; set; }
    public scr_WeaponSway Sway { get; set; }
    public scr_WeaponBob Bob { get; set; }

    private void Awake()
    {
        foreach (var _item in Loadout)
        {
            Instantiate(_item.Prefab, transform);
        }
        currentWeaponChild = transform.GetChild(currentWeapon);
        Initialize();
        EnableWeapon();
    }

    private void Initialize()
    {
        Shoot = currentWeaponChild.GetComponent<scr_WeaponShoot>();
        Reload = currentWeaponChild.GetComponent<scr_WeaponReload>();
        Aim = currentWeaponChild.GetComponent<scr_WeaponAim>();
        Recoil = GetComponent<scr_WeaponRecoil>();
        Sway = GetComponent<scr_WeaponSway>();
        Bob = GetComponent<scr_WeaponBob>();

        if (Shoot != null) Shoot.Initialize(this);
        if (Reload != null) Reload.Initialize(this);
        if (Aim != null) Aim.Initialize(this);
        if (Sway != null) Sway.Initialize(this);
        if (Bob != null) Bob.Initialize(this);
        if (Recoil != null)
        {
            Recoil.Initialize(this);
            Recoil.Initialize();
        }
    }

    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public WeaponState state { get; set; }

    private void StateHandler()
    {
        if (Reload.IsReloading)
        {
            state = WeaponState.Reloading;
        }
        else if (Aim.IsADS)
        {
            state = WeaponState.ADS;
        }
        else
        {
            state = WeaponState.Idle;
        }

        scr_UIManager.Instance.UpdateWeaponState(state);
    }

    #endregion

    #region - Loadout -

    private void EnableWeapon()
    {
        transform.GetChild(1 - currentWeapon).gameObject.SetActive(false);
        currentWeaponChild.gameObject.SetActive(true);
    }

    public void OnSwitch(InputValue _value)
    {
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        StopAllCoroutines();
        Reload.CancelReload();
        Shoot.ResetShooting();
        Aim.ResetADS();
        currentWeapon = 1 - currentWeapon;
        currentWeaponChild = transform.GetChild(currentWeapon);
        Data = Loadout[currentWeapon];
        EnableWeapon();
        Initialize();
        scr_UIManager.Instance.UpdateAmmo(Data.AmmoInMag.ToString() + " / " + Data.AmmoInReserve.ToString());
    }

    public Transform GetCurrentWeapon()
    {
        return currentWeaponChild;
    }

    #endregion
}


[Serializable]
public enum WeaponState
{
    Idle,
    Moving,
    ADS,
    Reloading
}
