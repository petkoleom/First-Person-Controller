using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_Weapon : MonoBehaviour
{
    public sco_WeaponData data;

    public sco_WeaponData[] loadout = new sco_WeaponData[2];
    private int currentWeapon = 0;

    private Transform currentWeaponChild;

    public scr_WeaponShoot weaponShoot { get; set; }
    public scr_WeaponReload weaponReload { get; set; }
    public scr_WeaponAim weaponAim { get; set; }
    public scr_WeaponRecoil weaponRecoil { get; set; }
    public scr_WeaponSway weaponSway { get; set; }
    public scr_WeaponBob weaponBob { get; set; }

    private void Awake()
    {
        foreach (var item in loadout)
        {
            Instantiate(item.weaponPrefab, transform);
        }


        currentWeaponChild = transform.GetChild(currentWeapon);

        Initialize();


        EnableWeapon();

    }

    private void Initialize()
    {
        weaponShoot = currentWeaponChild.GetComponent<scr_WeaponShoot>();
        weaponReload = currentWeaponChild.GetComponent<scr_WeaponReload>();
        weaponAim = currentWeaponChild.GetComponent<scr_WeaponAim>();
        weaponRecoil = GetComponent<scr_WeaponRecoil>();
        weaponSway = GetComponent<scr_WeaponSway>();
        weaponBob = GetComponent<scr_WeaponBob>();


        if (weaponShoot != null) weaponShoot.Initialize(this);
        if (weaponReload != null) weaponReload.Initialize(this);
        if (weaponAim != null) weaponAim.Initialize(this);
        if(weaponSway != null) weaponSway.Initialize(this);
        if(weaponBob != null) weaponBob.Initialize(this);
        if (weaponRecoil != null)
        {
            weaponRecoil.Initialize(this);
            weaponRecoil.Initialize();
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
        if (weaponReload.reloading)
        {
            state = WeaponState.Reloading;
        }
        else if (weaponAim.isADS)
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

    public void OnSwitch(InputValue value)
    {
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {

        StopAllCoroutines();
        weaponReload.CancelReload();
        weaponShoot.ResetShooting();
        weaponAim.ResetADS();
        currentWeapon = 1 - currentWeapon;
        currentWeaponChild = transform.GetChild(currentWeapon);
        data = loadout[currentWeapon];


        EnableWeapon();
        Initialize();
        scr_UIManager.Instance.UpdateAmmo(data.ammoInMag.ToString() + " / " + data.ammoInReserve.ToString());

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
