using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ExplosiveBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Explosive explosive;
    public void Initialize(scr_Explosive explosive)
    {
        this.explosive = explosive;
    }
}
