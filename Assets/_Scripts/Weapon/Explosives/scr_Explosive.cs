using UnityEngine;
using UnityEngine.InputSystem;

public class scr_Explosive : MonoBehaviour
{
    public sco_ExplosiveData data;

    public Rigidbody rb;
    public scr_ExplosiveThrow exThrow { get; set; }
    public scr_ExplosivePlace exPlace { get; set; }
    public scr_ExplosiveExplosion exExplosion { get; set; }
   
    public scr_ExplosiveTrigger exTrigger { get; set; }


    private void Awake()
    {
        
    }

    public void OnExplosive(InputValue value)
    {
        GameObject go = Instantiate(data.explosivePrefab, transform.position, Quaternion.identity);
        

        exExplosion = go.GetComponent<scr_ExplosiveExplosion>();
        exExplosion.Initialize(this);

        go.TryGetComponent<Rigidbody>(out rb);

        exThrow = go.GetComponent<scr_ExplosiveThrow>();
        exPlace = go.GetComponent<scr_ExplosivePlace>();
        exTrigger = go.transform.GetComponentInChildren<scr_ExplosiveTrigger>();

        if (exThrow != null)
        {
            exThrow.Initialize(this);
            exThrow.Throw();
        }
        if (exPlace != null) 
        { 
            exPlace.Initialize(this);
            exPlace.Place();
        }
        if(exTrigger != null)
        {
            exTrigger.Initialize(this);
        }


    }
}
