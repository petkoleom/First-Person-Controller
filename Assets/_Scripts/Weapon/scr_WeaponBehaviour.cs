using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Weapon weapon;
    public void Initialize(scr_Weapon weapon)
    {
        this.weapon = weapon;
    }
}
