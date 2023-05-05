using UnityEngine;
using UnityEngine.InputSystem;

public class scr_WeaponShoot : scr_WeaponBehaviour
{
    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }
    public FireMode fireMode;

    private RaycastHit hit;

    public bool shootHeld;

    private void Start()
    {
        shootHeld = false;
    }

    private void Update()
    {
        if(fireMode == FireMode.Auto && shootHeld)
        {
            Shoot();
        }
    }

    public void OnShoot(InputValue value)
    {
        if (!shootHeld)
        {
            shootHeld = true;
            Shoot();
        }
        else
        {
            shootHeld = false;
            StopShooting();
        }
    }

    public void Shoot()
    {
        var shot = Physics.Raycast(transform.parent.position, transform.parent.forward, out hit);
        if (shot)
        {
            print(hit.collider.name);
        }
    }

}
