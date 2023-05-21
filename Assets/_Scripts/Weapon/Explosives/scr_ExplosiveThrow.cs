using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_ExplosiveThrow : scr_ExplosiveBehaviour
{


    public void Throw()
    {
        explosive.rb.AddForce(explosive.transform.forward * explosive.data.throwDistance + Vector3.up * explosive.data.throwDistance / 2, ForceMode.Impulse);
        StartCoroutine(explosive.exExplosion.Explode(explosive.data.delay));

    }
}
