using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponShootBullet : scr_WeaponShoot
{
    public override void DecideShotType()
    {
        RaycastShot();
    }
}
