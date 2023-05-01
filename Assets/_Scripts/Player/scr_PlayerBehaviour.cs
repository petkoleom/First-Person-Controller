using UnityEngine;

public class scr_PlayerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Player player;
    public void Initialize(scr_Player player)
    {
        this.player = player;
    }
}
