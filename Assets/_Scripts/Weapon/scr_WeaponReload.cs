using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_WeaponShoot))]
public class scr_WeaponReload : scr_WeaponBehaviour
{
    public bool IsReloading { get; private set; }

    public void OnReload(InputValue _value)
    {
        if(Weapon.Data.AmmoInMag < Weapon.Data.MagSize && Weapon.Data.AmmoInReserve > 0)
            StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(Weapon.Data.ReloadSpeed);

        int _amountNeeded = Weapon.Data.MagSize - Weapon.Data.AmmoInMag;

        if(_amountNeeded >= Weapon.Data.AmmoInReserve)
        {
            Weapon.Data.AmmoInMag += Weapon.Data.AmmoInReserve;
            Weapon.Data.AmmoInReserve = 0;
        }
        else
        {
            Weapon.Data.AmmoInMag = Weapon.Data.MagSize;
            Weapon.Data.AmmoInReserve -= _amountNeeded;
        }

        IsReloading = false;
        scr_UIManager.Instance.UpdateAmmo(Weapon.Data.AmmoInMag.ToString() + " / " + Weapon.Data.AmmoInReserve.ToString());
    }

    public void CancelReload()
    {
        IsReloading = false;
    }
}
