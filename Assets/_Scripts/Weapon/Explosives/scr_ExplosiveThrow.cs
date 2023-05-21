using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scr_ExplosiveThrow : scr_ExplosiveBehaviour
{


    public void Throw()
    {
        explosive.rb.AddForce(explosive.transform.forward * 10, ForceMode.Impulse);
        StartCoroutine(explosive.exExplosion.Explode(2));

    }
}
