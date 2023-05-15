using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_WeaponRecoil : scr_WeaponBehaviour
{

    private Vector3 currentRot, targetRot, currentPos, targetPos, originPos;

    [SerializeField]
    private float snapiness, returnAmount;
    private float kickback, recoilX, recoilY, recoilZ;

    [SerializeField]
    private Transform camRecoil;

    private Transform currentWeapon;

    public void Initialize()
    {
        currentWeapon = weapon.GetCurrentWeapon();
        originPos = currentWeapon.transform.localPosition;
        kickback = weapon.data.kickback;
        recoilX = weapon.data.visualRecoil.x;
        recoilY = weapon.data.visualRecoil.y;
        recoilZ = weapon.data.visualRecoil.z;
    }

    private void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * returnAmount);
        currentRot = Vector3.Slerp(currentRot, targetRot, Time.fixedDeltaTime * snapiness);
        currentWeapon.transform.localRotation = Quaternion.Euler(currentRot);
        Kickback();



    }

    private void Kickback()
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
