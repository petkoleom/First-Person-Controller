using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface itf_Damage
{
    public void TakeDamage(float damage);
}

public interface itf_Pickup
{
    public void Pickup(GameObject pickup);
}
