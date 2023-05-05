using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_WeaponReload : scr_WeaponBehaviour
{
    public bool reloading;

    public void OnReload(InputValue value)
    {
        if(weapon.data.ammoInMag < weapon.data.magSize && weapon.data.ammoInReserve > 0)
            StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(weapon.data.reloadSpeed);

        int amountNeeded = weapon.data.magSize - weapon.data.ammoInMag;

        if(amountNeeded >= weapon.data.ammoInReserve)
        {
            weapon.data.ammoInMag += weapon.data.ammoInReserve;
            weapon.data.ammoInReserve = 0;
        }
        else
        {
            weapon.data.ammoInMag = weapon.data.magSize;
            weapon.data.ammoInReserve -= amountNeeded;
        }

        reloading = false;
        scr_UIManager.Instance.UpdateAmmo(weapon.data.ammoInMag.ToString() + " / " + weapon.data.ammoInReserve.ToString());

    }
}
