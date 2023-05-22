using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Weapon Weapon;
    public void Initialize(scr_Weapon _weapon)
    {
        this.Weapon = _weapon;
    }
}
