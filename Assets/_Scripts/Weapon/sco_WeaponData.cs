using UnityEngine;

[CreateAssetMenu]
public class sco_WeaponData : ScriptableObject
{
    public string Name;
    public GameObject Prefab;

    [Header("General")]
    public int FireRate;
    public AnimationCurve DamageCurve;
    public float Spread;
    public FireMode FireMode;

    [Header("Ammo")]
    public int MagSize;
    public int AmmoReserve;
    [HideInInspector]
    public int AmmoInMag;
    [HideInInspector]
    public int AmmoInReserve;
    public float ReloadSpeed;


    [Header("Recoil")]
    public float HorizontalRecoil;
    public float VerticalRecoil;
    public float Kickback;
    public Vector3 VisualRecoil;


    [Header("ADS")]
    public float ADSSpeed;

}
public enum FireMode
{
    Single,
    Burst,
    Auto
}
