using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Weapon : MonoBehaviour
{
    private void Update()
    {
        StateHandler();
    }

    #region - States -

    public WeaponState state { get; set; }

    private void StateHandler()
    {

    }

    #endregion
}


[Serializable]
public enum WeaponState
{
    Idle,
    Moving,
    Shooting,
    Aiming,
    Reloading
}
