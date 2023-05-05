using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sco_WeaponData : ScriptableObject
{
    //public GameObject weapon;

    public int fireRate;
    public int damage;

    public int magSize;
    public int ammoReserve;
    [HideInInspector]
    public int ammoInMag;
    [HideInInspector]
    public int ammoInReserve;

    public float reloadSpeed;

    public FireMode fireMode;
}
public enum FireMode
{
    Single,
    Burst,
    Auto
}
