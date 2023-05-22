using System.Collections;
using UnityEngine;

public class scr_ExplosiveExplosion : scr_ExplosiveBehaviour, itf_Damage
{
    private Collider[] hits = new Collider[50];
    [SerializeField]
    private LayerMask ignore;
    public IEnumerator Explode(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        var _explosionOrigin = transform.localPosition;

        Instantiate(Explosive.Data.Explosion, _explosionOrigin, Quaternion.identity);
        int hitSize = Physics.OverlapSphereNonAlloc(_explosionOrigin, Explosive.Data.Radius, hits, ignore);
        for (int i = 0; i < hitSize; i++)
        {
            if (hits[i].TryGetComponent(out itf_Damage _itfDamage))
            {
                float distance = Vector3.Distance(_explosionOrigin, hits[i].transform.position);
                if (!Physics.Raycast(_explosionOrigin, (hits[i].transform.position - _explosionOrigin).normalized, distance - .1f, Explosive.Data.Ground))
                {
                    //Debug.Log($"would hit {itfDamage} for {Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / radius))}");                   
                    scr_UIManager.Instance.ShowHitmarker(_itfDamage.TakeDamage(Mathf.Lerp(Explosive.Data.Damage, 1, distance / Explosive.Data.Radius)));
                }
            }
        }
        if (transform.parent != null) Destroy(transform.parent);
        else Destroy(gameObject);
    }

    public int TakeDamage(float _damage)
    {
        StartCoroutine(Explode(.1f));
        return 0;
    }
}
