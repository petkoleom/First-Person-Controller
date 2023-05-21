using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponShootPellet : scr_WeaponShoot
{

    private int pelletAmount = 6;

    public override void DecideShotType()
    {
        for (int i = 0; i < pelletAmount; i++)
        {
            RaycastShot();
        }
    }
}
