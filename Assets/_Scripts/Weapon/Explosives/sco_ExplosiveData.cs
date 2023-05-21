using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sco_ExplosiveData : ScriptableObject 
{

    public string explosiveName;
    public GameObject explosivePrefab;
    public ParticleSystem explosionParticles;

    [Header("General")]
    public float damage;
    public float radius;
    public LayerMask ground;

    [Header("Throwing")]
    public bool throwable;
    public float throwDistance;
}
