using UnityEngine;

public class scr_ExplosiveBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Explosive Explosive;
    public void Initialize(scr_Explosive _explosive)
    {
        this.Explosive = _explosive;
    }
}
