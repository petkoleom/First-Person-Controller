using Newtonsoft.Json.Serialization;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ExplosiveExplosion : scr_ExplosiveBehaviour, itf_Damage
{
    private Collider[] hits = new Collider[50];
    [SerializeField]
    private LayerMask ignore;
    public IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var explosionOrigin = transform.localPosition;

        Instantiate(explosive.data.explosionParticles, explosionOrigin, Quaternion.identity);
        int hitSize = Physics.OverlapSphereNonAlloc(explosionOrigin, explosive.data.radius, hits, ignore);
        for (int i = 0; i < hitSize; i++)
        {
            if (hits[i].TryGetComponent(out itf_Damage itfDamage))
            {
                float distance = Vector3.Distance(explosionOrigin, hits[i].transform.position);
                if(!Physics.Raycast(explosionOrigin, (hits[i].transform.position - explosionOrigin).normalized, distance - .1f, explosive.data.ground))
                {
                    //Debug.Log($"would hit {itfDamage} for {Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / radius))}");                   
                    scr_UIManager.Instance.ShowHitmarker(itfDamage.TakeDamage(Mathf.Lerp(explosive.data.damage, 1, distance / explosive.data.radius)));
                }
            }
        }
        if(transform.parent != null) Destroy(transform.parent);
        else Destroy(gameObject);
    }

    public int TakeDamage(float damage)
    {
        StartCoroutine(Explode(.1f));
        return 0;
    }
}
