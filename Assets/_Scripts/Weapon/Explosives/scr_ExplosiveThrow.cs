using UnityEngine;

public class scr_ExplosiveThrow : scr_ExplosiveBehaviour
{
    public void Throw()
    {
        Explosive.Rb.AddForce(Explosive.transform.forward * Explosive.Data.ThrowDistance + Vector3.up * Explosive.Data.ThrowDistance / 2, ForceMode.Impulse);
        StartCoroutine(Explosive.Explosion.Explode(Explosive.Data.Delay));

    }
}
