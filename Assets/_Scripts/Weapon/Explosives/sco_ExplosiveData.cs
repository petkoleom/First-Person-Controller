using UnityEngine;

[CreateAssetMenu]
public class sco_ExplosiveData : ScriptableObject
{

    public string Name;
    public GameObject Prefab;
    public ParticleSystem Explosion;

    [Header("General")]
    public float Damage;
    public float Radius;
    public LayerMask Ground;

    [Header("Throwing")]
    public bool Throwable;
    public float ThrowDistance;
    public float Delay;
}
