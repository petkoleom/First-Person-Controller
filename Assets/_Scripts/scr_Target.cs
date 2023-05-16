using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Target : MonoBehaviour, itf_Damage
{
    private float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
            Destroy(gameObject);    
    }


}
