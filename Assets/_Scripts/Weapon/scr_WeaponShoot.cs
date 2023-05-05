using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_WeaponReload))]
public class scr_WeaponShoot : scr_WeaponBehaviour
{
    private bool shootHeld;

    private RaycastHit hit;
    private bool allowFire;

    public bool shooting;

    private void Start()
    {
        shootHeld = false;
        allowFire = true;

        //ammo
        weapon.data.ammoInMag = weapon.data.magSize;
        weapon.data.ammoInReserve = weapon.data.ammoReserve;
        scr_UIManager.Instance.UpdateAmmo(weapon.data.ammoInMag.ToString() + " / " + weapon.data.ammoInReserve.ToString());
    }

    
    private void Update()
    {
        if(weapon.data.fireMode == FireMode.Auto && shootHeld && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    public void OnShoot(InputValue value)
    {
        if (!shootHeld && canShoot)
        {
            shootHeld = true;
            StartCoroutine(Shoot());
        }
        else
        {
            shootHeld = false;
        }
    }

    public IEnumerator Shoot()
    {
        shooting = true;
        allowFire = false;
        var shot = Physics.Raycast(transform.parent.position, transform.parent.forward, out hit);
        if (shot)
        {
            var target = hit.transform.GetComponent<itf_Damageable>();
            if(target != null)
            {
                target.TakeDamage(weapon.data.damage);
                
            }
            
        }
        weapon.data.ammoInMag--;

        scr_UIManager.Instance.UpdateAmmo(weapon.data.ammoInMag.ToString() + " / " + weapon.data.ammoInReserve.ToString());

        yield return new WaitForSeconds(60f / weapon.data.fireRate);

        shooting = false;
        allowFire = true;

        if (weapon.data.ammoInMag == 0 && weapon.data.ammoInReserve > 0)
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(weapon.weaponReload.Reload());
        }

    }

    private bool canShoot { get { return allowFire && weapon.data.ammoInMag > 0 && weapon.state != WeaponState.Reloading; } }

}
