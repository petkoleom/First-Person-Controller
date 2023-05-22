using UnityEngine;
using UnityEngine.InputSystem;

public class scr_Explosive : MonoBehaviour
{
    public sco_ExplosiveData Data;

    public Rigidbody Rb;
    public scr_ExplosiveThrow Throw { get; set; }
    public scr_ExplosivePlace Place { get; set; }
    public scr_ExplosiveExplosion Explosion { get; set; }

    public scr_ExplosiveTrigger Trigger { get; set; }
    public void OnExplosive(InputValue _value)
    {
        GameObject _go = Instantiate(Data.Prefab, transform.position, Quaternion.identity);


        Explosion = _go.GetComponent<scr_ExplosiveExplosion>();
        Explosion.Initialize(this);

        _go.TryGetComponent<Rigidbody>(out Rb);

        Throw = _go.GetComponent<scr_ExplosiveThrow>();
        Place = _go.GetComponent<scr_ExplosivePlace>();
        Trigger = _go.transform.GetComponentInChildren<scr_ExplosiveTrigger>();

        if (Throw != null)
        {
            Throw.Initialize(this);
            Throw.Throw();
        }
        if (Place != null)
        {
            Place.Initialize(this);
            Place.Place();
        }
        if (Trigger != null)
        {
            Trigger.Initialize(this);
        }


    }
}
