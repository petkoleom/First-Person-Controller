using UnityEngine;

public class scr_PlayerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public scr_Player Player;
    public void Initialize(scr_Player player)
    {
        this.Player = player;
    }
}
