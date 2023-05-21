using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ExplosiveTrigger : scr_ExplosiveBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(explosive.exExplosion.Explode(.3f));
    }
}
