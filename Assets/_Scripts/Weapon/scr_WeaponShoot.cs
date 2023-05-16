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

    public GameObject bulletHole;

    private void Start()
    {
        allowFire = true;

        //ammo
        weapon.data.ammoInMag = weapon.data.magSize;
        weapon.data.ammoInReserve = weapon.data.ammoReserve;
        scr_UIManager.Instance.UpdateAmmo(weapon.data.ammoInMag.ToString() + " / " + weapon.data.ammoInReserve.ToString());

    }

    private void Update()
    {
        if (weapon.data.fireMode == FireMode.Auto && shootHeld && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    public void OnShoot(InputValue value)
    {
        shootHeld = value.isPressed;
        if (canShoot && shootHeld)
            StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        shooting = true;
        allowFire = false;

        RaycastShot();
        
        weapon.data.ammoInMag--;

        weapon.weaponRecoil.Recoil();

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

    private void RaycastShot()
    {
        var shot = Physics.Raycast(transform.parent.position, transform.parent.forward, out hit);
        if (shot)
        {
            GameObject bh = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity);
            bh.transform.LookAt(hit.point + hit.normal);
            bh.transform.parent = hit.transform;
            Destroy(bh, 15);
            var target = hit.transform.GetComponent<itf_Damage>();
            if (target != null)
            {
                target.TakeDamage(weapon.data.damage);

            }
        }
    }

    public void ResetShooting()
    {
        shootHeld = false;
        allowFire = true;
    }

    private bool canShoot { get { return allowFire && weapon.data.ammoInMag > 0 && weapon.state != WeaponState.Reloading; } }

}
