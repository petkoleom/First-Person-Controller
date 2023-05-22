using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(scr_WeaponReload))]
public class scr_WeaponShoot : scr_WeaponBehaviour
{
    private bool shootHeld, allowFire;

    private RaycastHit hit;

    [SerializeField]
    private GameObject bulletHole;

    private Transform fpsCam;

    public void Start()
    {
        allowFire = true;

        //ammo
        Weapon.Data.AmmoInMag = Weapon.Data.MagSize;
        Weapon.Data.AmmoInReserve = Weapon.Data.AmmoReserve;
        scr_UIManager.Instance.UpdateAmmo(Weapon.Data.AmmoInMag.ToString() + " / " + Weapon.Data.AmmoInReserve.ToString());
        fpsCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (Weapon.Data.FireMode == FireMode.Auto && shootHeld && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    public void OnShoot(InputValue _value)
    {
        shootHeld = _value.isPressed;
        if (canShoot && shootHeld)
            StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        allowFire = false;
        DecideShotType();
        Weapon.Data.AmmoInMag--;
        Weapon.Recoil.Recoil();
        scr_UIManager.Instance.UpdateAmmo(Weapon.Data.AmmoInMag.ToString() + " / " + Weapon.Data.AmmoInReserve.ToString());

        yield return new WaitForSeconds(60f / Weapon.Data.FireRate);

        allowFire = true;
        if (Weapon.Data.AmmoInMag == 0 && Weapon.Data.AmmoInReserve > 0)
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(Weapon.Reload.Reload());
        }
    }

    public virtual void DecideShotType()
    {
        RaycastShot();
    }

    public void RaycastShot()
    {
        var _trajectory = fpsCam.position + fpsCam.forward * 1000f;
        if (Weapon.state != WeaponState.ADS)
        {
            _trajectory += Random.Range(-Weapon.Data.Spread, Weapon.Data.Spread) * fpsCam.up;
            _trajectory += Random.Range(-Weapon.Data.Spread, Weapon.Data.Spread) * fpsCam.right;
        }
        _trajectory -= fpsCam.position;
        _trajectory.Normalize();
        var _shot = Physics.Raycast(fpsCam.position, _trajectory, out hit);
        if (_shot)
        {
            GameObject _bulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity);
            _bulletHole.transform.LookAt(hit.point + hit.normal);
            _bulletHole.transform.parent = hit.transform;
            Destroy(_bulletHole, 60);
            var _target = hit.transform.GetComponent<itf_Damage>();
            if (_target != null)
            {
                scr_UIManager.Instance.ShowHitmarker(_target.TakeDamage(CalculateDamage(hit.distance)));
            }
        }
    }

    public void ResetShooting()
    {
        shootHeld = false;
        allowFire = true;
    }

    private bool canShoot { get { return allowFire && Weapon.Data.AmmoInMag > 0 && Weapon.state != WeaponState.Reloading; } }

    private float CalculateDamage(float _distance)
    {
        int _roundedDistance = Mathf.FloorToInt(_distance);
        var _damage = Weapon.Data.DamageCurve.Evaluate(_roundedDistance);
        return _damage;
    }



}
