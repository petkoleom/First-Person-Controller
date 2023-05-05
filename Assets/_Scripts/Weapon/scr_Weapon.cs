using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_Weapon : MonoBehaviour
{
    public sco_WeaponData data;

    public sco_WeaponData[] loadout = new sco_WeaponData[2];
    public int currentWeapon = 0;

    private Transform currentWeaponChild;

    public scr_WeaponShoot weaponShoot { get; set; }
    public scr_WeaponReload weaponReload { get; set; }

    private void Awake()
    {
        currentWeaponChild = transform.GetChild(currentWeapon);

        Initialize();


        EnableWeapon();
    }

    private void Initialize()
    {
        weaponShoot = currentWeaponChild.GetComponent<scr_WeaponShoot>();
        weaponReload = currentWeaponChild.GetComponent<scr_WeaponReload>();


        if (weaponShoot != null) weaponShoot.Initialize(this);
        if (weaponReload != null) weaponReload.Initialize(this);

    }

    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public WeaponState state { get; set; }

    private void StateHandler()
    {
        if (weaponShoot.shooting)
        {
            state = WeaponState.Shooting;
        }
        else if(weaponReload.reloading)
        {
            state = WeaponState.Reloading;
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

    public void OnSwitchWeapon(InputValue value)
    {
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        currentWeapon = currentWeapon == 0 ? 1 : 0;
        currentWeaponChild = transform.GetChild(currentWeapon);
        data = loadout[currentWeapon];
        EnableWeapon();
        Initialize();
        scr_UIManager.Instance.UpdateAmmo(data.ammoInMag.ToString() + " / " + data.ammoInReserve.ToString());

    }

    #endregion
}


[Serializable]
public enum WeaponState
{
    Idle,
    Moving,
    Shooting,
    Aiming,
    Reloading
}
