using UnityEngine;

public class scr_WeaponRecoil : scr_WeaponBehaviour
{

    private Vector3 currentRot, targetRot, currentPos, targetPos, originPos;

    [SerializeField]
    private float snapiness, returnAmount, kickback, recoilX, recoilY, recoilZ;

    [SerializeField]
    private Transform camRecoil, currentWeapon;

    public void Initialize()
    {
        currentWeapon = Weapon.GetCurrentWeapon();
        originPos = currentWeapon.transform.localPosition;
        kickback = Weapon.Data.Kickback;
        recoilX = Weapon.Data.VisualRecoil.x;
        recoilY = Weapon.Data.VisualRecoil.y;
        recoilZ = Weapon.Data.VisualRecoil.z;
    }

    private void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * returnAmount);
        currentRot = Vector3.Slerp(currentRot, targetRot, Time.fixedDeltaTime * snapiness);
        currentWeapon.transform.localRotation = Quaternion.Euler(currentRot);
        CalculateKickback();
    }

    private void CalculateKickback()
    {
        targetPos = Vector3.Lerp(targetPos, originPos, Time.deltaTime * returnAmount);
        currentPos = Vector3.Lerp(currentPos, targetPos, Time.fixedDeltaTime * snapiness);
        currentWeapon.transform.localPosition = currentPos;
    }

    public void Recoil()
    {
        targetPos -= new Vector3(0, 0, kickback);
        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }





}
