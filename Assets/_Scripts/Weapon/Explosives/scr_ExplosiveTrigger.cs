using UnityEngine;

public class scr_ExplosiveTrigger : scr_ExplosiveBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        StartCoroutine(Explosive.Explosion.Explode(.3f));
    }
}
