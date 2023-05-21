using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Target : MonoBehaviour, itf_Damage
{
    private float health = 100;

    public int TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            
            Destroy(gameObject, .1f);
            return 2;
        }

        return 1;
    }


}
