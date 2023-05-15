using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sco_WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;

    [Header("General")]
    public int fireRate;
    public int damage;
    public FireMode fireMode;

    [Header("Ammo")]
    public int magSize;
    public int ammoReserve;
    [HideInInspector]
    public int ammoInMag;
    [HideInInspector]
    public int ammoInReserve;
    public float reloadSpeed;

    [Header("Recoil")]
    public float horizontalRecoil;
    public float verticalRecoil;
    public float kickback;
    public Vector3 visualRecoil;


}
public enum FireMode
{
    Single,
    Burst,
    Auto
}
